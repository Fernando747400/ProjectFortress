using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action PauseGameEvent;
    public event Action PlayGameEvent;
    public event Action StartGameEvent;
    public event Action FinishGameEvent;

    public void Awake()
    {
        Instance = this;
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
        PauseGameEvent?.Invoke();
    }

    public void UnpauseGame()
    {
        PlayGameEvent?.Invoke();
    }


}
