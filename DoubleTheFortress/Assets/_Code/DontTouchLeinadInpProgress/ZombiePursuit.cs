using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours
{
    [Header("Pursuit Dependences")]
    public GameObject target;
    public GameObject wall;
    private Queue<GameObject> transformQueue = new Queue<GameObject>();

    void Update()
    {
        Vector3 pursuit = this.Pursuit(target.transform.position);
        transform.position += pursuit * Time.deltaTime;
    }
}   
