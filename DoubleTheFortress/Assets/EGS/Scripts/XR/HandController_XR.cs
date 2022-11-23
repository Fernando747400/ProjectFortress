using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;

public class HandController_XR : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private SkinnedMeshRenderer handSkinnedMesh;
    [SerializeField] private Hand _myHand;

    [SerializeField] private bool _handIsEmpty;
    [SerializeField] private bool _isUsingCannon;
    
    public Hand MyHand
    {
        get => _myHand;
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
        switch (MyHand)
        {
            case Hand.LeftHand:
                if (_inventoryController.SelectingHand == _myHand )
                {
                    _handIsEmpty = false;
                }
                break;
            case Hand.RightHand:
                if (_inventoryController.SelectingHand == _myHand)
                {
                    _handIsEmpty = false;
                }
                break;
        }
        
    }


    public void HandleTorchCannon(bool isUsing)
    {
        _isUsingCannon = isUsing;
        
        switch (MyHand)
        {
            case Hand.LeftHand:
                if (_isUsingCannon)
                {
                    _inventoryController.SelectObject(PlayerSelectedItem.Torch, Hand.RightHand);
                }
                else
                {
                    _inventoryController.DeselectItem(PlayerSelectedItem.Torch, Hand.RightHand);
                }
                break;
            
            case Hand.RightHand:
                if (_isUsingCannon)
                {
                    _inventoryController.SelectObject(PlayerSelectedItem.Torch, Hand.LeftHand);
                }
                else
                {
                    _inventoryController.DeselectItem(PlayerSelectedItem.Torch, Hand.LeftHand);

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