using System;
using UnityEngine;
using TMPro;

public class UIClock : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshPro _clockText;
    [SerializeField] private GameObject _secondHand;
    [SerializeField] private GameObject _minuteHand;

    [Header("Settings")]
    [SerializeField] private bool _isDigital;

    private TimeSpan _timeSpan;
    private float _secondsDegrees;
    private float _minutesDegrees;

    private void Start()
    {
        if (_isDigital)
        {
            _secondHand.gameObject.SetActive(false);
            _minuteHand.gameObject.SetActive(false);
        } else
        {
            _clockText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_isDigital) 
        {
            CalculateTime();
            UpdateHands();
        } else
        {
            DisplayTime(); 
        }    
    }
    
    private void CalculateTime()
    {
        _timeSpan = TimeSpan.FromSeconds(GameProgression.Instance.ElapsedTime);
        _secondsDegrees = (_timeSpan.Seconds / 60f + _timeSpan.Milliseconds / 60000f) * 360f;
        _minutesDegrees = (_timeSpan.Minutes / 60f + _timeSpan.Seconds / 3600) * 360f;
    }
    
    private void UpdateHands()
    {
        _secondHand.transform.localRotation = Quaternion.Euler(0f, _secondsDegrees, 0f);
        _minuteHand.transform.localRotation = Quaternion.Euler(0f, _minutesDegrees, 0f);
    }

    private void DisplayTime()
    {
        _clockText.text = TimeSpan.FromSeconds(GameProgression.Instance.ElapsedTime).ToString(@"mm\:ss");
    }
}
