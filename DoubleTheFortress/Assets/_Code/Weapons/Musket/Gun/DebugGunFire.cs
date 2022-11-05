using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class DebugGunFire :  GeneralAgressor
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    // [SerializeField] private GameObject hitMarkerRed;
    [SerializeField] private GameObject hitMarkerBlue;
    [SerializeField] private LayerMask layers;

    [Tooltip("Cooldown Time in seconds")] [SerializeField]
    private float cooldown = 1;
    
    private float _bulletMass;
    private float _travelTime;
    protected bool canFire = true;
    private Vector3 _hitPosition;
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 _savedFirePosition;
    protected virtual  void Update()
    {
        CheckInput();
    }
    
    
    protected virtual void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)  | canFire)
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
        // Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
        _hitPosition = hitScan.point;
        _travelTime = hitScan.distance / (bulletSpeed) * Time.fixedDeltaTime;
        canFire = false;

        StartCoroutine(CorWaitForTravel());
    }
    
    private IEnumerator CorWaitForTravel()
    {
        // Debug.Log("waiting " + _travelTime + " seconds");
        yield return new WaitForSeconds(_travelTime);
        FireSimulated();
    }

    private IEnumerator CorWaitForCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldown);

        canFire = true;

    }

    protected virtual void FireSimulated()
    {
        _savedFirePosition = transform.position;
        Vector3 simulatedHitPos = _hitPosition - _savedFirePosition;
        //Vector3 simulatedHitPos = (_hitPosition - new Vector3(0, _gravity.y / _travelTime / Time.fixedDeltaTime, 0)) - _savedFirePosition;
        Physics.Raycast(_savedFirePosition, simulatedHitPos.normalized,out RaycastHit simulatedHit,
            maxDistance, layers);
        Debug.DrawLine(_savedFirePosition,simulatedHit.point,Color.cyan);
        Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);
        
        if (TryGetGeneralTarget(simulatedHit.collider.gameObject))
        {
            simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
        }

        StartCoroutine(CorWaitForCooldown());

    }
  
}