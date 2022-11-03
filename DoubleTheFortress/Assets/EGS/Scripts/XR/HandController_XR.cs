using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;

public class HandController_XR : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private SkinnedMeshRenderer handSkinnedMesh;
    [SerializeField] private Hand _hand;
    private Vector3 _handPos;

    private bool _handIsEmpty;
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
        set => _handIsEmpty = value;
    }

    void Start()
    {
        _inventoryController.OnIsSelecting += HandleHandsVisible;
    }

    void Update()
    {
        
    }

    public void HandleHandsVisible(bool visible)
    {
        handSkinnedMesh.enabled = visible;
    }

    private void HandleIsEmpty(bool isSelecting)
    {
        PlayerSelectedItem selectedItem;
        switch (Hand)
        {
            case Hand.LeftHand:
                
                
                break;
            
        }
        _handIsEmpty = isSelecting;
        
    }
    
    
    
}
public enum Hand
{
    RightHand,
    LeftHand,
}