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
    [SerializeField] private GameObject _skullPrefab;
    [SerializeField] private Transform _totemPosition;


    private void Start()
    {
        CoreManager.Instance.RecievedDamageEvent += UpdateCoreLifeBar;
        GameManager.Instance.GotKillEvent += UpdateKillCounter;
    }

    private void FixedUpdate()
    {

    }

    public void ZombieDeadEffect(Transform zombiePosition) 
    {
        _skullPrefab.transform.position = zombiePosition.position;
        iTween.MoveTo(_skullPrefab, iTween.Hash("position", _totemPosition.position, "time", 2f, "oncomplete", "ResetSkullPosition", "oncompletetarget", gameObject));
    }

    private void ResetSkullPosition()
    {
        _skullPrefab.transform.position = new Vector3(0, -10, 0);
    }

    private void UpdateKillCounter()
    {
        _killCounterText.text = GameManager.Instance.Kills.ToString();
        iTween.PunchScale(_killCounterCanvas, Vector3.one * 0.1f, 0.5f);
    }
    
    private void UpdateTimer()
    {
        
    }
    
    private void UpdateCoreLifeBar()
    {
        _coreLifeBar.UpdateBar(CoreManager.Instance.CurrentHp, CoreManager.Instance.MaxHp);
        iTween.PunchScale(_coreLifeBarCanvas, Vector3.one * 0.1f, 0.5f);
    }
    

}
