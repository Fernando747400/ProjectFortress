using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Canon_CanonBall : GeneralAgressor
{
    private Rigidbody _rb;
    private MeshRenderer _rend;
    private Collider _col;
    private Debug_CannonFire _cannonFire;
    
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
        _cannonFire.Reload(this);
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
        Vector3 offset = new Vector3(0, 0, 1.3f);
        transform.position = origin + offset;
        Activate();
        _rb.AddForce(dir.normalized * mag);
    }

    void Explode()
    {
        Deactivate();
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

        try
        {
            _cannonFire = GameObject.Find("Cannon").GetComponent<Debug_CannonFire>();
        }
        catch { Debug.Log("Missing Debug_CanonFire");}
    }
}
