using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class InventoryController : MonoBehaviour
{
 

    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    
    private bool hasObjectSelected = false;

    public InputActionReference handReferences;

    void Start()
    {
        handReferences.action.performed += ctx => SelectHammer();
        InitializeHandObjects();
    }

    void InitializeHandObjects()
    {
        hammerHand.SetActive(false);
        gunHand.SetActive(false);
    }
    void SelectHammer()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
            hasObjectSelected = false;
        }
        
        //Handle Hammer Selection
        if(hammerHand.activeSelf)
        {
            hammerHand.SetActive(false);
            hasObjectSelected = false;
            
        }
        else
        {
            hammerHand.SetActive(true);
            hasObjectSelected = true;
        }
    }
    void SelectWeapon()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
            hasObjectSelected = false;
        }
        //Handle Weapon Selection
        if(gunHand.activeSelf)
        {
            gunHand.SetActive(false);
            hasObjectSelected = false;

        }
        else
        {
            gunHand.SetActive(true);
            hasObjectSelected = true;

        }
    }
}
    
    

