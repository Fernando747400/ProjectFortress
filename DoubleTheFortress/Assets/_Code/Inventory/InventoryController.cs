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
    [SerializeField] private GameObject[] _objectsRightHand;
    [SerializeField] private GameObject[] _objectsLeftHand;
    
    // [Header("Materials")]
    // [SerializeField] private Material shadowMaterial;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference rightSelectHammer;
    [SerializeField] private InputActionReference rightSelectMusket;
    [SerializeField] private InputActionReference leftSelectHammer;
    [SerializeField] private InputActionReference leftSelectMusket;

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

    public Hand _currentSelectingHand = Hand.None;

    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    public Action<bool> OnIsSelecting;

    private bool _isPaused;
    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
        protected set => _playerSelectedItem = value;
    }
    
    public PlayerSelectedItem HandsObjects
    {
        get => _playerHandsObjects;
    }
    #endregion
    
    void Start()
    {
        
        // _currentSelectedObjects = new List<GameObject>();
        
        // DeselectRightReference.action.performed += ctx => DeselectItems(_currentSelectedObjects, Hand.RightHand);
        // DeselectLeftReference.action.performed += ctx => DeselectItems(_currentSelectedObjects, Hand.LeftHand);
        // SelectRightReference.action.performed += ctx => SelectItem( Hand.RightHand);
        // SelectLeftReference.action.performed += ctx => SelectItem( Hand.LeftHand);
        // ConfirmSelectLeftReference.action.performed += ctx => ConfirmSelection(Hand.LeftHand);
        // ConfirmSelectRightReference.action.performed += ctx => ConfirmSelection(Hand.RightHand);
        
        // HandleAreasInteraction();
        // OnPlayerSelectItem += HandleConfirmItem;
        
        // foreach (var obj in _objectsLeftHand)
        // {
        //     _currentSelectedObjects.Add(obj);
        // }
        //
        // foreach (var obj in _objectsRightHand)
        // {
        //     _currentSelectedObjects.Add(obj);
        // }
     
        // DeselectItems(_currentSelectedObjects, Hand.None);


        rightSelectHammer.action.performed += ctx => SelectObject(PlayerSelectedItem.Hammer, Hand.RightHand);
        rightSelectMusket.action.performed += ctx => SelectObject(PlayerSelectedItem.Musket, Hand.RightHand);
        
        leftSelectHammer.action.performed += ctx => SelectObject(PlayerSelectedItem.Hammer, Hand.LeftHand);
        leftSelectMusket.action.performed += ctx => SelectObject(PlayerSelectedItem.Musket, Hand.LeftHand);
        
        DeselectItems();



    }

    
    

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent += Paused; 
            GameManager.Instance.PlayGameEvent += Unpaused;
        }

    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.PauseGameEvent -= Paused;
            GameManager.Instance.PlayGameEvent -= Unpaused;
        }
    }

    private void Paused()
    {
        _isPaused = true;
    }
    void Unpaused()
    {
        _isPaused = false;
    }
   /* public void HandleAreasInteraction(BoxAreasInteraction interaction = null)
    {
        if (areasInteraction.Count > 0 )
        {
            foreach (BoxAreasInteraction box in areasInteraction)
            {
                // box.InventoryPlayer = this;
                // box.OnHandEnterActionZone += HandleBoxInteraction;
            }
        }
    }
*/
    
  /*  void DeselectItems(Hand hand)
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
        
    }*/

  /*  void SelectItem(Hand _currentHand)
    {
        // if (_isPaused)return;
        if(hasObjectSelected && _currentSelectingHand == _currentHand) return;
        if (_isInBoxInteraction && _currentHand == Hand.RightHand) return;

        DeselectItems(_currentSelectedObjects , _currentSelectingHand);
       
        _currentSelectingHand = _currentHand;
        ResetTimer();
        
        List<GameObject> objects = new List<GameObject>();

        switch (_currentHand)
        {
            case Hand.LeftHand:
                objects = _objectsLeftHand.ToList();
                break;
            case Hand.RightHand:
                objects = _objectsRightHand.ToList();
                break;
        }
        _currentSelectedObjects = objects;
        
        if (_selectIndex >= objects.Count) _selectIndex = 0;
        
        _currentSelected = _selectIndex;
        objects[_currentSelected].SetActive(true);
        HandleSelectedItem(_currentSelected, objects, true);
        OnPlayerSelectItem?.Invoke(PlayerSelectedItem.Selecting);
        OnIsSelecting?.Invoke(true);
        StartTimer();
        _selectIndex++;
        
    }

*/
    void SelectObject(PlayerSelectedItem item, Hand selectingHand)
    {
       
        if (SelectedItem == item && selectingHand == _currentSelectingHand)
        {
            DeselectItems();
            return;
        }
        
        if (SelectedItem == item && selectingHand != _currentSelectingHand)
        {
            DeselectItems();
        }
        
        SelectedItem = item;
        _currentSelectingHand = selectingHand;

        GameObject[] handbOjects = new GameObject[0];
        switch (selectingHand)
        {
            case Hand.LeftHand:
                handbOjects = _objectsLeftHand;
                break;
                
            case Hand.RightHand:
                handbOjects = _objectsRightHand;
                break;
        }
        
        TurnOnObject((int)SelectedItem, handbOjects);
    }

    void DeselectItems()
    {
        TurnOffObjects();
        SelectedItem = PlayerSelectedItem.None;
    }
    
    void TurnOnObject(int itemIndex, GameObject[] handObjects)
    {
        TurnOffObjects();
        handObjects[itemIndex].SetActive(true);

    }

    void TurnOffObjects()
    {
        for (int i = 0; i < _objectsRightHand.Length; i++)
        {
            _objectsRightHand[i].SetActive(false);
        }

        for (int i = 0; i < _objectsLeftHand.Length; i++)
        {
            _objectsLeftHand[i].SetActive(false);
        }
    }
    
    
    
  /*  void ConfirmSelection(Hand hand)
    {
        if (_playerSelectedItem == PlayerSelectedItem.None) return;
        if (_currentSelectingHand != hand) return;
        
        HandleSelectedItem(_currentSelected, _currentSelectedObjects, false);
        ResetTimer();
        HandleConfirmItem(_currentSelected, _currentSelectedObjects);
        hasObjectSelected = true;
        OnIsSelecting?.Invoke(false);
    }*/
    
   /* void HandleBoxInteraction(bool interaction)
    {
        _isInBoxInteraction = interaction;
        if (_playerSelectedItem == PlayerSelectedItem.Selecting)
        {
            // DeselectItems(_currentSelectedObjects, _currentSelectingHand);
        }
    }*/

   /* void HandleSelectedItem(int item, List<GameObject> objects,bool isSelecting)
    {
        IGrabbable selected = objects[item].GetComponent<IGrabbable>();
        selected.SetMaterials(shadowMaterial);
        selected.HandleSelectedState(isSelecting);
    }*/

   /* void HandleConfirmItem(int itemSelected, List<GameObject> objects)
    {
        IGrabbable item = objects[itemSelected].GetComponent<IGrabbable>();
        item.ResetMaterials();
        _playerHandsObjects = item.TypeOfItem;
        _selectIndex = _currentSelectedObjects.Count;
        OnPlayerSelectItem?.Invoke(item.Item);
    }*/



}
    
    

