using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadFinalScene : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private bool _goingToMenu;

    private void Start()
    {
        GameManager.Instance.ScoreScreenEvent += LoadScene;
    }

    private void LoadScene()
    {
        if (_goingToMenu) GameManager.Instance.MainMenu = true;
        SceneManager.LoadScene(_sceneToLoad);
    }

    private void OnEnable()
    {
        GameManager.Instance.ScoreScreenEvent += LoadScene;
    }

    private void OnDisable()
    {
        GameManager.Instance.ScoreScreenEvent -= LoadScene;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ScoreScreenEvent -= LoadScene;
    }

}
