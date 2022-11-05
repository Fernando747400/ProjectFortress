using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Dependencies")]
    [SerializeField] private Transform _totemPosition;
    [SerializeField] private GameObject _skullPrefab;
    [SerializeField] private Pooling _skullPooler;

    [Header("UI Canvas")]
    [SerializeField] private GameObject _tutorialCanvas;
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _finishCanvas;
    
    [Header("Kill Counter")]
    [SerializeField] private GameObject _killCounterCanvas;
    [SerializeField] private TextMeshProUGUI _killCounterText;

    [Header("Settings")]
    [SerializeField] private iTween.EaseType _easeType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Prepare();
    }
    
    public void ZombieDeadEffect(Transform zombiePosition) 
    {
        GameObject skull = _skullPooler.GetObject(_skullPrefab);
        skull.transform.position = zombiePosition.position;
        iTween.MoveTo(skull, iTween.Hash("position", _totemPosition.position, "time", 10f,"easetype", _easeType, "oncomplete", "ResetSkullPosition", "oncompletetarget", gameObject, "oncompleteparams", skull));
    }

    private void ResetSkullPosition(GameObject skull)
    {
        _skullPooler.RecicleObject(_skullPrefab, skull);
    }

    private void UpdateKillCounter()
    {
        _killCounterText.text = GameManager.Instance.Kills.ToString("000");
        iTween.PunchScale(_killCounterCanvas, Vector3.one * 0.001f, 0.5f);
    }
   
    private void UpdateCoreLife(GameObject heart)
    {
        heart.transform.parent = null;
        iTween.MoveTo(heart, iTween.Hash("position", _totemPosition.position, "time", 10f, "easetype", _easeType));
    }

    private void CloseTutorialCanvas()
    {
        _tutorialCanvas.SetActive(false);
    }

    private void OpenPauseCanvas()
    {
        if (!GameManager.Instance.GameStarted) return;
        if (GameManager.Instance.GameFinished) return;
        _pauseCanvas.SetActive(true);
    }

    private void ClosePauseCanvas()
    {
        _pauseCanvas.SetActive(false);
    }

    private void FinishGameCanvas()
    {
        _finishCanvas.SetActive(true);
    }

    private void Prepare()
    {
        CoreManager.Instance.LostAHeartEvent += UpdateCoreLife;
        GameManager.Instance.GotKillEvent += UpdateKillCounter;
        GameManager.Instance.StartGameEvent += CloseTutorialCanvas;
        GameManager.Instance.PauseGameEvent += OpenPauseCanvas;
        GameManager.Instance.PlayGameEvent += ClosePauseCanvas;
        GameManager.Instance.FinishGameEvent += FinishGameCanvas;
        _skullPooler.Preload(_skullPrefab, 10);
    }
    
}
