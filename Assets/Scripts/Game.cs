using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game : MonoBehaviour
{
    public GameObject player;

    // --- UI STUFF --- //
    public GameObject hud;
    public GameObject mainMenu;
    public GameObject settingsMenu;

    public Text mapLevelText;
    public Text qualityText;
    public Text fpsText;

    public GameObject pauseButton;

    private SaveData saveData;

    private float durationLimit = -1f;
    private float timer;
    private int buttonIndex;

    void Start()
    {

        // Read "user.save" & write into "SaveData" class
        saveData = SaveSystem.ReadFile();
        if (!saveData.Checksum())   // If true, user has cheated
        {
            saveData.mapLevel = 0;
            saveData.level = 0;
        }
        if (saveData.fps == 0)  // If first time opening the game, set fps to phone's refresh rate
            saveData.fps = Screen.currentResolution.refreshRate;

        // --- SETUP USERDATA --- //
        UserData.mapLevel = saveData.mapLevel;
        UserData.level = saveData.level;
        UserData.quality = saveData.quality;
        UserData.fps = saveData.fps;

        // --- SETUP SETTINGS --- //
        QualitySettings.vSyncCount = -1;
        Application.targetFrameRate = UserData.fps;

        // --- SETUP UI SYSTEM --- //
        UI.hud = hud;
        UI.mainMenu = mainMenu;
        UI.settingsMenu = settingsMenu;
        UI.mapLevelText = mapLevelText;

        UI.qualityText = qualityText;
        UI.fpsText = fpsText;

        UI.pauseButton = pauseButton;

        // Init UI with data from save file
        UI.Init();

        // Update UI with data from save file
        UI.Update();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > durationLimit && durationLimit >= 0f)
        {
            ButtonEvent();
            durationLimit = -1f;
        }
    }

    public void ButtonClick(int index)
    {
        timer = 0.001f;
        durationLimit = 0f;

        buttonIndex = index;
        switch (index)
        {
            case 0:
            case 1:
            case 6:
                durationLimit = 0.35f * Time.timeScale;
                break;
        }
    }

    private void ButtonEvent()
    {
        switch (buttonIndex)
        {
            case 0: // Play button 
                UI.mainMenu.SetActive(false);
                UI.pauseButton.SetActive(true);
                Time.timeScale = 1f;

                Play();
                break;
            case 1: // Settings button
                UI.mainMenu.SetActive(false);
                UI.settingsMenu.SetActive(true);
                break;
            case 2: // Quality increase button
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
                break;
            case 3: // Quality decrease button
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
                break;
            case 4: // Fps increase button
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

                UI.RefreshSettings();    // Refresh first, otherwise game freezes for a second
                Application.targetFrameRate = UserData.fps;
                break;
            case 5: // Fps decrease button
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

                UI.RefreshSettings();   // Refresh first, otherwise game freezes for a second
                Application.targetFrameRate = UserData.fps;
                break;
            case 6: // Back to "main menu" button
                UI.mainMenu.SetActive(true);
                UI.settingsMenu.SetActive(false);
                break;
            case 7: // Pause button
                UI.pauseButton.SetActive(false);
                Time.timeScale = 0f;
                UserData.isPlaying = false;

                UI.mainMenu.SetActive(true);
                UI.settingsMenu.SetActive(false);
                break;
        }
    }

    private void Play()
    {
        UserData.isPlaying = true;

        if (!UserData.isAlive)
        {
            ObstacleManager.SetupMapLevel();
            UserData.isAlive = true;
            Instantiate(player);
        }
    }

    void OnApplicationQuit()
    {
        UserData.isQuitting = true;

        // --- UPDATE SAVEDATA --- //
        saveData.mapLevel = UserData.mapLevel;
        saveData.level = UserData.level;
        saveData.quality = UserData.quality;
        saveData.fps = UserData.fps;

        // Write checksum
        saveData.Checksum();

        SaveSystem.WriteFile(saveData);
    }
}
