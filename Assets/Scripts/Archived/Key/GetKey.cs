using System.Collections;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    private string scene = "Level_2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(NextLevel());
        }
    }

    private IEnumerator NextLevel()
    {
        // Make the key invisible
        // Renderer[] renderers = GetComponentsInChildren<Renderer>();
        // foreach (Renderer renderer in renderers)
        // {
        //     renderer.enabled = false;
        // }

        // Wait for a short duration
        yield return new WaitForSeconds(1f);

        // Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
