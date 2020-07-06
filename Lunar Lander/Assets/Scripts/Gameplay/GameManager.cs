using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform terrains;

    public static event Action onLevelSetting;

    void OnEnable()
    {
        LandingMenu.onLevelSetting += SetLevel;
    }

    void Start()
    {
        SetLevel();
        Debug.Log("Hasta aca llega");
    }

    void OnDisable()
    {
        LandingMenu.onLevelSetting -= SetLevel;
    }

    void SetLevel()
    {
        foreach (Transform terrain in terrains)
        {
            terrain.gameObject.SetActive(false);
        }

        int terrainIndex = UnityEngine.Random.Range(0, 3);
        terrains.GetChild(terrainIndex).gameObject.SetActive(true);

        if (onLevelSetting != null)
            onLevelSetting();
    }
}