using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    [SerializeField] private GameObject disfriHand;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void HandleGameInventory(GameObject objHand, bool active)
    {
        objHand.SetActive(active);
    }
    
    
    
}
