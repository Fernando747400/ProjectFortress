using System;
using System.Collections.Generic;
using UnityEngine;

public class CoreManager : MonoBehaviour, IPause , IGeneralTarget
{
    public static CoreManager Instance;

    [Header("Settings")]
    [SerializeField] private float _cooldown;
    [SerializeField] private float _heartValue;
    [SerializeField] private List<GameObject> _heartsList;
    
    private Queue<GameObject> _heartsQueue = new Queue<GameObject>();
    
    public event Action RecievedDamageEvent;
    public event Action<GameObject> LostAHeartEvent;

    private float _maxLife;
    private float _life;
    private float _currentCooldown;
    private float _cumulativeDamage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SubscribeToPauseEvents();
        Prepare();
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
        CumulativeDamage(damage);
        ClampLife();
        RecievedDamageEvent?.Invoke();
        CheckLife();
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

    private void CumulativeDamage(float damage)
    {
        _cumulativeDamage += damage;
        
       while (_cumulativeDamage >= _heartValue)
        {
            _cumulativeDamage -= _heartValue;
            if (_heartsQueue.Count == 0) return;
            LostAHeartEvent(_heartsQueue.Dequeue());
        }
    }

    private void Prepare()
    {
        _maxLife = _heartValue * _heartsList.Count;
        foreach (GameObject heart in _heartsList)
        {
            _heartsQueue.Enqueue(heart);
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
