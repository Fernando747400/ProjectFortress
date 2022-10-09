using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


public class InventoryController : MonoBehaviour
{
 

    public InputDeviceCharacteristics rightController;    

    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    
    public bool hasObjectSelected = false;

    public InputActionReference hammerReference;
    public InputActionReference gunReference;

    private InputDevice rightHand;


    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(rightController, devices);
        if (devices.Count > 0)
        {
            rightHand = devices[0];
        }
        
        hammerReference.action.performed += ctx => SelectHammer();
        gunReference.action.performed += ctx => SelectWeapon();
        InitializeHandObjects();
    }

    private void Update()
    {
        rightHand.TryGetFeatureValue(CommonUsages.grip, out float gripValue);

        if (gripValue < 0.1f)
        {
            if (!hasObjectSelected) return;
            DeselectHammer();
        }
    }

    void InitializeHandObjects()
    {
        hammerHand.SetActive(false);
        gunHand.SetActive(false);
        hasObjectSelected = false;
    }
    void SelectHammer()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
        }
        else
        {
            hammerHand.SetActive(true);
            hasObjectSelected = true;
        }
       
    }
    
    void DeselectHammer()
    {
        hammerHand.SetActive(false);
        hasObjectSelected = false;
    }
    void SelectWeapon()
    {
        if (hasObjectSelected)
        {
            InitializeHandObjects();
        }
        else
        {
            gunHand.SetActive(true);
            hasObjectSelected = true;

        }
    }
}
    
    

