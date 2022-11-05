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
    [SerializeField] private Vector3 _canvasOffset;

    private bool _isRunningShowCanvas;
    private Camera _mainCamera;
    private Collider _hammerCollider;

    private void Start()
    {
        _hamer_Grab.ConstructableHitEvent += GetConstrutableEvent;
        _isRunningShowCanvas = false;
        _barsCanvas.SetActive(false);
        _mainCamera = Camera.main;
        _hammerCollider = _hammer.GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        _barsCanvas.transform.position = _hammerCollider.bounds.center + _canvasOffset;
        _barsCanvas.transform.LookAt(_mainCamera.transform);
    }

    public void ResetCanvasBars()
    {
        StopCoroutine(ShowCanvas(_showCanvasTime));
        _barsCanvas.SetActive(false);
        _isRunningShowCanvas = false;
    }
    private void GetConstrutableEvent(GameObject constructable)
    {
        if(constructable.TryGetComponent(typeof(IConstructable), out Component iConstructable))
        {
            UpdateBars(iConstructable.GetComponent<IConstructable>());
            if (!_isRunningShowCanvas) StartCoroutine(ShowCanvas(_showCanvasTime));
        }
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
