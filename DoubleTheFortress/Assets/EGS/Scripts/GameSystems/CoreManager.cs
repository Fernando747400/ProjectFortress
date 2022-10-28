using System;
using UnityEngine;

public class CoreManager : MonoBehaviour, IPause , IGeneralTarget
{
    [Header("Settings")]
    [SerializeField] private float _maxLife;
    [SerializeField] private float _cooldown;

    public event Action RecievedDamageEvent;

    private float _life;
    private float _currentCooldown;

    private void Start()
    {
        SubscribeToPauseEvents();
        _life = _maxLife;
        _currentCooldown = 0f;
    }

    private void FixedUpdate()
    {
        if (!_isPaused)
        {
            _currentCooldown += Time.deltaTime;
        }
    }

    protected virtual void TakeDamage(float damage)
    {
        if (_isPaused) return;
        if (_currentCooldown < _cooldown) return;
        _currentCooldown = 0f;
        _life -= damage;
        ClampLife();
        CheckLife();
        RecievedDamageEvent?.Invoke();
    }

    private void ClampLife()
    {
        _life = Mathf.Clamp(_life, 0, _maxLife);
    }

    private void CheckLife()
    {
        if (_life <= 0)
        {
            GameManager.Instance.PauseGame();
            GameManager.Instance.FinishGame();
        }
    }

    #region Interface Methods

    private bool _isPaused;
    private bool _isSensitive;
    public bool IsPaused { set => _isPaused = value; }
    public bool Sensitive { get => _isSensitive; set => _isSensitive = value; }
    public float MaxHp { get => _maxLife; set => _maxLife = value; }
    public float CurrentHp { get =>_life; set => _life = value; }

    void Pause()
    {
        _isPaused= true;
    }

    void Unpause()
    {
        _isPaused = false;
    }

    private void SubscribeToPauseEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
    }
    #endregion

}
