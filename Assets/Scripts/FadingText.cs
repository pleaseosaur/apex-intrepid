using UnityEngine;
using TMPro;
using System.Collections;

public class FadingText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTextCoroutine(message));
    }

    private IEnumerator FadeTextCoroutine(string message)
    {
        textMesh.text = message;
        textMesh.alpha = 1;

        yield return new WaitForSeconds(0.5f); // Wait a moment before fading

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textMesh.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        textMesh.alpha = 0;
    }
}