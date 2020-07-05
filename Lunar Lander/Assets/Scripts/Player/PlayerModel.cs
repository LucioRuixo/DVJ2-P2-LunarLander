using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public PlayerController controller;

    bool gamePaused;

    [HideInInspector] public int fuel;
    [HideInInspector] public int score;

    [HideInInspector] public float height;
    [HideInInspector] public float horizontalSpeed;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public float time;

    public static event Action<string, int> onStatUpdateI;
    public static event Action<string, float> onStatUpdateF;

    void OnEnable()
    {
        UIManager_Gameplay.onPauseChange += SetPause;
    }

    void Start()
    {
        fuel = controller.fuel;
        height = controller.height;

        InitializeStatUI();
    }

    void Update()
    {
        if (!gamePaused)
        {
            fuel = controller.fuel;
            height = controller.height;
            horizontalSpeed = controller.horizontalSpeed;
            verticalSpeed = controller.verticalSpeed;
            time += Time.deltaTime;

            onStatUpdateI("Fuel", fuel);
            onStatUpdateF("Height", height);
            onStatUpdateF("Horizontal speed", horizontalSpeed);
            onStatUpdateF("Vertical speed", verticalSpeed);
            onStatUpdateF("Time", time);
        }
    }

    void OnDisable()
    {
        UIManager_Gameplay.onPauseChange -= SetPause;
    }

    void SetPause(bool state)
    {
        gamePaused = state;
    }

    void InitializeStatUI()
    {
        if (onStatUpdateI != null)
        {
            onStatUpdateI("Fuel", fuel);
            onStatUpdateI("Score", score);
        }

        if (onStatUpdateF != null)
        {
            onStatUpdateF("Height", (int)height);
            onStatUpdateF("Horizontal speed", horizontalSpeed);
            onStatUpdateF("Vertical speed", verticalSpeed);
            onStatUpdateF("Time", time);
        }
    }
}