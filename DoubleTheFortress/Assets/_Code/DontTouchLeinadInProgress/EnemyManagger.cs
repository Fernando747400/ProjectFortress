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
        Pooling.Preload(Zombie, 1);
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
        RouteManagger.Instance.RandomNum();
        Vector3 vector = SpawnPosition().Peek().transform.position;

        GameObject temporal;
        temporal = Pooling.GetObject(Zombie);
        temporal.transform.position = vector;
    }

    public Queue<Transform> SpawnPosition()
    {
        return RouteManagger.Instance.RandomRoute();
    }

    public void Despawn(GameObject primitive, GameObject temporalObject)
    {
        Pooling.RecicleObject(primitive, temporalObject);
    }
}
