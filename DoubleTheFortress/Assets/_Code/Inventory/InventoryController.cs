using System;
using System.Collections.Generic;
using System.Linq;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryController : MonoBehaviour
{
    #region Variables
    [Header("GameObjects")]
    [SerializeField] private PlayerSelectedItem _playerSelectedItem;
    [SerializeField] private PlayerSelectedItem _playerHandsObjects;
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private GameObject[] _objectsRightHand;
    [SerializeField] private GameObject[] _objectsLeftHand;
    

    [Header("Mesh Renderers")]
    [SerializeField] private MeshRenderer hammerBlackMesh;
    [SerializeField] private MeshRenderer hammerWoodMesh;
    [SerializeField] private MeshRenderer gunMesh;
    [SerializeField] private MeshRenderer torchMesh;
    // [SerializeField] private MeshRenderer[] _meshes;
    
    [Header("Materials")]
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private Material hammerBlackMaterial;
    [SerializeField] private Material hammerWoodMaterial;
    [SerializeField] private Material gunMaterial;
    [SerializeField] private Material torchMaterial;
    
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference ConfirmSelectRightReference;
    [SerializeField] private InputActionReference ConfirmSelectLeftReference;
    [SerializeField] private InputActionReference DeselectReference;
    [SerializeField] private InputActionReference SelectRightReference;
    [SerializeField] private InputActionReference SelectLeftReference;
    // public InputActionReference WeaponReference;

    [Header("Inventory")]
    [SerializeField] float _maxTimeToSelect = 1.5f;
    private bool hasObjectSelected = false;
    public List<BoxAreasInteraction> areasInteraction;
    
    private int _currentSelected;
    private int _selectIndex;
    private bool _isInBoxInteraction;
   
    private float _time;
    private float _initialTimer = 0;
    private bool _timerIsActive;
    private bool _timerHasFinished;

    private Hand _currentSelectingHand = Hand.None;

    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    public Action<bool> OnIsSelecting;

    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
    }
    
    public PlayerSelectedItem HandsObjects
    {
        get => _playerHandsObjects;
    }
    #endregion
    
    void Start()
    {
        
        DeselectReference.action.performed += ctx => DeselectItems();
        SelectRightReference.action.performed += ctx => SelectItem(false);
        SelectLeftReference.action.performed += ctx => SelectItem(true);
        ConfirmSelectLeftReference.action.performed += ctx => ConfirmSelection(Hand.LeftHand);
        ConfirmSelectRightReference.action.performed += ctx => ConfirmSelection(Hand.RightHand);

        foreach (BoxAreasInteraction box in areasInteraction)
        {
            box.InventoryPlayer = this;
            box.OnHandEnterActionZone += HandleBoxInteraction;
        }
        OnPlayerSelectItem += HandleSelectedItem;
        DeselectItems();
    }

    private void Update()
    {
        if (SelectedItem == PlayerSelectedItem.Selecting)
        {
            HandleTimer();
            
        }
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
        _currentSelectingHand = Hand.None;
        _playerHandsObjects = PlayerSelectedItem.None;
    }

    void SelectItem(bool isLeft)
    {
        if (_isInBoxInteraction) return;

        ResetTimer();
        //Deselect current objects in hand
        DeselectItems();

        List<GameObject> objects = new List<GameObject>();
        
        if (isLeft)
        {
            objects = _objectsLeftHand.ToList();
            _currentSelectingHand = Hand.LeftHand;
        }
        else
        {
            objects = _objectsRightHand.ToList();
            _currentSelectingHand = Hand.RightHand;

        }
        
        if (_selectIndex >= objects.Count) _selectIndex = 0;
        
        _currentSelected = _selectIndex;
        objects[_currentSelected].SetActive(true);
        MaterialObjectSelecting(_currentSelected);
        _selectIndex++;
        OnIsSelecting?.Invoke(true);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);
        StartTimer();
        
    }
    
    void ConfirmSelection(Hand hand)
    {
        if (_playerSelectedItem == PlayerSelectedItem.None) return;
        if (_currentSelectingHand != hand) return;
        
            
        OnIsSelecting?.Invoke(false);
        ResetTimer();
        HandleSelectedItem(_currentSelected);
        hasObjectSelected = true;
    }
    
    void HandleBoxInteraction(bool interaction)
    {
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
                torchMesh.material = shadowMaterial;

                break;
            // case 1:
            //     torchMesh.material = shadowMaterial;
            //     break;
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
                _playerHandsObjects = PlayerSelectedItem.ObjectsRightHand;
                

                break;
            //Gun selected
            case 1 :
                gunMesh.material = gunMaterial;
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Musket);
                _playerHandsObjects = PlayerSelectedItem.ObjectsRightHand;

                break;
            //Torch
            case 2:
                torchMesh.material = torchMaterial;
                OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Torch);
                _playerHandsObjects = PlayerSelectedItem.ObjectsLeftHand;
                break;
        }

        _selectIndex = _objects.Length;
        
    }


    #region Timer Inventory
    void HandleTimer()
    {
        if (_isInBoxInteraction) return;
        if (!_timerIsActive && _timerHasFinished) return;
            
        _time += Time.deltaTime;
        if (_time > _maxTimeToSelect)
        {
            ResetTimer();
            DeselectItems();
        }
        
    }
    void StartTimer()
    {
        if (_playerSelectedItem != PlayerSelectedItem.Selecting) return;
        _time = _initialTimer;
        _timerIsActive = true;
        _timerHasFinished = false;
    }
    void ResetTimer()
    {
        Debug.Log("RESET TIMER");
        _time = 0;
        _timerIsActive = false;
        _timerHasFinished = true;

    }
    #endregion

}
    
    

