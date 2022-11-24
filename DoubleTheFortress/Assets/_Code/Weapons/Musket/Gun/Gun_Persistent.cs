using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun_Persistent : MonoBehaviour, IPause
{

    [Header("Dependencies")] 
    [SerializeField] private float cooldown;

    private float _elapsedTime = 0;
    
    public float Cooldown {get => cooldown; set => cooldown = value; }
    
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
        return _elapsedTime >= cooldown;
    }
    
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
}
