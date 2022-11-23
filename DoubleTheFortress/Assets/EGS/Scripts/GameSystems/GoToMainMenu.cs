using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.ZombieDanceScene = true;
        GameManager.Instance.ZombieScreenEvent += LoadMainMenuScene;
    }

    private void LoadMainMenuScene()
    {
        Debug.Log("Received main menu event");
        GameManager.Instance.MainMenu = true;
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        GameManager.Instance.ZombieScreenEvent += LoadMainMenuScene;
    }

    private void OnDisable()
    {
        GameManager.Instance.ZombieScreenEvent -= LoadMainMenuScene;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ZombieScreenEvent -= LoadMainMenuScene;
    }
}
