using System.Collections;
using UnityEngine;

public class DebugGunFire :  GeneralAgressor
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
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
    protected virtual  void Update()
    {
        CheckInput();
    }
    
    protected virtual void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire)
        {
            FireHitScan();
        }
    }

    protected void FireHitScan()
    {
        StopAllCoroutines();
        RaycastHit hitScan;
        if (!Physics.Raycast(transform.position, transform.forward, out hitScan, maxDistance,
                Physics.DefaultRaycastLayers)) return;
        
        Debug.DrawLine(transform.position,hitScan.point,Color.red);

        Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
        _hitPosition = hitScan.point;
        float travelDistance = hitScan.distance;
        _travelTime = travelDistance / (bulletSpeed) * Time.fixedDeltaTime;
        _canFire = false;

        StartCoroutine(CorWaitForTravel());
        
    }
    
    private IEnumerator CorWaitForTravel()
    {
        yield return new WaitForSeconds(_travelTime);
        FireSimulated();
    }

    protected void FireSimulated()
    {
        RaycastHit simulatedHit;
        Vector3 simulatedHitPos = Vector3.zero;
        
        simulatedHitPos = _hitPosition - ((_gravity) * _travelTime) ;
        Physics.Raycast(_savedFirePosition, simulatedHitPos,out simulatedHit, maxDistance, Physics.DefaultRaycastLayers);
        Debug.DrawLine(transform.position,simulatedHit.point,Color.blue);
        Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);

        if (TryGetGeneralTarget(simulatedHit.collider.gameObject))
        {
            Debug.Log("TryGetGeneralTarget is true");
            simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
            
        }
        _canFire = true;
        
    }
  
}