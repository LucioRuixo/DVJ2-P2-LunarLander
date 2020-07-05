using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;
    public GameObject creditsMenu;

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ViewInstructionsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(true);
    }

    public void ViewCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Return()
    {
        if (instructionsMenu.activeSelf)
            instructionsMenu.SetActive(false);
        else
            creditsMenu.SetActive(false);

        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}