using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Header("LifeBar")]
    [SerializeField] private GameObject _coreLifeBarCanvas;
    [SerializeField] private InformationBar _coreLifeBar;
    [Header("Kill Counter")]
    [SerializeField] private GameObject _killCounterCanvas;
    [SerializeField] private TextMeshProUGUI _killCounterText;
    [Header("Time Counter")]
    [SerializeField] private GameObject _timeCounterCanvas;
    [SerializeField] private TextMeshProUGUI _timeCounterText;

    private void Start()
    {
        CoreManager.Instance.RecievedDamageEvent += UpdateCoreLifeBar;
    }

    private void FixedUpdate()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _timeCounterText.text = GameManager.Instance.ElapsedTime.ToString("F2");
    }
    
    private void UpdateCoreLifeBar()
    {
        _coreLifeBar.UpdateBar(CoreManager.Instance.CurrentHp, CoreManager.Instance.MaxHp);
        iTween.PunchScale(_coreLifeBarCanvas, Vector3.one * 0.1f, 0.5f);
    }
}
