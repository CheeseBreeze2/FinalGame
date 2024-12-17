using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("ControlsMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTutorialControls()
    {
        SceneManager.LoadScene("ControlsTutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
