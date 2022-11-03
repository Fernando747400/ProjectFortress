using System.Collections;
using System.Collections.Generic;
using DebugStuff.Inventory;
using UnityEditor.Rendering.Universal;
using UnityEngine;
[RequireComponent(typeof(BoxAreasInteraction))]
public class WheelController : MonoBehaviour
{
    [SerializeField] private HeavyWeaponRotate_VR _heavyWeaponRotateVR;
    [SerializeField] private Two_HandsHolder rightHolder;
    [SerializeField] private Two_HandsHolder leftHolder;
    [SerializeField] private float maxDistance;
    
    private Vector3 _distanceBetweenHands;
    private bool _canMoveCannon = false;
    private InventoryController _inventoryController;
    private BoxAreasInteraction _boxInteraction;
    
    void Start()
    {
        _boxInteraction = GetComponent<BoxAreasInteraction>();
        
        rightHolder.OnGrabbed += HandleWheelInteraction;
        rightHolder.OnReleased += HandleWheelInteraction;
        
        leftHolder.OnReleased += HandleWheelInteraction;
        leftHolder.OnGrabbed += HandleWheelInteraction;

        leftHolder.OnHandsOut += HandleWheelInteraction;
        rightHolder.OnHandsOut += HandleWheelInteraction;
    }

    void Update()
    {
        if (rightHolder.Hand != null && leftHolder.Hand != null)
        {
            if (_canMoveCannon )
            {
                print("MOVE CANNON");
                Vector3 leftHand = leftHolder.Hand.HandPos;
                Vector3 rightHand = rightHolder.Hand.HandPos;
                
                transform.position = GetMidPoint(leftHand, rightHand);
            }
        }
    }

    private Vector3 GetMidPoint(Vector3 p1, Vector3 p2)
    {
        float p3X = (p1.x + p2.x) * .5f;
        float p3Y = (p1.y + p2.y) * .5f;
        float p3Z = (p1.z + p2.z) * .5f;
        Vector3 p3 = new Vector3(p3X, p3Y , p3Z);
       // Debug.Break();
        return p3;

    }

    void HandleWheelInteraction()
    {

        if (_inventoryController== null)
            _inventoryController = _boxInteraction.InventoryPlayer;
        
        _canMoveCannon = false;

        // if (!leftHolder.IsGrabbing || !rightHolder.IsGrabbing)
            // _canMoveCannon = false;
        
        if (leftHolder.IsGrabbing)
        {
            if (_inventoryController.SelectedItem == PlayerSelectedItem.None || _inventoryController.SelectedItem == PlayerSelectedItem.Torch)
            {
                _canMoveCannon = true;
            }
            
        }

    }




}
