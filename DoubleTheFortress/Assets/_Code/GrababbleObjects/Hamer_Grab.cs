using System;
using TMPro;
using UnityEngine;

public class Hamer_Grab : IGrabbable
{

    #region Variables

    [Header("Inventory")] 
    [SerializeField] private InventoryController _inventoryController;

    [Header("Settings")]
    [SerializeField] private float _pointsToRepair;
    [SerializeField] private float _pointsToUpgrade;
    [SerializeField] private float _cooldown;

    private float _elapsedTime = 0f;

    public event Action<GameObject> ConstructableHitEvent;
    public event Action DisableHammerEvent;
    

    #endregion

    #region unity Methods
    void Start()
    {
        _inventoryController.OnIsSelecting += HandleIsSelectingState;
    }

    void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
    }
    

    #endregion
   

    #region public Methods
    
    
    #endregion

    #region private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IConstructable>() != null && _elapsedTime >= _cooldown)
        {
            _elapsedTime = 0f;
            Debug.Log("<color=#FFB233>Receive Hammer</color>");
            other.GetComponent<IConstructable>().RecieveHammer(_pointsToRepair, _pointsToUpgrade);
            ConstructableHitEvent?.Invoke(other.gameObject);
        }
    }

    void HandleIsSelectingState(bool isSelecting)
    {
        Collider collider = GetComponent<Collider>();

        if (isSelecting)
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
    }
    #endregion

}
