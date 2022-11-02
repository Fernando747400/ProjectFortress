using System;
using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject hammerBlackPart;
    [SerializeField] private GameObject hammerWoodPart;
    [SerializeField] private GameObject musketGunHand;
    [SerializeField] private PlayerSelectedItem _playerSelectedItem;

    [SerializeField] private GameObject[] _objects;
    

    [Header("Materials")]
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private Material hammerBlackMaterial;
    [SerializeField] private Material hammerWoodMaterial;
    [SerializeField] private Material gunMaterial;
    
    public bool hasObjectSelected = false;
    public InputActionReference hammerReference;
    public InputActionReference DeselectReference;
    public InputActionReference SelectReference;
    public InputActionReference hammerDiselect;
    public InputActionReference gunReference;

    public List<BoxAreasInteraction> areasInteraction;

    private int selectIndex;
    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    private bool _isInBoxInteraction;

    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
    }
    private void Awake()
    {
    }

    void Start()
    {
        
        // hammerReference.action.performed += ctx => SelectHammer();
        // hammerDiselect.action.performed += ctx => DeselectItems();
        // gunReference.action.performed += ctx => SelectWeapon();


        DeselectReference.action.performed += ctx => DeselectItems();
        SelectReference.action.performed += ctx => SelectItem();
        
        
        foreach (BoxAreasInteraction box in areasInteraction)
        {
            box.OnHandEnterActionZone += HandleBoxInteraction;
        }
        OnPlayerSelectItem += HandleSelectedItem;
        DeselectHandObjects();
    }

    void HandleSelectedItem(PlayerSelectedItem item)
    {
        _playerSelectedItem = item;
    }
   
   public void DeselectHandObjects()
    {
        hammerHand.SetActive(false);
        musketGunHand.SetActive(false);
        hasObjectSelected = false;
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);
    }
    int SelectHammer()
    {
        if (hasObjectSelected)
        {
            DeselectHandObjects();
        }
        else
        {
            hammerHand.SetActive(true);
            hasObjectSelected = true;
            OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Hammer);
        }

        return 0;
    }
    
    void DeselectItems()
    {
        if (_isInBoxInteraction) return;

        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);

        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(false);
        }
        hasObjectSelected = false;
    }

    void SelectItem()
    {
        if (_isInBoxInteraction) return;
        int countIndex = selectIndex;
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);
        
        //Deselect current objects in hand
        DeselectItems();

        if (countIndex <= _objects.Length)
        {
            selectIndex++;
        }
        else
        {
            selectIndex = 0;
        }
        
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[countIndex].SetActive(true);
        }   
        
    }
    

    void ConfirmSelection()
    {
        
    }

    int  SelectWeapon()
    {
        if (hasObjectSelected)
        {
            DeselectHandObjects();
        }
        else
        {
            musketGunHand.SetActive(true);
            hasObjectSelected = true;
            OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);
        }
        
        return 2;
    }

    void HandleBoxInteraction(bool interaction)
    {
        if (hasObjectSelected) DeselectHandObjects();
        _isInBoxInteraction = !interaction;
    }

    void MaterialObjectSelecting(GameObject item)
    {
        MeshRenderer meshRenderer = item.GetComponent<MeshRenderer>();
        meshRenderer.material = shadowMaterial;
    }

    void ResetMaterialObject()
    {
        
    }
}
    
    

