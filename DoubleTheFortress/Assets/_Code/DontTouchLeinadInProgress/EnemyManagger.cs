using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagger : MonoBehaviour
{
    public static EnemyManagger Instance;
    public float maxDistance;

    [SerializeField]
    GameObject Zombie;

    private GameObject temporalZ;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Pooling.Preload(Zombie, 10);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnSpawn();
        }
    }

    public void OnDie()
    {
        Despawn(Zombie, temporalZ);
    }

    public void OnSpawn()
    {
        Vector3 vector = SpawnPosition();

        temporalZ = Pooling.GetObject(Zombie);
        temporalZ.transform.position = vector;
    }

    Vector3 SpawnPosition()
    {
        return RouteManagger.Instance.RandomRoute().Peek().transform.position;
    }

    void Despawn(GameObject primitive, GameObject go)
    {
        Pooling.RecicleObject(primitive, go);
    }
}
