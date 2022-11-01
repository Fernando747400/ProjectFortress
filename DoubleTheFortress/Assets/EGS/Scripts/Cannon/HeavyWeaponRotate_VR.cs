using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponRotate_VR : Gun_Rotate
{


    [SerializeField] private GameObject handle;
    private Vector3 _handlePos;
    private Vector3 _initialPos;
    public Vector3 InitPos
    {
        get => _initialPos;
    }

    protected override void Start()
    {
        base.Start();
        _initialPos = transform.position;
    }

    protected override void Update()
    {
        AimAtDirection();
    }

    private void AimAtDirection()
    {
        Vector3 aimDir = transform.position - handle.transform.position;
        transform.rotation = Quaternion.LookRotation(aimDir);
        // Debug.DrawRay(transform.position, -(handlePos - transform.position), Color.magenta);
        // Debug.DrawLine(transform.position, handlePos, Color.cyan);
    }

    // public void ResetPosition()
    // {
    //     transform.position = InitPos;
    // }

    
    

}
