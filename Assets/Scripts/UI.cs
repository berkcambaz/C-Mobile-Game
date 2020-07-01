using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI
{
    public static GameObject notification;
    public static GameObject hud;
    public static GameObject mainMenu;
    public static GameObject upgradesMenu;
    public static GameObject customizeMenu;
    public static GameObject settingsMenu;

    public static Text mapLevelText;

    public static Text qualityText;
    public static Text particleText;
    public static Text fpsText;

    public static GameObject pauseButton;

    public static GameObject upgradesButton;
    public static GameObject customizeButton;

    public static void Init()
    {
        RefreshSettings();
    }

    public static void Update()
    {
        mapLevelText.text = UserData.mapLevel.ToString();
    }

    public static void RefreshSettings()
    {
        RefreshQualityText();
        RefreshParticleText();
        RefreshFpsText();
    }

    private static void RefreshQualityText()
    {
        switch (UserData.quality)
        {
            case true:
                qualityText.text = "high";
                break;
            case false:
                qualityText.text = "low";
                break;
        }
    }

    private static void RefreshParticleText()
    {
        switch (UserData.particles)
        {
            case 3:
                particleText.text = "low";
                break;
            case 5:
                particleText.text = "medium";
                break;
            case 10:
                particleText.text = "high";
                break;
        }
    }

    private static void RefreshFpsText()
    {
        if (UserData.fps == Screen.currentResolution.refreshRate)
        {
            fpsText.text = "default";
            return;
        }

        fpsText.text = UserData.fps.ToString();
    }
}
