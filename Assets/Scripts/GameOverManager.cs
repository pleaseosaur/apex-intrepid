using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject confirmRestartScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject priceScreen;
    [SerializeField] private float fadeDuration = 15f;
    [SerializeField] private float delayBeforeConfirmRestart = 0.25f;

    private CanvasGroup gameOverCanvasGroup;

    private void Start()
    {
        // Ensure the screens are initially hidden
        gameOverScreen.SetActive(false);
        confirmRestartScreen.SetActive(false);

        // Get or add CanvasGroup component to Game Over Screen
        gameOverCanvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
        if (gameOverCanvasGroup == null)
        {
            gameOverCanvasGroup = gameOverScreen.AddComponent<CanvasGroup>();
        }
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverSequence());
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator GameOverSequence()
    {
        // hide the HUD
        menuScreen.SetActive(false);
        priceScreen.SetActive(false);
        
        // Activate and fade in the Game Over Screen
        gameOverScreen.SetActive(true);
        gameOverCanvasGroup.alpha = 0f;
        yield return StartCoroutine(FadeIn(gameOverCanvasGroup));

        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeConfirmRestart);

        // Show the Confirm Restart screen
        confirmRestartScreen.SetActive(true);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}