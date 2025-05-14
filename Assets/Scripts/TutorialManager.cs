using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public GameObject panel;
        public Button nextButton;
        public string triggerAction;
    }

    public TutorialStep[] tutorialSteps;
    public GameObject tutorialChoicePanel;
    public Button yesButton;
    public Button noButton;
    public HUDMenu hudMenu;
    public Player_Stats playerStats;

    private bool tutorialActive = false;
    private int currentStepIndex = -1;

    private void Start()
    {
        ShowTutorialChoice();
    }

    private void ShowTutorialChoice()
    {
        tutorialChoicePanel.SetActive(true);
        yesButton.onClick.AddListener(StartTutorial);
        noButton.onClick.AddListener(SkipTutorial);
        SetCursorState(true);
    }

    private void StartTutorial()
    {
        tutorialActive = true;
        tutorialChoicePanel.SetActive(false);
        NextTutorialStep();
    }

    private void SkipTutorial()
    {
        tutorialChoicePanel.SetActive(false);
        SetCursorState(false);
    }

    private void NextTutorialStep()
    {
        if (currentStepIndex >= 0)
        {
            tutorialSteps[currentStepIndex].panel.SetActive(false);
        }

        currentStepIndex++;

        if (currentStepIndex < tutorialSteps.Length)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            currentStep.panel.SetActive(true);
            SetCursorState(true);  // Explicitly set cursor state when showing a panel

            if (currentStep.nextButton != null)
            {
                currentStep.nextButton.onClick.RemoveAllListeners();
                currentStep.nextButton.onClick.AddListener(NextTutorialStep);
            }

            if (!string.IsNullOrEmpty(currentStep.triggerAction))
            {
                StartCoroutine(WaitForTriggerAction(currentStep.triggerAction));
            }
        }
        else
        {
            EndTutorial();
        }
    }

    private IEnumerator WaitForTriggerAction(string action)
    {
        SetCursorState(false);  // Lock cursor during gameplay actions

        switch (action)
        {
            case "ScrollWheel":
                yield return new WaitUntil(() => Input.GetAxis("Mouse ScrollWheel") != 0);
                break;
            case "CategorySelect":
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButtonDown(2));
                break;
            case "BuildBase":
                int initialCash = playerStats.currentCash;
                yield return new WaitUntil(() => playerStats.currentCash < initialCash);
                break;
            case "BuildWeapon":
                initialCash = playerStats.currentCash;
                yield return new WaitUntil(() => playerStats.currentCash < initialCash);
                break;
        }

        SetCursorState(true);  // Unlock cursor after gameplay action
        NextTutorialStep();
    }

    private void EndTutorial()
    {
        tutorialActive = false;
        SetCursorState(false);
    }

    private void SetCursorState(bool unlocked)
    {
        Cursor.lockState = unlocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = unlocked;
    }
}