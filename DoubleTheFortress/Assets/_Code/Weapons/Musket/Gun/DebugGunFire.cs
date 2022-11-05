using System;
using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class DebugGunFire :  GeneralAgressor
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    // [SerializeField] private GameObject hitMarkerRed;
    [SerializeField] private GameObject hitMarkerBlue;
    [SerializeField] private LayerMask layers;
    
    [SerializeField] private GameObject shootParticle;
    private ParticleSystem _shootParticleSystem;
    [SerializeField] private GameObject hitParticle;
    private ParticleSystem _hitParticleSystem;
    public GameObject _particleOffset;

    [Tooltip("Cooldown Time in seconds")] [SerializeField]
    private float cooldown = 5;
    private float elapsedTime = 0;
    
    private float _bulletMass;
    private float _travelTime;
    protected bool canFire = true;
    private Vector3 _hitPosition;
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 _savedFirePosition;

    [Header("Audios")]
    public AudioClip _ShootSound;

    private void Start()
    {
        Prepare();
        elapsedTime = cooldown;
    }

    private void FixedUpdate()
    {
        if (elapsedTime <= cooldown) elapsedTime++;
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
        if (elapsedTime < cooldown) return;
        
        canFire = false;
        StopAllCoroutines();
        _savedFirePosition = transform.position;
        
        PlayParticles("Musket_SmokeParticle",shootParticle,_shootParticleSystem,_particleOffset.transform.position,transform.localEulerAngles);
        PlayAudio(_ShootSound, 0.5f);
        
        if (!Physics.Raycast( transform.position, transform.forward, out RaycastHit hitScan, maxDistance,
                Physics.DefaultRaycastLayers)) return;
        Debug.DrawLine(transform.position,hitScan.point,Color.red,10f);
        // Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
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
        //Vector3 simulatedHitPos = (_hitPosition - new Vector3(0, _gravity.y / _travelTime / Time.fixedDeltaTime, 0)) - _savedFirePosition;
        Physics.Raycast(_savedFirePosition, simulatedHitPos.normalized,out RaycastHit simulatedHit,
            maxDistance, layers);
        PlayParticles("Musket_HitParticle",hitParticle,_hitParticleSystem,simulatedHit.point, simulatedHit.normal);

        Debug.DrawLine(_savedFirePosition,simulatedHit.point,Color.cyan);
        //Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);

        //StartCoroutine(CorWaitForCooldown());


        if (!TryGetGeneralTarget(simulatedHit.collider.gameObject))
        {
            elapsedTime = 0;
            return;
        }
        simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
        elapsedTime = 0;
    }

    private IEnumerator CorWaitForCooldown()
    {
        Debug.Log("waiting " + cooldown);
        yield return new WaitForSeconds(cooldown);
        canFire = true;
        Debug.Log("done waiting");

    }
    void PlayParticles(string name,GameObject particle, ParticleSystem particleSystem, Vector3 pos, Vector3 rot)
    {
        if (particleSystem == null)
        {
            particleSystem = GameObject.Find(name).GetComponent<ParticleSystem>();
            
        }
        particle.transform.position = pos;
        particle.transform.eulerAngles = rot;
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