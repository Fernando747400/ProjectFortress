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

    // Start is called before the first frame update
    void Start()
    {
        GunShoot.action.performed += ctx => FireHitScan();
    }

    protected override void Update()
    {
        //overriden do not use
    }

    protected override void CheckInput()
    {
        //overriden do not use  
    }

    protected override void FireSimulated()
    {
        if (_isPaused) return;
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
        GameManager.Instance.PauseGameEvent += Paused;
        GameManager.Instance.PlayGameEvent += Unpaused;
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseGameEvent -= Paused;
        GameManager.Instance.PlayGameEvent -= Unpaused;
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
