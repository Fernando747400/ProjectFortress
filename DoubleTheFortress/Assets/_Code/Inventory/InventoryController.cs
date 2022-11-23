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
    [SerializeField] private Hand _currentSelectingHand = Hand.None;

    [SerializeField] private GameObject[] _objectsRightHand;
    [SerializeField] private GameObject[] _objectsLeftHand;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference rightSelectHammer;
    [SerializeField] private InputActionReference rightSelectMusket;
    [SerializeField] private InputActionReference leftSelectHammer;
    [SerializeField] private InputActionReference leftSelectMusket;

    [Header("Inventory")]
    private bool hasObjectSelected = false;
    
    private int _currentSelected;
    private int _selectIndex;
    private bool _isInBoxInteraction;
   
    private float _time;
    private float _initialTimer = 0;
    private bool _timerIsActive;
    private bool _timerHasFinished;


    private Action<PlayerSelectedItem> OnPlayerSelectItem;
    public Action<bool> OnIsSelecting;

    private bool _isPaused;
    public PlayerSelectedItem SelectedItem
    {
        get => _playerSelectedItem;
        protected set => _playerSelectedItem = value;
    }
    
    public Hand SelectingHand
    {
        get => _currentSelectingHand;
    }
    #endregion
    
    void Start()
    {
        
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
        if (GameManager.Instance != null)
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
    
    public void SelectObject(PlayerSelectedItem item, Hand selectingHand)
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
        OnIsSelecting?.Invoke(true);
    }

    private void DeselectItems()
    {
        TurnOffObjects();
        SelectedItem = PlayerSelectedItem.None;
        _currentSelectingHand = Hand.None;
        OnIsSelecting?.Invoke(false);

    }

    public void DeselectItem(PlayerSelectedItem itemToDeselect, Hand hand)
    {
        int itemIndex = (int)itemToDeselect;
        switch (hand)
        {
            case Hand.LeftHand:
                if (_objectsLeftHand[itemIndex].activeInHierarchy)
                {
                    _objectsLeftHand[itemIndex].SetActive(false);
                }
                break;
            case Hand.RightHand:
                if (_objectsRightHand[itemIndex].activeInHierarchy)
                {
                    _objectsRightHand[itemIndex].SetActive(false);
                }
                break;
        }
        
        SelectedItem = PlayerSelectedItem.None;
        _currentSelectingHand = Hand.None;
        OnIsSelecting?.Invoke(false);
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
            if (_objectsRightHand[i].gameObject == null) continue;
            _objectsRightHand[i].SetActive(false);
        }

        for (int i = 0; i < _objectsLeftHand.Length; i++)
        {
            if (_objectsLeftHand[i].gameObject == null) continue;
            _objectsLeftHand[i].SetActive(false);
        }
    }

}
    
    

