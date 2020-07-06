using System;
using TMPro;
using UnityEngine;

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
        PlayerModel.onScoreUpdate += UpdateScore;
        PlayerModel.onScoreUpdate += EnableLandingMenu;
        PlayerModel.onStatResetOnNewLevel += ResetStatsOnNewLevel;

    }

    void OnDisable()
    {
        PlayerModel.onStatUpdateI -= UpdateStatI;
        PlayerModel.onStatUpdateF -= UpdateStatF;
        PlayerModel.onScoreUpdate -= UpdateScore;
        PlayerModel.onScoreUpdate -= EnableLandingMenu;
        PlayerModel.onStatResetOnNewLevel -= ResetStatsOnNewLevel;

    }

    void UpdateStatI(string stat, int value)
    {
        if (stat == "Score")
            scoreText.text = "SCORE: " + value;
    }

    void UpdateStatF(string stat, float value)
    {
        switch (stat)
        {
            case "Fuel":
                fuelText.text = "FUEL: " + (int)value;
                break;
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

    void UpdateScore(bool landingSuccessful, int value)
    {
        if (landingSuccessful)
            scoreText.text = "SCORE: " + value;
    }

    void ResetStatsOnNewLevel(float newTime, float newFuel)
    {
        timeText.text = "TIME: " + ((int)(newTime / 60)).ToString() + ":" + ((int)(newTime % 60)).ToString("00");
        fuelText.text = "FUEL: " + (int)newFuel;
    }

    void EnableLandingMenu(bool landingSuccessful, int scoreDisplayValue)
    {
        landingMenu.GetComponent<LandingMenu>().InitializeValues(landingSuccessful, scoreDisplayValue);
        landingMenu.SetActive(true);
    }

    public void SetPauseMenuActive()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf ? true : false);

        if (onPauseChange != null)
            onPauseChange(pauseMenu.activeSelf ? true : false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}