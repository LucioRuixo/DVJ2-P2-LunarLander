using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LandingMenu : MonoBehaviour
{
    bool landingSuccessful;

    int score;

    public Button button;
    public TextMeshProUGUI landingText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI buttonText;

    public static event Action onLevelSetting;

    void OnEnable()
    {
        if (landingSuccessful)
        {
            landingText.text = "YOUR SHIP HAS LANDED!";
            scoreText.text = "SCORE: " + score;
            buttonText.text = "NEXT LEVEL";

            button.onClick.AddListener(SetLevel);
        }
        else
        {
            landingText.text = "YOUR SHIP CRASHED!";
            scoreText.text = "SCORE: " + score;
            buttonText.text = "RETURN TO MAIN MENU";

            button.onClick.AddListener(ReturnToMainMenu);
        }
    }

    void SetLevel()
    {
        if (onLevelSetting != null)
            onLevelSetting();

        gameObject.SetActive(false);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void InitializeValues(bool newLandingSuccessful, int newScore)
    {
        landingSuccessful = newLandingSuccessful;
        score = newScore;
    }
}