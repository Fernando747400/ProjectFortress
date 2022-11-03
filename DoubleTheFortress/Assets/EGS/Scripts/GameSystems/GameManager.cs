using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _zombieKills;
    private double _elapsedTime;
    private bool _isPaused;
    
    public int Kills { get => _zombieKills; }
    public double ElapsedTime { get => _elapsedTime; }
    public bool IsPaused { get => _isPaused; } //Only zombies use

    public event Action PauseGameEvent;
    public event Action PlayGameEvent;
    public event Action StartGameEvent;
    public event Action FinishGameEvent;
    public event Action GotKillEvent;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Prepare();
    }

    private void FixedUpdate()
    {
        AddTime();
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
        GotKillEvent?.Invoke();
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
        _isPaused = false;
        Debug.LogWarning("The game manager time starts on play for dev purposes. Be sure to change the value to false before game deploy.");
    }
    
}
