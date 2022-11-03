using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class SceneTransition: MonoBehaviour
{
    public static SceneTransition Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }      
    }
    
    public void LoadScene(string SceneToLoad)
    {
        SceneManager.LoadSceneAsync(SceneToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
}