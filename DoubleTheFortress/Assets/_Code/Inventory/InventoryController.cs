using System;
using System.Collections.Generic;
using System.Linq;
using DebugStuff.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryController : MonoBehaviour
{
    #region Variables
    [Header("GameObjects")]
    [SerializeField] private PlayerSelectedItem _playerSelectedItem;
    [SerializeField] private PlayerSelectedItem _playerHandsObjects;
    [SerializeField] private GameObject[] _objectsRightHand;
    [SerializeField] private GameObject[] _objectsLeftHand;
    
    [Header("Materials")]
    [SerializeField] private Material shadowMaterial;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference ConfirmSelectRightReference;
    [SerializeField] private InputActionReference ConfirmSelectLeftReference;
    [SerializeField] private InputActionReference DeselectLeftReference;
    [SerializeField] private InputActionReference DeselectRightReference;
    [SerializeField] private InputActionReference SelectRightReference;
    [SerializeField] private InputActionReference SelectLeftReference;

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
    private List<GameObject> _currentSelectedObjects;

    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    public Action<bool> OnIsSelecting;

    private bool _isPaused;
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
        
        _currentSelectedObjects = new List<GameObject>();
        
        DeselectRightReference.action.performed += ctx => DeselectItems(_currentSelectedObjects, Hand.RightHand);
        DeselectLeftReference.action.performed += ctx => DeselectItems(_currentSelectedObjects, Hand.LeftHand);
        SelectRightReference.action.performed += ctx => SelectItem(false);
        SelectLeftReference.action.performed += ctx => SelectItem(true);
        ConfirmSelectLeftReference.action.performed += ctx => ConfirmSelection(Hand.LeftHand);
        ConfirmSelectRightReference.action.performed += ctx => ConfirmSelection(Hand.RightHand);
        
        // HandleAreasInteraction();
        OnPlayerSelectItem += HandleSelectedItem;
        
        foreach (var obj in _objectsLeftHand)
        {
            _currentSelectedObjects.Add(obj);
        }

        foreach (var obj in _objectsRightHand)
        {
            _currentSelectedObjects.Add(obj);
        }
     
        DeselectItems(_currentSelectedObjects, Hand.None);
    }

    
    
    private void Update()
    {
        if (SelectedItem == PlayerSelectedItem.Selecting)
        {
            HandleTimer();
        }
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

    private void Paused()
    {
        _isPaused = true;
    }
    void Unpaused()
    {
        _isPaused = false;
    }
    public void HandleAreasInteraction(BoxAreasInteraction interaction = null)
    {
        areasInteraction.Add(interaction);

        if (areasInteraction.Count > 0 )
        {
            foreach (BoxAreasInteraction box in areasInteraction)
            {
                box.InventoryPlayer = this;
                box.OnHandEnterActionZone += HandleBoxInteraction;
            }
        }
        
        
    }

    void HandleSelectedItem(PlayerSelectedItem item)
    {
        _playerSelectedItem = item;
    }
    
    void DeselectItems(List<GameObject> objects, Hand hand)
    {
        if (_isPaused)
        {
            return;
        }
        if (hand != _currentSelectingHand)
        {
            return;
        }
        
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        _currentSelectedObjects.Clear();
        hasObjectSelected = false;
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.None);
        _currentSelectingHand = Hand.None;
        _playerHandsObjects = PlayerSelectedItem.None;
        
    }



    void SelectItem(bool isLeft)
    {
        if (_isPaused)return;
        if (_isInBoxInteraction) return;
        if(hasObjectSelected) return;

        ResetTimer();
        //Deselect current objects in hand
        DeselectItems(_currentSelectedObjects , _currentSelectingHand);

        List<GameObject> objects = new List<GameObject>();
        
        if (isLeft)
        {
            objects = _objectsLeftHand.ToList();
            _currentSelectedObjects = objects;
            _currentSelectingHand = Hand.LeftHand;
            
        }
        else
        {
            objects = _objectsRightHand.ToList();
            _currentSelectedObjects = objects;
            _currentSelectingHand = Hand.RightHand;

        }
        
        if (_selectIndex >= objects.Count) _selectIndex = 0;
        
        _currentSelected = _selectIndex;
        objects[_currentSelected].SetActive(true);
        MaterialObjectSelecting(_currentSelected, objects);
        _selectIndex++;
        OnIsSelecting?.Invoke(true);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);
        StartTimer();
        
    }
    
    void ConfirmSelection(Hand hand)
    {
        if (_isInBoxInteraction) return;
        if (_playerSelectedItem == PlayerSelectedItem.None) return;
        if (_currentSelectingHand != hand) return;
        
            
        OnIsSelecting?.Invoke(false);
        ResetTimer();
        HandleSelectedItem(_currentSelected, _currentSelectedObjects);
        hasObjectSelected = true;
    }
    
    void HandleBoxInteraction(bool interaction)
    {
        _isInBoxInteraction = !interaction;
        if (_playerSelectedItem == PlayerSelectedItem.Selecting)
        {
            DeselectItems(_currentSelectedObjects, _currentSelectingHand);
        }
    }

    void MaterialObjectSelecting(int item, List<GameObject> objects)
    {
        objects[item].GetComponent<IGrabbable>().SetMaterials(shadowMaterial);
    }

    void HandleSelectedItem(int itemSelected, List<GameObject> objects)
    {
        IGrabbable item = objects[itemSelected].GetComponent<IGrabbable>();
        item.ResetMaterials();
        OnPlayerSelectItem?.Invoke(item.Item);
        _playerHandsObjects = item.TypeOfItem;
        _selectIndex = _currentSelectedObjects.Count;

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
            DeselectItems(_currentSelectedObjects, _currentSelectingHand);
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
        _time = 0;
        _timerIsActive = false;
        _timerHasFinished = true;

    }
    #endregion

}
    
    

