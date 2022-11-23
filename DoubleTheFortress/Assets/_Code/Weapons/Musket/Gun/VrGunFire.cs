using System;
using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class VrGunFire : DebugGunFire
{
    [SerializeField] InventoryController inventoryController;
    public InputActionReference gunShoot;

    private bool _isPaused;
    void Start()
    {
        gunShoot.action.performed += ctx => FireHitScan();
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
        
        StartCoroutine(CorWaitForCoolDown());
       
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
            _isPaused = GameManager.Instance.IsPaused;
        }
    }

    protected override IEnumerator CorWaitForCoolDown()
    {
        Debug.Log("waiting " + cooldown);
        yield return new WaitForSeconds(cooldown);
        canFire = true;
        Debug.Log("done waiting");
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
}
