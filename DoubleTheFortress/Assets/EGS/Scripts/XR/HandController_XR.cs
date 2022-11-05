using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;

public class HandController_XR : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private SkinnedMeshRenderer handSkinnedMesh;
    [SerializeField] private Hand _hand;
    [SerializeField] PlayerSelectedItem handObjects;
    private Vector3 _handPos;

    public bool _handIsEmpty;
    public Hand Hand
    {
        get => _hand;
    }
    public Vector3 HandPos
    {
        get { return transform.position; }
    }

    public bool HandIsEmpty
    {
        get => _handIsEmpty;
    }

    void Start()
    {
        _inventoryController.OnIsSelecting += HandleIsEmpty;
        
    }
    
    public void HandleHandsVisible(bool visible)
    {
        handSkinnedMesh.enabled = visible;
    }

    public void HandleIsEmpty(bool isSelecting = false)
    {
        _handIsEmpty = true;
        switch (Hand)
        {
            case Hand.LeftHand:
                
                if (_inventoryController.HandsObjects == handObjects)
                {
                    _handIsEmpty = false;
                }
                break;
            case Hand.RightHand:
                if (_inventoryController.HandsObjects == handObjects)
                {
                    _handIsEmpty = false;
                }
                break;
        }
        
    }
    
    
    
}
public enum Hand
{
    None,
    RightHand,
    LeftHand,
}