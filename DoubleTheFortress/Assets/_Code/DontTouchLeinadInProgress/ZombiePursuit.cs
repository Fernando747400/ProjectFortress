using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours
{
    public RouteManagger.Route Route;
    [Header("Pursuit Dependences")]
    private Queue<Transform> transformQueue = new Queue<Transform>();
    private Transform target;
    private float distance;

    private void Start()
    {
        transformQueue = RouteManagger.Instance.SelectRoute();
        target = transformQueue.Peek();
    }

    void Update()
    {
        distance = Vector3.Distance(this.transform.position, target.transform.position);
        Debug.Log(distance);
        if (distance < 1)
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
}   
