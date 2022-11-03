using System.Collections;
using UnityEngine;

public class Musket_Gun : IGrabbable
{

    #region Variables

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
    
   
    
    
    
    #endregion
    
    #region Unity Methods
    private void Update()
    {
        checkInput();
    }
    #endregion


    #region private Methods
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

    #endregion
   

    #region public Methods
    
    

    #endregion
    
    
}
