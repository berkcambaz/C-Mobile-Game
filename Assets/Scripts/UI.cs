using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI
{
    public static GameObject hud;
    public static GameObject mainMenu;
    public static GameObject upgradesMenu;
    public static GameObject customizeMenu;
    public static GameObject settingsMenu;

    public static Text mapLevelText;
    public static Text qualityText;
    public static Text fpsText;

    public static GameObject pauseButton;

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
        fpsText.text = UserData.fps.ToString();
    }

    private static void RefreshQualityText()
    {
        switch (UserData.quality)
        {
            case 3:
                qualityText.text = "low";
                break;
            case 5:
                qualityText.text = "medium";
                break;
            case 10:
                qualityText.text = "high";
                break;
        }
    }
}
