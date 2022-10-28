using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxAreasInteraction : MonoBehaviour
{
    [SerializeField] private Collider triggerZone;
    
    public Action<bool> OnHandEnterActionZone;
    private Action OnInitializeBox;
    

    private void Start()
    { 
        OnInitializeBox?.Invoke();
        triggerZone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            OnHandEnterActionZone?.Invoke(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            OnHandEnterActionZone?.Invoke(true);
        }
    }

    
}