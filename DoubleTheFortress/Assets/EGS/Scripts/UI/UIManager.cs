using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _totemPosition;
    [SerializeField] private GameObject _skullPrefab;
    [SerializeField] private Pooling _skullPooler;
    
    [Header("Kill Counter")]
    [SerializeField] private GameObject _killCounterCanvas;
    [SerializeField] private TextMeshProUGUI _killCounterText;

    [Header("Settings")]
    [SerializeField] private iTween.EaseType _easeType;


    private void Start()
    {
        CoreManager.Instance.LostAHeartEvent += UpdateCoreLife;
        GameManager.Instance.GotKillEvent += UpdateKillCounter;
        _skullPooler.Preload(_skullPrefab, 10);
    }
    
    public void ZombieDeadEffect(Transform zombiePosition) 
    {
        GameObject skull = _skullPooler.GetObject(_skullPrefab);
        skull.transform.position = zombiePosition.position;
        iTween.MoveTo(skull, iTween.Hash("position", _totemPosition.position, "time", 2f,"easetype", _easeType, "oncomplete", "ResetSkullPosition", "oncompletetarget", gameObject, "oncompleteparams", skull));
    }

    private void ResetSkullPosition(GameObject skull)
    {
        _skullPooler.RecicleObject(_skullPrefab, skull);
    }

    private void UpdateKillCounter()
    {
        _killCounterText.text = GameManager.Instance.Kills.ToString();
        iTween.PunchScale(_killCounterCanvas, Vector3.one * 0.1f, 0.5f);
    }
   
    
    private void UpdateCoreLife(GameObject heart)
    {
        heart.transform.parent = null;
        iTween.MoveTo(heart, iTween.Hash("position", _totemPosition.position, "time", 10f, "easetype", _easeType));
    }
    

}
