using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Test_Rotating : Target_Test
{
    private float _speed = .33f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(_speed, _speed, _speed));
    }
}
