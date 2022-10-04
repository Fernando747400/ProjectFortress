using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    [SerializeField] private GameObject hammerInv;
    [SerializeField] private GameObject gunInv;
    [SerializeField] private GameObject disfriInv;
    
    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    [SerializeField] private GameObject disfriHand;

    [SerializeField] private GameObject holderHammer;
    [SerializeField] private GameObject holderGun;
    [SerializeField] private GameObject holderdesfri;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void HandleGameInventory(GameObject objHand, GameObject objInv, bool active)
    {
        objHand.SetActive(active);
        objInv.SetActive(!active);
    }
    
    
    
}
