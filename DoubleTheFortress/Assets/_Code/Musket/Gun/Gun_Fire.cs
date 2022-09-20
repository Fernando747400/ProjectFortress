using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class Gun_Fire : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject hitMarkerRed;
    [SerializeField] GameObject hitMarkerBlue;
    private float bulletMass;
    private float travelTime;
    private bool canFire = true;
    private Vector3 hitPosition;
    private Vector3 gravity = new Vector3(0, 9.8f, 0);
    
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 savedFirePosition;
    private void Update()
    {
        checkInput();
    }
    
    
    private void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            StopAllCoroutines();
            RaycastHit hitScan;
            savedFirePosition = transform.position;
            Physics.Raycast(transform.position, transform.forward,out hitScan, Mathf.Infinity, Physics.DefaultRaycastLayers);
            Debug.DrawLine(transform.position,hitScan.point,Color.red);
            GameObject.Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
            hitPosition = hitScan.point;
            float travelDistance = hitScan.distance;
            travelTime = travelDistance / (bulletSpeed * Time.fixedTime);
            canFire = false;

            StartCoroutine(corWaitForTravel());

        }
        
    }
    
    IEnumerator corWaitForTravel()
    {
        yield return new WaitForSeconds(travelTime);
        
        RaycastHit simulatedHit;
        Vector3 simulatedHitPos = Vector3.zero;
        simulatedHitPos = hitPosition - ((gravity) * travelTime);
        Physics.Raycast(savedFirePosition, simulatedHitPos,out simulatedHit, Mathf.Infinity, Physics.DefaultRaycastLayers);
        GameObject.Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);
        canFire = true;
    }

    
}
