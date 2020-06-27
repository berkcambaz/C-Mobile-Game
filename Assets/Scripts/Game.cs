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

    private SaveData saveData;

    private float durationLimit = -1f;
    private float timer;
    private int buttonIndex;

    void Start()
    {
        // Read "user.save" & write into "SaveData" class
        saveData = SaveSystem.ReadFile();

        // --- SETUP USERDATA --- //
        UserData.mapLevel = saveData.mapLevel;
        UserData.level = saveData.level;
        UserData.quality = saveData.quality;
        UserData.fps = saveData.fps;

        // --- SETUP SETTINGS --- //
        Application.targetFrameRate = (UserData.fps) ? 60 : 30;

        // --- SETUP UI SYSTEM --- //
        UI.hud = hud;
        UI.mainMenu = mainMenu;
        UI.settingsMenu = settingsMenu;
        UI.mapLevelText = mapLevelText;

        UI.qualityText = qualityText;
        UI.fpsText = fpsText;

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
        timer = 0f;
        durationLimit = 0f;

        buttonIndex = index;
        switch (index)
        {
            case 0:
            case 1:
            case 4:
                durationLimit = 0.35f;
                break;

        }
    }

    private void ButtonEvent()
    {
        switch (buttonIndex)
        {
            case 0: // Play button 
                UI.mainMenu.SetActive(false);

                Play();
                break;
            case 1: // Settings button
                UI.mainMenu.SetActive(false);
                UI.settingsMenu.SetActive(true);

                GoToSettings();
                break;
            case 2: // Quality increase&decrease button
                UserData.quality = !UserData.quality;
                // TODO: Set quality

                UI.RefreshSettings();
                break;
            case 3: // Fps increase&decrease button
                UserData.fps = !UserData.fps;
                Application.targetFrameRate = (UserData.fps) ? 60 : 30;

                UI.RefreshSettings();
                break;
            case 4: // Back to "main menu" button
                UI.mainMenu.SetActive(true);
                UI.settingsMenu.SetActive(false);
                break;
        }
    }

    private void Play()
    {
        if (!UserData.isAlive)
        {
            ObstacleManager.SetupMapLevel();
            UserData.isPlaying = true;
            UserData.isAlive = true;
            Instantiate(player);
        }
    }

    private void GoToSettings()
    {

    }

    void OnApplicationQuit()
    {
        UserData.isQuitting = true;

        // --- UPDATE SAVEDATA --- //
        saveData.mapLevel = UserData.mapLevel;
        saveData.level = UserData.level;
        saveData.fps = UserData.fps;

        SaveSystem.WriteFile(saveData);
    }
}
