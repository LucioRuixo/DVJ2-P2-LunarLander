using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI horizontalSpeedText;
    public TextMeshProUGUI verticalSpeedText;
    public TextMeshProUGUI timeText;
    public GameObject pauseMenu;
    public GameObject landingMenu;
    public TextMeshProUGUI landingText;

    public static event Action<bool> onPauseChange;

    void OnEnable()
    {
        PlayerModel.onStatUpdateI += UpdateStatI;
        PlayerModel.onStatUpdateF += UpdateStatF;

        PlayerController.onLanding += EnableLandingMenu;
    }

    void OnDisable()
    {
        PlayerModel.onStatUpdateI -= UpdateStatI;
        PlayerModel.onStatUpdateF -= UpdateStatF;

        PlayerController.onLanding -= EnableLandingMenu;
    }
    
    void UpdateStatI(string stat, int value)
    {
        if (stat == "Fuel")
            fuelText.text = "FUEL: " + value;
        else
            scoreText.text = "SCORE: " + value;
    }
    
    void UpdateStatF(string stat, float value)
    {
        switch (stat)
        {
            case "Height":
                heightText.text = "HEIGHT: " + value.ToString("0.0");
                break;
            case "Horizontal speed":
                horizontalSpeedText.text = value >= 0f ? "HORIZONTAL SPEED: " + (int)value + "  >" : "HORIZONTAL SPEED: " + (int)value * -1 + "  <";
                break;
            case "Vertical speed":
                verticalSpeedText.text = value >= 0f ? "VERTICAL SPEED: " + (int)value + " /\\" : "VERTICAL SPEED: " + (int)value * -1 + " \\/";
                break;
            case "Time":
                timeText.text = "TIME: " + ((int)(value / 60)).ToString() + ":" + ((int)(value % 60)).ToString("00");
                break;
            default:
                break;
        }
    }

    void EnableLandingMenu(bool landingSuccessful)
    {
        landingText.text = landingSuccessful ? "YOUR SHIP HAS LANDED!" : "YOUR SHIP CHRASHED!";
        landingMenu.SetActive(true);
    }

    public void SetPauseMenuActive()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf ? true : false);

        if (onPauseChange != null)
            onPauseChange(pauseMenu.activeSelf ? true : false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToNextLevel()
    {
        
    }
}