using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void MainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }
}
