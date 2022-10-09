using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class InventoryController : MonoBehaviour
{
 

    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    [SerializeField] private GameObject disfriHand;
    
    private bool hasObjectSelected = false;
    
    private XRIDefaultInputActions inputActions;
    private XRIDefaultInputActions.XRIRightHandInteractionActions rightHandActions;


    private InputDevice targetDevice;

    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, devices);
        
        
        
        // inputActions = new XRIDefaultInputActions();
        // rightHandActions = inputActions.XRIRightHandInteraction;
        // rightHandActions.Select_Hammer.performed += ctx => SelectHammer();
        // rightHandActions.Select_Weapon.performed += ctx => SelectWeapon();
        InitializeHandObjects();
    }


    private void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("Primary button pressed");
        }
    }

    void InitializeHandObjects()
    {
        hammerHand.SetActive(false);
        gunHand.SetActive(false);
        disfriHand.SetActive(false);
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
    
    

