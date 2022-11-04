using System.Collections;
using UnityEngine;

public class DebugGunFire :  GeneralAgressor
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] GameObject hitMarkerRed;
    [SerializeField] GameObject hitMarkerBlue;
    private float _bulletMass;
    protected float _travelTime;
    private bool _canFire = true;
    private Vector3 _hitPosition;
    private Vector3 _gravity = new Vector3(0, 9.8f, 0);
    private Vector3 localForward = new Vector3(0, 0, 1);
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 _savedFirePosition;
    protected virtual  void Update()
    {
        CheckInput();
    }
    
    
    protected virtual void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)  | _canFire)
        {
            FireHitScan();
        }
    }

    protected virtual void FireHitScan()
    {
        StopAllCoroutines();
        if (!Physics.Raycast( transform.position, transform.forward, out RaycastHit hitScan, maxDistance,
                Physics.DefaultRaycastLayers)) return;
        Debug.DrawLine(transform.position,hitScan.point,Color.red,10f);
        Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
        _hitPosition = hitScan.point;
        _travelTime = hitScan.distance / (bulletSpeed) * Time.fixedDeltaTime;
        _canFire = false;

        StartCoroutine(CorWaitForTravel());
    }
    
    private IEnumerator CorWaitForTravel()
    {
        // Debug.Log("waiting " + _travelTime + " seconds");
        yield return new WaitForSeconds(_travelTime);
        FireSimulated();
    }

    protected virtual void FireSimulated()
    {
        _savedFirePosition = transform.position;
        Vector3 simulatedHitPos = _hitPosition - _savedFirePosition;
        //Vector3 simulatedHitPos = (_hitPosition - new Vector3(0, _gravity.y / _travelTime / Time.fixedDeltaTime, 0)) - _savedFirePosition;
        Debug.Log(_hitPosition);
        
        Physics.Raycast(_savedFirePosition, simulatedHitPos.normalized,out RaycastHit simulatedHit,
            maxDistance, Physics.DefaultRaycastLayers);
        Debug.DrawLine(_savedFirePosition,simulatedHit.point,Color.cyan);
        Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);
        
        if (TryGetGeneralTarget(simulatedHit.collider.gameObject))
        {
            simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
        }
        _canFire = true;
        
    }
  
}