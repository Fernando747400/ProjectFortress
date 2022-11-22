using System;
using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Unity.Mathematics;
using UnityEngine;

public class DebugGunFire :  GeneralAgressor
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] private LayerMask layers;
    [SerializeField] private GameObject shootParticle;
    private ParticleSystem _shootParticleSystem;
    [SerializeField] private GameObject hitParticle;
    private ParticleSystem _hitParticleSystem;
    [SerializeField]private GameObject particleOffset;

    [Tooltip("Cooldown Time in seconds")] [SerializeField]
    protected float cooldown = 5;
    private float _travelTime;
    protected bool canFire = true;
    private Vector3 _hitPosition;
    
    //saves the gun position when hitscan was fired
    private Vector3 _savedFirePosition;

    [Header("Audios")]
    public AudioClip shootSound;

    public float CoolDown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }
    private void Start()
    {
        Prepare();
    }

    protected virtual  void Update()
    {
        CheckInput();
    }
    
    
    protected virtual void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)  | canFire)
        {
            FireHitScan();
        }
    }

    protected virtual void FireHitScan()
    {
        canFire = false;
        StopAllCoroutines();
        _savedFirePosition = transform.position;
        
        PlayParticles("Musket_SmokeParticle",shootParticle,_shootParticleSystem,particleOffset.transform.position,transform.rotation);
        PlayAudio(shootSound, 0.5f);
        
        if (!Physics.Raycast( transform.position, transform.forward, out RaycastHit hitScan, maxDistance,
                Physics.DefaultRaycastLayers)) return;
        _hitPosition = hitScan.point;
        _travelTime = hitScan.distance / (bulletSpeed) * Time.fixedDeltaTime;
        StartCoroutine(CorWaitForTravel());
    }
    
    private IEnumerator CorWaitForTravel()
    {
        // Debug.Log("waiting " + _travelTime + " seconds");
        yield return new WaitForSeconds(_travelTime);
        FireSimulated();
    }

    protected virtual void FireSimulated()
    {
        StopAllCoroutines();
        Vector3 simulatedHitPos = _hitPosition - _savedFirePosition;
        Physics.Raycast(_savedFirePosition, simulatedHitPos.normalized,out RaycastHit simulatedHit, maxDistance, layers);
        PlayParticles("Musket_HitParticle",hitParticle,_hitParticleSystem,simulatedHit.point, quaternion.identity);

        if (!TryGetGeneralTarget(simulatedHit.collider.gameObject)) return;
        simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
    }

    protected virtual IEnumerator CorWaitForCoolDown()
    {
        Debug.Log("waiting " + cooldown);
        yield return new WaitForSeconds(cooldown);
        canFire = true;
        Debug.Log("done waiting");

    }
    void PlayParticles(string name,GameObject particle,
        ParticleSystem particleSystem, Vector3 pos, Quaternion rot)
    {
        if (particleSystem == null)
        {
            particleSystem = GameObject.Find(name).GetComponent<ParticleSystem>();
            
        }
        particle.transform.position = pos;
        
        particle.transform.rotation = rot;
        particleSystem.Clear();
        particleSystem.Play();
    }

    private void Prepare()
    {
        try
        {
            _shootParticleSystem = shootParticle.GetComponent<ParticleSystem>();
        }
        catch { Debug.LogWarning("Missing shootParticleSystem");}
        
        try
        { 
            _hitParticleSystem = hitParticle.GetComponent<ParticleSystem>();
        }
        catch { Debug.LogWarning("Missing hitParticleSystem");}
    }

    private void PlayAudio(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
    }
}