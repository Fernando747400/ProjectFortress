using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Two_HandsHolder : XRGrabInteractable
{
    public Action OnGrabbed;
    public Action OnReleased;

    private bool _isGrabbing;
    
    
    public bool IsGrabbing => _isGrabbing;

    protected override void Awake()
    {
        base.Awake();
        onSelectEntered.AddListener(Grab);
        onSelectExited.AddListener(Drop);
        
    }

    
    protected virtual void Grab(XRBaseInteractor interactor)
    {
        _isGrabbing = true;
        OnGrabbed?.Invoke();
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {
        _isGrabbing = false;
        OnReleased?.Invoke();

    }


    private void OnTriggerStay(Collider other)
    {
        
    }
}
