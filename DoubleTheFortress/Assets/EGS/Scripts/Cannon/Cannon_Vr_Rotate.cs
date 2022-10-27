using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_Vr_Rotate : Gun_Rotate
{


    [SerializeField] private GameObject handle;
    private Vector3 _handlePos;
    
    protected override void Update()
    {
        LookAtRotate();

    }

    private void LookAtRotate()
    { 
        _handlePos = handle.transform.position;
        transform.LookAt(-_handlePos);

    }

    
    

}
