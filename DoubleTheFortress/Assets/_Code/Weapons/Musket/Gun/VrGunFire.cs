using System;
using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class VrGunFire : DebugGunFire
{
    [SerializeField] InventoryController inventoryController;
    public InputActionReference gunShootRight;
    public InputActionReference gunShootLeft;

    [SerializeField]private Gun_Persistent persistentData;

    private bool _isPaused;
    protected override void Awake()
    {
        Prepare();
    }

    private void Start()
    {
        gunShootRight.action.performed += ctx => CallInput(Hand.RightHand);
        gunShootLeft.action.performed += ctx => CallInput(Hand.LeftHand);
    }

    protected override void CheckInput()
    {
        //overriden do not use  
    }

    void CallInput(Hand hand)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }
        if (hand != inventoryController.SelectingHand || hand == Hand.None)
        {
            Debug.Log("son  manos diferentes");
            return;
        }
        FireHitScan();
    }
    protected override void FireHitScan()
    {
        // Debug.Log(hand);
        if (_isPaused) return;
        if (inventoryController.SelectedItem != PlayerSelectedItem.Musket)
        {
            Debug.Log("no debería poder disparar");
            return;
        }
        
        if (!persistentData.CooldownPassed()) return;
       
        base.FireHitScan();
    }

    protected override void FireSimulated()
    {
        if (_isPaused) return;
        if (inventoryController.SelectedItem != PlayerSelectedItem.Musket) return;
        base.FireSimulated();
        persistentData.ResetCooldwon();
        
    }


    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent += Paused;
            GameManager.Instance.PlayGameEvent += Unpaused;
            _isPaused = GameManager.Instance.IsPaused;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent -= Paused;
            GameManager.Instance.PlayGameEvent -= Unpaused;
            _isPaused = GameManager.Instance.IsPaused;        
        }
    }

    void Paused()
    {
        _isPaused = true;
    }
    void Unpaused()
    {
        _isPaused = false;
    }

    protected override void Prepare()
    {
        base.Prepare();
        if (persistentData == null)
        {
            throw new NullReferenceException("No persistentData found for musket");
        }
    }
}
