using System;
using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class VrGunFire : DebugGunFire
{
    [SerializeField] InventoryController inventoryController;
    public InputActionReference gunShoot;

    [SerializeField]private Gun_Persistent persistentData;

    private bool _isPaused;
    protected override void Awake()
    {
        Prepare();
    }

    private void Start()
    {
        gunShoot.action.performed += ctx => FireHitScan();
    }

    protected override void CheckInput()
    {
        //overriden do not use  
    }
    protected override void FireHitScan()
    {
        if (_isPaused) return;
        if (inventoryController.SelectedItem != PlayerSelectedItem.Musket) return;
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
