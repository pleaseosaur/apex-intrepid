using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public GameObject HUDMenu;
    public GameObject pricePanel;
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    private bool panelActiveBeforePause = false; // for toggling price panel
    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        HUDMenu.SetActive(!isPaused);

        if (isPaused)
        {
            panelActiveBeforePause = pricePanel.activeSelf;
            pricePanel.SetActive(false);
        }
        else
        {
            pricePanel.SetActive(panelActiveBeforePause);
        }

        Time.timeScale = isPaused ? 0 : 1;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
}