using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class InventoryController : MonoBehaviour
{
 

    [SerializeField] private GameObject hammerHand;
    [SerializeField] private GameObject gunHand;
    [SerializeField] private GameObject disfriHand;
    
    private bool hasObjectSelected = false;
    
    private XRIDefaultInputActions inputActions;
    private XRIDefaultInputActions.XRIRightHandInteractionActions rightHandActions;

    
    void Start()
    {
        inputActions = new XRIDefaultInputActions();
        rightHandActions = inputActions.XRIRightHandInteraction;
        rightHandActions.Select_Hammer.performed += ctx => SelectHammer();
        rightHandActions.Select_Weapon.performed += ctx => SelectWeapon();
        InitializeHandObjects();
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
    
    

