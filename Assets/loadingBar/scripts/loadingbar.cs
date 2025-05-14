using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class loadingbar : MonoBehaviour
{
    [SerializeField] private float loadingTime = 3f;
    private float currentTime = 0f;

    private Image imageComp;

    private void Start()
    {
        imageComp = GetComponent<Image>();
        imageComp.fillAmount = 0f;

        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("No scene to load!");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (currentTime < loadingTime)
        {
            currentTime += Time.deltaTime;
            imageComp.fillAmount = currentTime / loadingTime;
            yield return null;
        }

        // Ensure the scene has finished loading
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }

    private void OnDisable()
    {
        // Reset variables when the script is disabled (scene change)
        currentTime = 0f;
        if (imageComp != null)
            imageComp.fillAmount = 0f;
    }
}