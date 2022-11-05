using System;
using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class VrGunFire : DebugGunFire
{
    [SerializeField] InventoryController inventoryController;
    public InputActionReference GunShoot;
    private PlayerSelectedItem selectedItem;

    private bool _isPaused;
    void Start()
    {
        GunShoot.action.performed += ctx => FireHitScan();
    }

    protected override void Update()
    {
        //overriden do not use
        Debug.Log("canFire is "+canFire);
    }

    protected override void CheckInput()
    {
        //overriden do not use  
    }

    protected override void FireSimulated()
    {
        if (_isPaused) return;
        if (!canFire) return;
        if (inventoryController.SelectedItem != PlayerSelectedItem.Musket) return;
        base.FireSimulated();
    }

    protected override void FireHitScan()
    {
        if (_isPaused) return;
        if (inventoryController.SelectedItem != PlayerSelectedItem.Musket) return;
        base.FireHitScan();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent += Paused;
            GameManager.Instance.PlayGameEvent += Unpaused;

        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent -= Paused;
            GameManager.Instance.PlayGameEvent -= Unpaused;
            
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
}
