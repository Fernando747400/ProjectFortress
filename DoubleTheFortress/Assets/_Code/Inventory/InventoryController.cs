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
        
        DeselectReference.action.performed += ctx => DeselectItems();
        SelectReference.action.performed += ctx => SelectItem();
        ConfirmSelectReference.action.performed += ctx => ConfirmSelection();

        foreach (BoxAreasInteraction box in areasInteraction)
        {
            box.OnHandEnterActionZone += HandleBoxInteraction;
        }
        OnPlayerSelectItem += HandleSelectedItem;
        DeselectItems();
    }

    void HandleSelectedItem(PlayerSelectedItem item)
    {
        _playerSelectedItem = item;
    }
    
    int SelectHammer()
    {
        if (hasObjectSelected)
        {
            DeselectItems();
        }
        else
        {
            // hammerHand.SetActive(true);
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

        //Deselect current objects in hand
        if (hasObjectSelected) DeselectItems();
        
        if (selectIndex >= _objects.Length) selectIndex = 0;

        Debug.Log(selectIndex);
        _currentSelected = selectIndex;
        _objects[_currentSelected].SetActive(true);
        MaterialObjectSelecting(_currentSelected);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);

        selectIndex++;
    }
    

    void ConfirmSelection()
    {
        HandleSelectedItem(_currentSelected);
    }

    int  SelectWeapon()
    {
        musketGunHand.SetActive(true); 
        hasObjectSelected = true; 
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);
        return 2;
    }

    void HandleBoxInteraction(bool interaction)
    {
        if (hasObjectSelected) DeselectItems();
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

    void HandleSelectedItem(int itemSelected)
    {
        switch (itemSelected)
        {
            //hammer selected
            case 0:
                hammerBlackMesh.material = hammerBlackMaterial;
                hammerWoodMesh.material = hammerWoodMaterial;
                _currentSelected = SelectHammer();
                break;
            //Gun selected
            case 1 :
                gunMesh.material = gunMaterial;
                _currentSelected = SelectWeapon();
                break;
        }
    }
}
    
    

