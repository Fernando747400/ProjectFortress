using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;

public class Torch_Grab : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private GameObject _particles;
    [SerializeField] private Collider _collider;
    void Start()
    {
        _inventoryController.OnIsSelecting += HandleSelectingState;
        _particles.SetActive(false);
    }
    
    void HandleSelectingState(bool isSelecting)
    {
        if (isSelecting)
        {
            _particles.SetActive(false);
            _collider.enabled = false;
        }
        else
        {
            _particles.SetActive(true);
            _collider.enabled = true;
        }
        
    }
    
    
    
}
