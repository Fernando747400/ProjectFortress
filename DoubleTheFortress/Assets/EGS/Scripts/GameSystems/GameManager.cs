using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private InputActionReference _inputAReference;

    private int _zombieKills;
    private double _elapsedTime;
    private bool _isPaused;
    private bool _gameStarted;
    private bool _gameFinished;
    private int _currentMinute;
    
    public int Kills { get => _zombieKills; }
    public double ElapsedTime { get => _elapsedTime; }
    public bool IsPaused { get => _isPaused; } //Only zombies use
    public bool GameStarted { get => _gameStarted; }

    public event Action PauseGameEvent;
    public event Action PlayGameEvent;
    public event Action StartGameEvent;
    public event Action FinishGameEvent;
    public event Action GotKillEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _inputAReference.action.performed += ctx => RecievePauseInput();
        Prepare();
    }

    private void Update()
    {
        // RecievePauseInput();
    }

    private void FixedUpdate()
    {
        AddTime();
        ProgressGame();     
    }

    private void RecievePauseInput()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
            if (_gameFinished) return;

            if (!_gameStarted)
            {
                StartGame();
                return;
            }

            if (_isPaused)
            {
                UnpauseGame();
                return;
            }

            if (!_isPaused)
            {
                PauseGame();
                return;
            }
        // }
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
        Debug.Log("Paused Game");
    }

    public void UnpauseGame()
    {
        _isPaused = false;
        PlayGameEvent?.Invoke();
        Debug.Log("Unpaused Game");
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
            Debug.Log("Added more damage");
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
