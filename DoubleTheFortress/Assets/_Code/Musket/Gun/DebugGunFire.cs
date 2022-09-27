using System.Collections;
using UnityEngine;

public class DebugGunFire :  IGeneralOffender
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject hitMarkerRed;
    [SerializeField] GameObject hitMarkerBlue;
    private float _bulletMass;
    private float _travelTime;
    private bool _canFire = true;
    private Vector3 _hitPosition;
    private Vector3 _gravity = new Vector3(0, 9.8f, 0);
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 _savedFirePosition;
    private void Update()
    {
        CheckInput();
    }
    
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire)
        {
            FireHitScan();
        }
    }

    private void FireHitScan()
    {
        StopAllCoroutines();
        RaycastHit hitScan;
        _savedFirePosition = transform.position;
        Physics.Raycast(transform.position, transform.forward,out hitScan, Mathf.Infinity, Physics.DefaultRaycastLayers);
        Debug.DrawLine(transform.position,hitScan.point,Color.red);

        Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
        _hitPosition = hitScan.point;
        float travelDistance = hitScan.distance;
        _travelTime = travelDistance / (bulletSpeed * Time.fixedTime);
        _canFire = false;

        StartCoroutine(CorWaitForTravel());
        
    }
    
    private IEnumerator CorWaitForTravel()
    {
        yield return new WaitForSeconds(_travelTime);
        FireSimulated();
        
    }

    private void FireSimulated()
    {
        RaycastHit simulatedHit;
        Vector3 simulatedHitPos = Vector3.zero;
        simulatedHitPos = _hitPosition - ((_gravity) * _travelTime);
        Physics.Raycast(_savedFirePosition, simulatedHitPos,out simulatedHit, Mathf.Infinity, Physics.DefaultRaycastLayers);
        Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);
        if (TryGetGeneralTarget(simulatedHit.collider.gameObject))
        {
            simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, Damage);
            
        }
        _canFire = true;
        
    }
  
}