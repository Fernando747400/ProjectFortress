using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private Two_HandsHolder rightHolder;
    [SerializeField] private Two_HandsHolder leftHolder;
    void Start()
    {
        rightHolder.OnGrabbed += HandleWheelInteraction;
        rightHolder.OnReleased += HandleWheelInteraction;
        leftHolder.OnReleased += HandleWheelInteraction;
        leftHolder.OnGrabbed += HandleWheelInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleWheelInteraction()
    {
        if (leftHolder.IsGrabbing && rightHolder.IsGrabbing)
        {
            print("Ambas Manos estan agarrando el wheel");

        }
    }
    
}
