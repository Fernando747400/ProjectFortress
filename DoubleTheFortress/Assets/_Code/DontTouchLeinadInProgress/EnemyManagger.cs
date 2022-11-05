using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagger : MonoBehaviour
{
    public static EnemyManagger Instance;
    public float maxDistance;
    public float Damage;
    public bool StrongZombie = false;
    public Material DefaultSkin;
    public Material StrongSkin;
    public float ZombieLife = 10;

    [SerializeField]
    public GameObject Zombie;

    [SerializeField]
    private Pooling ZombiePooling;

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
        ZombiePooling.Preload(Zombie, 10);
        GameManager.Instance.StartGameEvent += FirstSpawn;
        StrongZombie = false;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnSpawn();
        //}
    }

    public void OnSpawn()
    {
        Vector3 vector = SpawnPosition().Peek().transform.position;

        GameObject temporal = ZombiePooling.GetObject(Zombie);
        temporal.transform.position = vector;
        temporal.GetComponent<ZombiePursuit>().ZombieDamage = Damage;
        temporal.GetComponent<ZombiePursuit>().MaxHp = ZombieLife;
    }

    public Queue<Transform> SpawnPosition()
    {
        return RouteManagger.Instance.RandomRoute();
    }

    public void Despawn(GameObject primitive, GameObject temporalObject)
    {
        ZombiePooling.RecicleObject(primitive, temporalObject);
    }

    public void SpawnWithDelay(float delay, int numberOfZombies)
    {
        StartCoroutine(SpawnRoutine(delay, numberOfZombies));
    }

    private void FirstSpawn()
    {
        StartCoroutine(SpawnRoutine(1.5f, 10));
    }

    private IEnumerator SpawnRoutine(float delay = 1, int zombiesToSpawn = 1)
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            yield return new WaitForSeconds(delay);
            OnSpawn();
        }
    }

}
