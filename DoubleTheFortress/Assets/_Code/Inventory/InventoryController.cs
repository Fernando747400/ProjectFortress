using System;
using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryController : MonoBehaviour
{
    #region Variables
    [Header("GameObjects")]
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
    
    
    [Header("Input Actions")]
    public InputActionReference ConfirmSelectReference;
    public InputActionReference DeselectReference;
    public InputActionReference SelectReference;
    // public InputActionReference WeaponReference;

    [Header("Inventory")]
    public float _maxTimeToSelect = 1.5f;
    public bool hasObjectSelected = false;
    public List<BoxAreasInteraction> areasInteraction;

    private int _currentSelected;
    private int _selectIndex;
    private bool _isInBoxInteraction;
   
    private float _time;
    private float _initialTimer;
    private bool _timerHasStarted;
    private bool _timerHasFinished;
    
    private Action<PlayerSelectedItem> OnPlayerSelectItem;

    
    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
    }
    #endregion
    
    void Start()
    {
        
        DeselectReference.action.performed += ctx => DeselectItems();
        SelectReference.action.performed += ctx => SelectItem();
        ConfirmSelectReference.action.performed += ctx => ConfirmSelection();
        // WeaponReference.action.performed += ctx => SelectWeapon();

        foreach (BoxAreasInteraction box in areasInteraction)
        {
            box.OnHandEnterActionZone += HandleBoxInteraction;
        }
        OnPlayerSelectItem += HandleSelectedItem;
        DeselectItems();
    }

    private void Update()
    {
        HandleTimer();
    }

    void HandleSelectedItem(PlayerSelectedItem item)
    {
        _playerSelectedItem = item;
    }
    
    void DeselectItems()
    {
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

        ResetTimer();
        //Deselect current objects in hand
        DeselectItems();
        
        if (_selectIndex >= _objects.Length) _selectIndex = 0;

        Debug.Log(_selectIndex);
        _currentSelected = _selectIndex;
        _objects[_currentSelected].SetActive(true);
        MaterialObjectSelecting(_currentSelected);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);

        _selectIndex++;

        StartTimer();
        
    }
    
    void ConfirmSelection()
    {
        HandleSelectedItem(_currentSelected);
        hasObjectSelected = true;
    }
    
    void HandleBoxInteraction(bool interaction)
    {
        if (hasObjectSelected) DeselectItems();
        _isInBoxInteraction = !interaction;
    }

    void MaterialObjectSelecting(int item)
    {
        switch (item)
        {
            case 0 :
                hammerBlackMesh.material = shadowMaterial;
                hammerWoodMesh.material = shadowMaterial;
                break;
            
            case 1 :
                gunMesh.material = shadowMaterial;
                break;
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
                
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Hammer);

                break;
            //Gun selected
            case 1 :
                gunMesh.material = gunMaterial;
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);

                break;
        }

        _selectIndex = _objects.Length;
        
    }


    void HandleTimer()
    {
        if (_playerSelectedItem != PlayerSelectedItem.Selecting) return;
        if (!_timerHasStarted && _timerHasFinished) return;
            
        _time += Time.deltaTime;

        if (_time > _maxTimeToSelect)
        {
            ResetTimer();
            DeselectItems();
        }
        
    }
    void StartTimer()
    {
        _time = _initialTimer;
        _timerHasStarted = true;
        _timerHasFinished = false;
    }
    void ResetTimer()
    {
        if (_playerSelectedItem != PlayerSelectedItem.Selecting) return;
        _time = 0;
        _timerHasFinished = true;
    }
}
    
    

