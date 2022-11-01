using UnityEngine;

public class HammerParticles : MonoBehaviour
{
    [Header("Depedencies")]
    [SerializeField] private GameObject _hammer;
    [SerializeField] private Hamer_Grab _hammerGrab;
    [SerializeField] private ParticleSystem _particleSystem;
    

    void Start()
    {
        _hammerGrab.ConstructableHitEvent += SpawnParticles;
        _particleSystem.gameObject.transform.parent = null;
    }

    private void SpawnParticles(GameObject other)
    {
        Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(_hammer.transform.position);
        _particleSystem.gameObject.transform.position = pos;
        _particleSystem.Clear();
        _particleSystem.Play();
    }
}
