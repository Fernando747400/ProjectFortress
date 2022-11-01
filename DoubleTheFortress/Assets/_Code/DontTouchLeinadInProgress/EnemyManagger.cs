using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagger : MonoBehaviour
{
    public static EnemyManagger Instance;
    public float maxDistance;

    [SerializeField]
    public GameObject Zombie;

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

    public void OnSpawn()
    {
        Vector3 vector = SpawnPosition();

        GameObject temporal;
        temporal = Pooling.GetObject(Zombie);
        temporal.transform.position = vector;
    }

    Vector3 SpawnPosition()
    {
        return RouteManagger.Instance.RandomRoute().Peek().transform.position;
    }

    public void Despawn(GameObject primitive, GameObject temporalObject)
    {
        Pooling.RecicleObject(primitive, temporalObject);
    }
}
