using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCannon : MonoBehaviour
{
    [Header("Cannon config")]
    [SerializeField] private Collider _collider;
    [SerializeField] private float delay;

    [Header("Cannon UI")] 
    
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private Color[] _mycColors;

   [SerializeField] [Range(0f, 1f)] private float _lerpColor;
   [SerializeField] [Range(0, 0.5f)] private float _maxValue = 0.5f;
    
    float _time;

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

    void HandleUICannon(float value)
    {
        fill.fillAmount = value;
    }
}
