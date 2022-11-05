using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_CannonFire : MonoBehaviour
{
    [Tooltip("Object Fired by the cannon")] 
    [SerializeField] private GameObject projectile;
    [SerializeField] private int maxProjectiles;
    [SerializeField] private float launchForce = 800f;
    
    private Queue<Canon_CanonBall> _canonBalls;
    private int _current;

    // Start is called before the first frame update
    void Start()
    {
        BuildPool();
    }

    // Update is called once per frame
    void Update()
    { 
        CheckInput();   
    }

    private void CheckInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Launch();
    }

    public void Launch()
    {
        try
        {
            Canon_CanonBall current = _canonBalls.Dequeue();
            current.Fire(transform.position,transform.forward,launchForce);
            // Debug.Log("Launched" + current.name);
        }
        catch { Debug.Log("Queue Is empty"); }
    }

    public void Reload(Canon_CanonBall canonBall)
    {
        try
        {
            _canonBalls.Enqueue(canonBall);
        }
        catch { Debug.Log("Error Queuing");}
    }
    private void BuildPool()
    {
        _canonBalls = new Queue<Canon_CanonBall>();
        for (int i = 0; i < maxProjectiles; i++)
        {
            GameObject temp = Instantiate(projectile);
            temp.name = "CanonBall " + i;
            temp.GetComponent<Canon_CanonBall>().cannonFire = this;
            _canonBalls.Enqueue(temp.GetComponent<Canon_CanonBall>());
        }    
    }
}
