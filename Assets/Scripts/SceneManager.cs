
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private GameObject player;
    // -- use this to use the loading scene (whenever it's actually working properly) --
    // [SerializeField] private string loadingSceneName = "LoadingScene";
    //
    // public void LoadScene(string sceneName)
    // {
    //     PlayerPrefs.SetString("SceneToLoad", sceneName);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(loadingSceneName);
    // }
    //
    // public void QuitGame()
    // {
    //     Application.Quit();
    // }
    
    // -- use this to load scenes directly --
    public void LoadScene (string sceneName)
    {
        if (sceneName == "Level_2")
        {
            // assign existing player stats to level 2 player object
            player = GameObject.Find("Player");
            DontDestroyOnLoad(player);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
