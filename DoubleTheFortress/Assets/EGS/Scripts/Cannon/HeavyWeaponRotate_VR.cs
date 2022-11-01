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
        // LookAtRotate();
    }

    public void LookAtRotate(Vector3 position)
    {
        _handlePos = position;
        _handlePos = handle.transform.position;
        transform.LookAt(-_handlePos);

    }

    // public void ResetPosition()
    // {
    //     transform.position = InitPos;
    // }

    
    

}
