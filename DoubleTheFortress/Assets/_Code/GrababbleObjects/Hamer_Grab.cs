using System;
using UnityEngine;

public class Hamer_Grab : MonoBehaviour, IGrabbable
{

    #region Variables

    [Header("Settings")]
    [SerializeField] private float pointsToRepair;
    [SerializeField] private float pointsToUpgrade;

    public event Action ConstructableHitEvent;
    
    public Vector3 Position
    {
        get;
        set;
    }

    public Vector3 Rotation
    {
        get;
        set;
    }

    

    #endregion

    #region unity Methods
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    

    #endregion
   

    #region public Methods
    
    
    #endregion

    #region private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IConstructable>() != null)
        {
            Debug.Log("<color=#FFB233>Receive Hammer</color>");
            other.GetComponent<IConstructable>().RecieveHammer(pointsToRepair, pointsToUpgrade);
            ConstructableHitEvent?.Invoke();
        }
    }

    #endregion
    
}
