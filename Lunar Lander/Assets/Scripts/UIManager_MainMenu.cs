using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ViewInstructionsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(true);
    }

    public void Return()
    {
        instructionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}