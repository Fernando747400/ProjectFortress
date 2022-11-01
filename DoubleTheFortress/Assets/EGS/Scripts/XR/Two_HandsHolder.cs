using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Two_HandsHolder : XRGrabInteractable
{
    private HandController_XR _hand;

    public Action OnGrabbed;
    public Action OnReleased;
    public Action OnHandsOut;

    private bool _isGrabbing = false;

    public HandController_XR Hand
    {
        get => _hand;
    }

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
        _hand.HandleHandsVisible(!_isGrabbing);
        OnGrabbed?.Invoke();
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {
        _isGrabbing = false;
        if(_hand!= null) _hand.HandleHandsVisible(!_isGrabbing);
        OnReleased?.Invoke();

    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // Debug.Log("Hand is in the handle");
            // _isGrabbing = true;
            _hand = other.GetComponent<HandController_XR>();
            OnHandsOut?.Invoke();

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // Debug.Log("Hand is out the handle");
            _isGrabbing = false;
            _hand.HandleHandsVisible(!_isGrabbing);
            _hand = null;
            OnHandsOut?.Invoke();
            print("send event onexit");

        }
    }
    
    
}
