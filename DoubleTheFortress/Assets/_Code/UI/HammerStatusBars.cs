using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerStatusBars : MonoBehaviour
{
    [Header("Dependencies")]
    [Header("Information Bars")]
    [SerializeField] private GameObject _barsCanvas;
    [SerializeField] private InformationBar _healthBar;
    [SerializeField] private InformationBar _upgradeBar;

    [Header("Hammer")]
    [SerializeField] private GameObject _hammer;
    [SerializeField] private Hamer_Grab _hamer_Grab;

    [Header("Settings")]
    [SerializeField] private float _showCanvasTime;

    private bool _isRunningShowCanvas;

    private void Start()
    {
        _hamer_Grab.ConstructableHitEvent += GetConstrutableEvent;
    }

    private void GetConstrutableEvent(GameObject constructable)
    {
        UpdateBars(constructable.GetComponent<IConstructable>());
        if (!_isRunningShowCanvas) StartCoroutine(ShowCanvas(_showCanvasTime));
    }

    private void UpdateBars(IConstructable constructable)
    {
        _healthBar.UpdateBar(constructable.CurrentHealth, constructable.MaxHealth);
        _upgradeBar.UpdateBar(constructable.UpgradePoints, constructable.UpgradePointsRequired);
    }

    private IEnumerator ShowCanvas(float showForTime)
    {
        _isRunningShowCanvas = true;
        _barsCanvas.SetActive(true);
        yield return new WaitForSeconds(showForTime);
        _barsCanvas.SetActive(false);
        _isRunningShowCanvas=false;
    }
}
