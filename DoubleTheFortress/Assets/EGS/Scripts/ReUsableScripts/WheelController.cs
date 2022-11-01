using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private HeavyWeaponRotate_VR _heavyWeaponRotateVR;
    [SerializeField] private Two_HandsHolder rightHolder;
    [SerializeField] private Two_HandsHolder leftHolder;


    private Vector3 _distanceBetweenHands;
    private Vector3 _middlePoint;
    private bool _canMoveCannon = false;
    void Start()
    {
        rightHolder.OnGrabbed += HandleWheelInteraction;
        rightHolder.OnReleased += HandleWheelInteraction;
        
        leftHolder.OnReleased += HandleWheelInteraction;
        leftHolder.OnGrabbed += HandleWheelInteraction;

        leftHolder.OnHandsOut += HandleWheelInteraction;
        rightHolder.OnHandsOut += HandleWheelInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Left" + leftHolder.IsGrabbing);
        Debug.Log("Right"+ rightHolder.IsGrabbing);
        if (rightHolder.Hand != null && leftHolder.Hand != null)
        {
            _distanceBetweenHands = rightHolder.Hand.gameObject.transform.position - leftHolder.Hand.gameObject.transform.position;

            _middlePoint = _distanceBetweenHands * 0.5f;

            if (_canMoveCannon)
            {
                print("MOVE CANNON");

                _heavyWeaponRotateVR.LookAtRotate(_distanceBetweenHands);
            }
            // Debug.DrawRay();
            // Debug.DrawLine(rightHolder.Hand.gameObject.transform.position, leftHolder.Hand.gameObject.transform.position);
        }
    }

    void HandleWheelInteraction()
    {
        print("send event ");

        if (!leftHolder.IsGrabbing || !rightHolder.IsGrabbing)
        {
            _canMoveCannon = false;
        }

        if (leftHolder.IsGrabbing && rightHolder.IsGrabbing)
        {
            
            print("Entra aqui");
            _canMoveCannon = true;
            // print("CAN MOVE CANNON = " + _canMoveCannon);
        }

    }
    
    
    
    
}
