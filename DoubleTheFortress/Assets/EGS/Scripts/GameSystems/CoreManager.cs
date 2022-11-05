using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreManager : MonoBehaviour, IPause , IGeneralTarget
{
    public static CoreManager Instance;

    [Header("Settings")]
    [SerializeField] private float _cooldown;
    [SerializeField] private float _heartValue;
    [SerializeField] private List<GameObject> _heartsList;

    [Header("Audio Clips")]
    public AudioClip DamageSound;
    public AudioClip DeathSound;

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
        //StartCoroutine(TestMethod());
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
            PlayAudio(DeathSound, 0.5f);
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
            LostAHeartEvent?.Invoke(_heartsQueue.Dequeue());
            PlayAudio(DamageSound, 0.5f);
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

    private void PlayAudio(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
    }

    private IEnumerator TestMethod()
    {
        yield return new WaitForSeconds(5f);
        TakeDamage(70f);
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
