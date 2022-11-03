using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Two_HandsHolder : XRBaseInteractable
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

    private void Start()
    {
        IXRSelectInteractor newInteractor = firstInteractorSelecting;
        List<IXRSelectInteractor> moreInteractors = interactorsSelecting;

    }

    protected virtual void Grab(XRBaseInteractor interactor)
    {
        Debug.Log(_hand.HandIsEmpty + "  _hand.HandIsEmpty ");
        if (!_hand.HandIsEmpty) return;
        _isGrabbing = true;
        _hand.HandleHandsVisible(false);
        OnGrabbed?.Invoke();
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {
        _isGrabbing = false;
        _hand.HandleHandsVisible(true);
        OnReleased?.Invoke();

    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            _hand = other.GetComponent<HandController_XR>();
            _hand.HandleIsEmpty();
            OnHandsOut?.Invoke();

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (_isGrabbing)
            {
                _isGrabbing = false;
                _hand.HandleHandsVisible(!_isGrabbing);
            }
           
            _hand = null;
            OnHandsOut?.Invoke();

        }
    }
    
    
}

