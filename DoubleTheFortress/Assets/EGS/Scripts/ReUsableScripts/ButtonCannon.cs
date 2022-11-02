using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonCannon : MonoBehaviour
{
    [Header("Cannon config")]
    [SerializeField] private Collider _collider;
    [SerializeField] private float delay;
    [SerializeField] private bool _isAutomatic;

    [Header("Cannon UI")] 
    
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private Color[] _myColors;

   [SerializeField]  private Color _lerpedColor;
   [SerializeField] [Range(0, 1f)] private float _maxValue = 0.5f;
    
    float _time;
    float _intialTimer = 0;
    private float _initialValue;
    private bool _timerHasStarted;
    private bool _timerHasFinished;
    private int _colorIndex;

    public Action OnPushedButton;
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        OnPushedButton += FireCannon;
        StartTimer();
    }

    void FixedUpdate()
    {
        HandleTimer();
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
        //.Log("FIRECannon");
        StartTimer();
    }

    void HandleUICannon(float value)
    {
       _initialValue = Mathf.Lerp(0, _maxValue, value);
       _lerpedColor = Color.Lerp(background.material.color, _myColors[_colorIndex], value);

      // Debug.Log(_initialValue);
       background.material.color = _lerpedColor;
       fill.fillAmount = _initialValue;
    }

    void StartTimer()
    {
        _time = _intialTimer;
        _timerHasStarted = true;
        _timerHasFinished = false;
    }

    void HandleTimer()
    {
        if (!_timerHasStarted && _timerHasFinished) return;
        
        _time += Time.deltaTime;
       // Debug.Log(_time);
        if (_time > delay)
        {
            RestartTimer();
        }

        
        if (_time > 1 && _time < 1.9f)
        {
            _colorIndex = 1;
        }
        else if (_time > 2 && _time < 2.5f)
        {
            _colorIndex = 2;
        }
            
        HandleUICannon(_time);
    }

    void RestartTimer()
    {
        _time = 0;
        _timerHasFinished = true;
        _colorIndex = 0;
        
        if(_isAutomatic) StartTimer();
    }
    
}
