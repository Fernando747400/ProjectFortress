using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEngine;

public class Torch_Grab : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private GameObject _particles;
    private bool isSelected;
    void Start()
    {
        _particles.SetActive(false);
    }

    void Update()
    {
        
        if (_inventoryController.SelectedItem == PlayerSelectedItem.Selecting)
        {
            isSelected = false;
        }
        
    }
    
    
    
    
}
