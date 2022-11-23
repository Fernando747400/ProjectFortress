using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class SceneTransition: MonoBehaviour
{
    public static SceneTransition Instance;
    [SerializeField] private GameObject LoadingScreen;

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
        
        LoadingScreen.SetActive(false);
    }
    
    public void LoadScene(string SceneToLoad)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(DelayLoadScene(SceneToLoad));
    }
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator DelayLoadScene(string SceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad);

        while(!asyncLoad.isDone)
        {
            yield return null; 
        }
    }
}