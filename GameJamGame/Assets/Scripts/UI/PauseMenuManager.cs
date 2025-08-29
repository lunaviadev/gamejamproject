using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    // Optional: For hooking up a Resume button in the UI
    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    // Optional: Quit button
    public void OnQuitButtonClicked()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
