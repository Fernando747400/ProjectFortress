using System;
using UnityEngine;

public class Hamer_Grab : MonoBehaviour, IGrabbable
{

    #region Variables

    [SerializeField] private Collider topCollider;
    [SerializeField] private float pointsToRepair;
    [SerializeField] private float pointsToUpgrade;
    
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
        if (other.GetComponent<WallManager>())
        {
            Debug.Log("<color=#FFB233>Receive Hammer</color>");
            WallManager wallManager = other.GetComponent<WallManager>();
            wallManager.ReceiveHammer(pointsToRepair, pointsToUpgrade);
        }
    }

    #endregion
    
}
