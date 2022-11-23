using System;
using UnityEngine;

public class GameProgression : MonoBehaviour, IPause
{
    public static GameProgression Instance;

    private double _elapsedTime;
    private int _currentMinute;
    private int _specialSpawn;

    public double ElapsedTime { get => _elapsedTime; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _isPaused = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetValues();
        SubscribeToPauseEvents();
    }

    private void FixedUpdate()
    {
        AddTime();
        ProgressGame();
    }

    private void ProgressGame()
    {
        if (_isPaused) return;
        if (GameManager.Instance.MainMenu) return;
        TimeSpan currentTime = TimeSpan.FromSeconds(_elapsedTime);

        if (currentTime.Minutes > _currentMinute && currentTime.Minutes > 3 && currentTime.Minutes <= 6)
        {
            EnemyManagger.Instance.Damage += 10f;
            EnemyManagger.Instance.ZombieLife += 5f;
            Debug.Log("Added more damage and life");
            EnemyManagger.Instance.StrongZombie = true;
        } 

        if (currentTime.Minutes > _currentMinute && currentTime.Minutes > 6 && currentTime.Minutes <= 10)
        {
            EnemyManagger.Instance.Damage += 20f;
            EnemyManagger.Instance.ZombieLife += 10f;
            Debug.Log("Added more damage and life");
            EnemyManagger.Instance.StrongZombie = true;
        }

        if (currentTime.Minutes > _currentMinute && currentTime.Minutes > 10)
        {
            EnemyManagger.Instance.Damage += 50f;
            EnemyManagger.Instance.ZombieLife += 50f;
            Debug.Log("Added more damage and life");
            EnemyManagger.Instance.StrongZombie = true;
        }

        if (currentTime.Minutes > _currentMinute)
        {
            EnemyManagger.Instance.SpawnWithDelay(1.5f, 3);
            EnemyManagger.Instance.Damage += 5f;
            Debug.Log("Spawned 3 more zombies");
            _currentMinute++;

            if(_specialSpawn * 3 == _currentMinute)
            {
                EnemyManagger.Instance.SpawnSpecialZombie();
                _specialSpawn++;
            }
        }

    }

    private void AddTime()
    {
        if (_isPaused) return;
        if (GameManager.Instance.MainMenu) return;
        _elapsedTime += Time.deltaTime;
    }

    public void ResetValues()
    {
        _elapsedTime = 0;
        _currentMinute = 0;
        _specialSpawn = 1;
    }

    #region Interface methods
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

    private void SubscribeToPauseEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
    }
    #endregion
}
