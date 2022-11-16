using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Two_HandsHolder : XRBaseInteractable
{
    public HandController_XR _currentHand;

    public List<HandController_XR> _hands;
    public Action OnGrabbed;
    public Action OnReleased;
    public Action OnHandsOut;

    private bool _isGrabbing = false;

    public HandController_XR Hand
    {
        get => _currentHand;
    }

    public bool IsGrabbing => _isGrabbing;

    protected override void Awake()
    {
        base.Awake();


        _hands = new List<HandController_XR>();
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
        _currentHand.HandleIsEmpty();
        if (!_currentHand.HandIsEmpty) return;
        
        _isGrabbing = true;
        _currentHand.HandleHandsVisible(false);
        _currentHand.HandleIsUsingCannon(true);
        OnGrabbed?.Invoke();
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {
        _isGrabbing = false;
        
        _currentHand.HandleHandsVisible(true);
        _currentHand.HandleIsUsingCannon(false);
        // foreach (var hand in _hands)
        // {
        //     hand.HandleHandsVisible(true);
        //     hand.HandleIsUsingCannon(false);
        // }
        OnReleased?.Invoke();

    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            _currentHand = other.GetComponent<HandController_XR>();
            _currentHand.HandleIsEmpty();
            // if (!_hands.Contains(_currentHand))
            // {
            //     _hands.Add(_currentHand);
            // }
            OnHandsOut?.Invoke();

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            _isGrabbing = false;

            // foreach (var hand in _hands)
            // {
            //     hand.HandleHandsVisible(true);
            // }
            //
            // _hands.Clear();
            
            _currentHand.HandleHandsVisible(true);
            _currentHand.HandleIsUsingCannon(false);

            _currentHand = null;
            OnHandsOut?.Invoke();

        }
    }
    
    
}

