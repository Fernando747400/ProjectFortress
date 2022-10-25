using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class VrGunFire : DebugGunFire
{
    public InputActionReference GunShoot;
    private PlayerSelectedItem selectedItem;
    
    
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
}
