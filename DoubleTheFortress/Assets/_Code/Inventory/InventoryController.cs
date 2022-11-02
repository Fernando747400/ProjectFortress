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
    [SerializeField] private GameObject musketGunHand;
    [SerializeField] private PlayerSelectedItem _playerSelectedItem;
    [SerializeField] private GameObject[] _objects;
    

    [Header("Mesh Renderers")]
    [SerializeField] private MeshRenderer hammerBlackMesh;
    [SerializeField] private MeshRenderer hammerWoodMesh;
    [SerializeField] private MeshRenderer gunMesh;
    [SerializeField] private MeshRenderer[] _meshes;
    [Header("Materials")]
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private Material hammerBlackMaterial;
    [SerializeField] private Material hammerWoodMaterial;
    [SerializeField] private Material gunMaterial;
    
    public bool hasObjectSelected = false;
    public InputActionReference ConfirmSelectReference;
    public InputActionReference DeselectReference;
    public InputActionReference SelectReference;

    public List<BoxAreasInteraction> areasInteraction;

    private int selectIndex;
    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    private bool _isInBoxInteraction;

    private int _currentSelected;

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
        ConfirmSelectReference.action.performed += ctx => ConfirmSelection();

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
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(false);
        }
        hasObjectSelected = false;
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);
    }

    void SelectItem()
    {
        if (_isInBoxInteraction) return;
        int countIndex = selectIndex;
        
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
        
        _currentSelected = selectIndex;
        _objects[_currentSelected].SetActive(true);
        MaterialObjectSelecting(_currentSelected);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);
    }
    

    void ConfirmSelection()
    {
        ResetMaterialObject(_currentSelected);
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

    void MaterialObjectSelecting(int item)
    {
        if (item == 0)
        {
            hammerBlackMesh.material = shadowMaterial;
            hammerWoodMesh.material = shadowMaterial;
        }
        else
        {
            _meshes[item].material = shadowMaterial;
        }
    }

    void ResetMaterialObject(int itemSelected)
    {
        switch (itemSelected)
        {
            //hammer selected
            case 0:
                hammerBlackMesh.material = hammerBlackMaterial;
                hammerWoodMesh.material = hammerWoodMaterial;
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Hammer);

                
                break;
            //Gun selected
            case 1 :
                gunMesh.material = gunMaterial;
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);

                break;
        }
    }
}
    
    

