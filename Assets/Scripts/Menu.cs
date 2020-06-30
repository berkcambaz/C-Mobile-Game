using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Menu
{
    public static void PlayButton()
    {
        UI.mainMenu.SetActive(false);
        UI.pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public static void SettingsButton()
    {
        UI.mainMenu.SetActive(false);
        UI.settingsMenu.SetActive(true);
    }

    public static void QualityIncreaseButton()
    {
        switch (UserData.quality)
        {
            case 10:
                UserData.quality = 3;
                break;
            case 5:
                UserData.quality = 10;
                break;
            case 3:
                UserData.quality = 5;
                break;
        }

        UI.RefreshSettings();
    }

    public static void QualityDecreaseButton()
    {
        switch (UserData.quality)
        {
            case 10:
                UserData.quality = 5;
                break;
            case 5:
                UserData.quality = 3;
                break;
            case 3:
                UserData.quality = 10;
                break;
        }

        UI.RefreshSettings();
    }

    public static void FpsIncreaseButton()
    {
        switch (UserData.fps)
        {
            case 15:
                UserData.fps = 30;
                break;
            case 30:
                UserData.fps = 60;
                break;
            case 60:
                UserData.fps = 90;
                break;
            case 90:
                UserData.fps = 120;
                break;
            case 120:
                UserData.fps = 15;
                break;
        }

        UI.RefreshSettings();
        Application.targetFrameRate = UserData.fps;
    }

    public static void FpsDecreaseButton()
    {
        switch (UserData.fps)
        {
            case 15:
                UserData.fps = 120;
                break;
            case 30:
                UserData.fps = 15;
                break;
            case 60:
                UserData.fps = 30;
                break;
            case 90:
                UserData.fps = 60;
                break;
            case 120:
                UserData.fps = 90;
                break;
        }

        UI.RefreshSettings();
        Application.targetFrameRate = UserData.fps;
    }

    public static void BackToMenuButton()
    {
        UI.hud.SetActive(true);
        UI.mainMenu.SetActive(true);

        UI.upgradesMenu.SetActive(false);
        UI.customizeMenu.SetActive(false);
        UI.settingsMenu.SetActive(false);
    }

    public static void PauseButton()
    {
        UI.pauseButton.SetActive(false);
        Time.timeScale = 0f;
        UserData.isPlaying = false;

        UI.upgradesButton.SetActive(false);
        UI.customizeButton.SetActive(false);

        UI.mainMenu.SetActive(true);
    }

    public static void UpgradesButton()
    {
        UI.hud.SetActive(false);
        UI.mainMenu.SetActive(false);

        UI.upgradesMenu.SetActive(true);
    }

    public static void CustomizeButton()
    {
        UI.hud.SetActive(false);
        UI.mainMenu.SetActive(false);

        UI.customizeMenu.SetActive(true);
    }
}
