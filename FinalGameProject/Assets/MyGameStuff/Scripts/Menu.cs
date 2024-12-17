using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}