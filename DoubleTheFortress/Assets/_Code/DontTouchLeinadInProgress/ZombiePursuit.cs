
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours
{
    [Header("Pursuit Dependences")]
    private Queue<Transform> transformQueue = new Queue<Transform>();
    private Transform target;
    private float distance;
    public float maxDistance;

    private void Start()
    {
        this.speed = Random.Range(.5f, 4);
        maxDistance = EnemyManagger.Instance.maxDistance;
    }

    void Update()
    {
        try 
        {
            Pursuit();
        }
        catch
        {
            GetRoute();
        }
        
    }

    void Pursuit()
    {
        distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance < maxDistance)
        {
            UpdateTarget();
        }
        Vector3 pursuit = this.Pursuit(target.transform.position);
        transform.position += pursuit * Time.deltaTime;
    }

    private void UpdateTarget()
    {
        target = transformQueue.Dequeue();
        target = transformQueue.Peek();
    }

    void GetRoute()
    {
        transformQueue = RouteManagger.Instance.RandomRoute();
        target = transformQueue.Peek();
        target = transformQueue.Peek();
    }

    void Die()
    {
        transformQueue.Clear();
        target = null;
        EnemyManagger.Instance.Despawn(EnemyManagger.Instance.Zombie, this.gameObject);
        EnemyManagger.Instance.OnSpawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Final"))
        {
            Die();
        }
    }
}   
