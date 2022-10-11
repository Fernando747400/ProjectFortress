using System;
using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


public class InventoryController : MonoBehaviour
{
 

    public static InventoryController Instance;

    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject musketGunHand;
    [SerializeField] private PlayerSelectedItem _playerSelectedItem;
        
    
    public bool hasObjectSelected = false;
    
    public InputActionReference hammerReference;
    public InputActionReference hammerDiselect;
    public InputActionReference gunReference;

    
    private Action<PlayerSelectedItem> OnPlayerSelectItem;

    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
    }
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        hammerReference.action.performed += ctx => SelectHammer();
        hammerDiselect.action.performed += ctx => DeselectHammer();
        gunReference.action.performed += ctx => SelectWeapon();
        OnPlayerSelectItem += HandleSelectedItem;
        InitializeHandObjects();
    }

    void HandleSelectedItem(PlayerSelectedItem item)
    {
        _playerSelectedItem = item;
        // print(_playerSelectedItem);

    }
   
    void InitializeHandObjects()
    {
        hammerHand.SetActive(false);
        musketGunHand.SetActive(false);
        hasObjectSelected = false;
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);
    }
    void SelectHammer()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
        }
        else
        {
            hammerHand.SetActive(true);
            hasObjectSelected = true;
            OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Hammer);
        }
       
    }
    
    void DeselectHammer()
    {
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);
        hammerHand.SetActive(false);
        hasObjectSelected = false;
    }
    void SelectWeapon()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
        }
        else
        {
            musketGunHand.SetActive(true);
            hasObjectSelected = true;
            OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);


        }
    }
}
    
    

