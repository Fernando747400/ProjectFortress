using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _zombieKills;
    private float _elapsedTime;
    private bool _isPaused;
    
    public int Kills { get => _zombieKills; }
    public float ElapsedTime { get => _elapsedTime; }

    public event Action PauseGameEvent;
    public event Action PlayGameEvent;
    public event Action StartGameEvent;
    public event Action FinishGameEvent;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Prepare();
    }

    public void StartGame()
    {
        StartGameEvent?.Invoke();
    }

    public void FinishGame()
    {
        FinishGameEvent?.Invoke();
    }

    public void PauseGame()
    {
        _isPaused = true;
        PauseGameEvent?.Invoke();
    }

    public void UnpauseGame()
    {
        _isPaused = false;
        PlayGameEvent?.Invoke();
    }

    public void AddKill() 
    {
        _zombieKills++;
    }

    private void AddTime()
    {
        if (_isPaused) return;
        _elapsedTime += Time.deltaTime;
    }
    private void Prepare()
    {
        _zombieKills = 0;
        _elapsedTime = 0;
        _isPaused = true;
    }
    
}
