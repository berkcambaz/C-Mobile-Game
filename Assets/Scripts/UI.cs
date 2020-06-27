using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI
{
    public static GameObject hud;
    public static GameObject mainMenu;
    public static GameObject settingsMenu;

    public static Text mapLevelText;
    public static Text qualityText;
    public static Text fpsText;

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
        qualityText.text = (UserData.quality) ? "high" : "low";
        fpsText.text = (UserData.fps) ? "60" : "30";
    }
}
