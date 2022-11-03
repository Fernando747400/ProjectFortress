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
    private bool _gameStarted;
    private bool _gameFinished;
    private int _currentMinute;
    
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
        ProgressGame();
        if (Input.GetKeyDown(KeyCode.L)) StartGame();
    }

    public void StartGame()
    {
        if (_gameStarted) return;
        StartGameEvent?.Invoke();
        _gameStarted = true;
        UnpauseGame();
        Debug.Log("Started Game");
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
        _currentMinute = 0;
        _isPaused = true;
        _gameStarted = false;
        //Debug.LogWarning("The game manager time starts on play for dev purposes. Be sure to change the value to false before game deploy.");
    }

    private void ProgressGame()
    {
        if (IsPaused) return;
        TimeSpan currentTime = TimeSpan.FromSeconds(_elapsedTime);

        if (currentTime.Minutes > _currentMinute && currentTime.Minutes > 3)
        {
            EnemyManagger.Instance.Damage += 10f;
        }

        if (currentTime.Minutes > _currentMinute)
        {
            EnemyManagger.Instance.SpawnWithDelay(1.5f,3);
            EnemyManagger.Instance.Damage += 5f;
            Debug.Log("Spawned 3 more zombies");
            _currentMinute++;
        }

    }
    
}
