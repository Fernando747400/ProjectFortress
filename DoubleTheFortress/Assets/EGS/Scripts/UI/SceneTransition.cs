using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string SceneToLoad;
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(SceneToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }

}