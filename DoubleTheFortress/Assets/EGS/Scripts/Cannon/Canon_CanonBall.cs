using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Canon_CanonBall : GeneralAgressor
{
    [Tooltip("Explosion Radius In Meters")]
    [SerializeField] private float blastRadius = 100f;

    [Tooltip("Layers with which the cannonball can interact")]
    [SerializeField] private LayerMask layers;
    private Rigidbody _rb;
    private MeshRenderer _rend;
    private Collider _col;
    [HideInInspector]public Debug_CannonFire cannonFire;
    [SerializeField]private ParticleSystem[] particleSystems;
    
    [Header("Audio Clips")]
    public AudioClip explosion;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Prepare();
    }

    void Start()
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
        _rend.enabled = false;
        _col.enabled = false;
    }

    private void Deactivate()
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
        _rend.enabled = false;
        _col.enabled = false;
        cannonFire.Reload(this);
        transform.localEulerAngles = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Explode();
    }

    private void Activate()
    {
        _rb.isKinematic = false;
        _rend.enabled = true;
        _col.enabled = true;
    }

    public void Fire(Vector3 origin,Vector3 dir, float mag)
    {
        transform.position = origin;
        transform.eulerAngles = Vector3.zero;
        Activate();
        _rb.AddForce(dir.normalized * mag);
    }

    void Explode()
    {
        foreach (var particle in particleSystems)
        {
            particle.Clear();
            particle.Play();
        }
        PlayAudio(explosion,.5f);
        CastDamage();
        Deactivate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position,blastRadius);
    }

    void CastDamage()
    {
        //Arbitrary Array Initialization
        //should get max number of zombies in the game through layer
        Collider[] hits = Physics.OverlapSphere(transform.position,blastRadius,layers);
        
        foreach (var hit in hits)
        {
            //Debug.Log(hit.name);
            if (!TryGetGeneralTarget(hit.gameObject)) continue;
            hit.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
        }
    }
    
    private void PlayAudio(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
    }

    private void Prepare()
    {
        try
        {
            _rb = GetComponent<Rigidbody>();
        }
        catch { Debug.Log("Missing RigidBody");}
        try
        {
            _rend = GetComponentInChildren<MeshRenderer>();
        }
        catch { Debug.Log("Missing MeshRenderer");}
        try
        {
            _col = GetComponent<Collider>();
        }
        catch { Debug.Log("Missing Collider");}
    }
}
