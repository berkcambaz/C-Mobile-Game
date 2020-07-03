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
        UI.levelContent.SetActive(false);

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
        UserData.quality = !UserData.quality;

        UI.RefreshSettings();
    }

    public static void QualityDecreaseButton()
    {
        UserData.quality = !UserData.quality;

        UI.RefreshSettings();
    }

    public static void ParticleIncreaseButton()
    {
        switch (UserData.particles)
        {
            case 10:
                UserData.particles = 3;
                break;
            case 5:
                UserData.particles = 10;
                break;
            case 3:
                UserData.particles = 5;
                break;
        }

        UI.RefreshSettings();
    }

    public static void ParticleDecreaseButton()
    {
        switch (UserData.particles)
        {
            case 10:
                UserData.particles = 5;
                break;
            case 5:
                UserData.particles = 3;
                break;
            case 3:
                UserData.particles = 10;
                break;
        }

        UI.RefreshSettings();
    }

    public static void FpsIncreaseButton()
    {
        if (UserData.fps == 30)
            UserData.fps = Screen.currentResolution.refreshRate;
        else if (UserData.fps == Screen.currentResolution.refreshRate)
            UserData.fps = 30;

        UI.RefreshSettings();
        Application.targetFrameRate = UserData.fps;
    }

    public static void FpsDecreaseButton()
    {
        if (UserData.fps == 30)
            UserData.fps = Screen.currentResolution.refreshRate;
        else if (UserData.fps == Screen.currentResolution.refreshRate)
            UserData.fps = 30;

        UI.RefreshSettings();
        Application.targetFrameRate = UserData.fps;
    }

    public static void BackToMenuButton()
    {
        UI.hud.SetActive(true);
        UI.mainMenu.SetActive(true);

        UI.goBackButton.SetActive(false);

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
        UI.mainMenu.SetActive(false);

        UI.goBackButton.SetActive(true);

        UI.upgradesMenu.SetActive(true);
    }

    public static void CustomizeButton()
    {
        UI.mainMenu.SetActive(false);

        UI.goBackButton.SetActive(true);

        UI.customizeMenu.SetActive(true);
    }
}
