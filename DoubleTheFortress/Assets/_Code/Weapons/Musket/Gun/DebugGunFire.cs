using System.Collections;
using DebugStuff.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public InputActionReference GunShoot;
    
    //saves the position the gun was... 
    //in when it fired the  hitScan
    private Vector3 _savedFirePosition;
    private PlayerSelectedItem selectedItem;

    private void Start()
    {
        GunShoot.action.performed += ctx => HarcodeShoot();
        
    }

    //Comentado para el testeo de vr y porque te quedaste dormido uwu suerte con tu bug xD
    /*
    private void Update()
    {
        CheckInput();
    }
    */

    private void HarcodeShoot()
    {
        selectedItem = InventoryController.Instance.SelectedItem;
        if(_canFire && selectedItem == PlayerSelectedItem.Musket) FireHitScan();
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
        if (!Physics.Raycast(transform.position, transform.forward, out hitScan, maxDistance,
                Physics.DefaultRaycastLayers)) return;
        
        Debug.DrawLine(transform.position,hitScan.point,Color.red);

        Instantiate(hitMarkerRed, hitScan.point, Quaternion.identity);
        _hitPosition = hitScan.point;
        float travelDistance = hitScan.distance;
        _travelTime = travelDistance / (bulletSpeed);
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
        print(simulatedHitPos);
        Physics.Raycast(_savedFirePosition, simulatedHitPos,out simulatedHit, maxDistance, Physics.DefaultRaycastLayers);
        Debug.DrawLine(transform.position,simulatedHit.point,Color.blue);
        Instantiate(hitMarkerBlue, simulatedHit.point, Quaternion.identity);
        
        if (simulatedHit.collider != null)
        {
            Debug.Log("Hit: " + simulatedHit.collider.name);
        }
            
        // if (TryGetGeneralTarget(simulatedHit.collider.gameObject))
        // {
        //     Debug.Log("TryGetGeneralTarget is true");
        //     simulatedHit.collider.gameObject.GetComponent<IGeneralTarget>().ReceiveRayCaster(gameObject, damage);
        //     
        // }
        _canFire = true;
        
    }
  
}