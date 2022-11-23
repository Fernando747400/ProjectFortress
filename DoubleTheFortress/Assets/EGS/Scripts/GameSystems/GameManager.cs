using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private InputActionReference _inputAReference;

    private int _zombieKills;
    private bool _isPaused;
    private bool _gameStarted;
    private bool _gameFinished;
    private bool _inMainMenu;
    private bool _zombieDanceScene;
    
    public int Kills { get => _zombieKills; }
    public bool IsPaused { get => _isPaused; } //Only zombies use
    public bool GameStarted { get => _gameStarted; }
    public bool GameFinished { get => _gameFinished; }
    public bool ZombieDanceScene { get => _zombieDanceScene; set => _zombieDanceScene = value; }
    public bool MainMenu { get => _inMainMenu; set { if(_inMainMenu == value) return; _inMainMenu = value; UpdateMainMenu(); } }

    public event Action PauseGameEvent;
    public event Action PlayGameEvent;
    public event Action StartGameEvent;
    public event Action FinishGameEvent;
    public event Action GotKillEvent;
    public event Action ScoreScreenEvent;
    public event Action ZombieScreenEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        _inputAReference.action.performed += ctx => RecievePauseInput();
        Prepare();
    }

    private void RecievePauseInput()
    {
        if (_inMainMenu) return;

        if (_zombieDanceScene)
        {
            Debug.Log("Going To Main Menu");
            ZombieScreenEvent?.Invoke();
            return;
        }

        if (_gameFinished)
        {
            Debug.Log("Game Finished");
            ScoreScreenEvent?.Invoke();
            return;
        }  

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
    }

    public void StartGame()
    {
        if (_gameStarted) return;
        StartGameEvent?.Invoke();
        _gameStarted = true;
        if (_isPaused) UnpauseGame();
        Debug.Log("Started Game");
    }
    

    public void FinishGame()
    {
        _gameFinished = true;
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

    private void Prepare()
    {
        _zombieKills = 0;
        _isPaused = true;
        _gameStarted = false;
    }

    private void UpdateMainMenu()
    {
        if (_inMainMenu) UnpauseGame();
        if (!_inMainMenu) ResetValues();
    }

    private void ResetValues()
    {
        _zombieKills = 0;
        _isPaused = true;
        PauseGame();
        _gameStarted = false;
        _gameFinished = false;
        _zombieDanceScene = false;
    }

}
