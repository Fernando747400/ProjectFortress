using TMPro;
using UnityEngine;

public class HammerPersistent : MonoBehaviour, IPause
{
    [Header("Dependencies")]
    [SerializeField] private float _cooldown;

    private float _elapsedTime = 0;

    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;        
    }
    public void ResetCooldwon()
    {
        _elapsedTime = 0;
    }

    public bool CooldownPassed()
    {
        return _elapsedTime >= _cooldown;
    }

    #region Interface Methods

    private bool _isPaused;
    public bool IsPaused { set => _isPaused = value; }

    void Pause()
    {
        _isPaused = true;
    }

    void Unpause()
    {
        _isPaused = false;
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
    }

    private void UnsubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent -= Pause;
        GameManager.Instance.PlayGameEvent -= Unpause;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            SubscribeToEvents();
            _isPaused = GameManager.Instance.IsPaused;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            UnsubscribeToEvents();
        }
    }
    #endregion
}
