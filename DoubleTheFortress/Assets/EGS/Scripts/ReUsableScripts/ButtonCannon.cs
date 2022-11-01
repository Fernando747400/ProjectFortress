using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCannon : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float delay;

    public Action OnPushedButton;
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        OnPushedButton += FireCannon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            OnPushedButton?.Invoke();
            
        }
    }

    void FireCannon()
    {
        Debug.Log("FIRECannon");
        
    }
}
