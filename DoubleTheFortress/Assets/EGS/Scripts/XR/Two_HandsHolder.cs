using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Two_HandsHolder : XRGrabInteractable
{
    private Action OnGrabbed;
    private Action OnReleased;

    private bool _isGrabbing;
    
    public bool IsGrabbing => _isGrabbing;

    protected override void Awake()
    {
        base.Awake();
        onSelectEntered.AddListener(Grab);
        
    }

    
    protected virtual void Grab(XRBaseInteractor interactor)
    {
        print("se grabea");
        OnGrabbed?.Invoke();
        _isGrabbing = true;
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {
        OnReleased?.Invoke();
        _isGrabbing = false;
    }
    
}
