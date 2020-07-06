using System;
using TMPro;
using UnityEngine;

public class LandingMenu : MonoBehaviour
{
    [HideInInspector] public bool landingSuccessful;

    public TextMeshProUGUI landingText;
    public TextMeshProUGUI setLevelButtonText;

    public static event Action<bool> onLevelSetting;

    void OnEnable()
    {
        landingText.text = landingSuccessful ? "YOUR SHIP HAS LANDED!" : "YOUR SHIP CRASHED!";
        setLevelButtonText.text = landingSuccessful ? "NEXT LEVEL" : "RESET LEVEL";
    }

    public void SetLevel()
    {
        if (onLevelSetting != null)
            onLevelSetting(landingSuccessful);

        gameObject.SetActive(false);
    }
}