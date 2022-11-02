using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PulseLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _pulseSpeed;
    [SerializeField] private float _minRange;

    private float _initialIntensity;
    private Light _myLight;

    private void Awake()
    {
        _myLight = GetComponent<Light>();
        _initialIntensity = _myLight.range;
        if (_minRange > _initialIntensity) Debug.LogWarning("Minimum range of pulsating light is greater than it's initial range");
    }

    private void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        _myLight.range = _minRange + Mathf.PingPong(Time.time * _pulseSpeed, _initialIntensity - _minRange); 
    }
}
