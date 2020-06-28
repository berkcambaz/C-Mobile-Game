using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game : MonoBehaviour
{
    public GameObject player;

    // --- UI STUFF --- //
    public GameObject hud;
    public GameObject mainMenu;
    public GameObject upgradesMenu;
    public GameObject customizeMenu;
    public GameObject settingsMenu;

    public Text mapLevelText;
    public Text qualityText;
    public Text fpsText;

    public GameObject pauseButton;

    private SaveData saveData;

    private float durationLimit = -1f;
    private float timer;

    private int selectedIndex = -1;

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
        UI.upgradesMenu = upgradesMenu;
        UI.customizeMenu = customizeMenu;
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

        selectedIndex = index;
        switch (index)
        {
            case 0:
            case 1:
            case 6:
            case 8:
            case 9:
                durationLimit = 0.35f * Time.timeScale;
                break;
        }
    }

    private void ButtonEvent()
    {
        switch (selectedIndex)
        {
            case 0:
                Menu.PlayButton();
                Play();
                break;
            case 1:
                Menu.SettingsButton();
                break;
            case 2:
                Menu.QualityIncreaseButton();
                break;
            case 3:
                Menu.QualityDecreaseButton();
                break;
            case 4:
                Menu.FpsIncreaseButton();
                break;
            case 5:
                Menu.FpsDecreaseButton();
                break;
            case 6:
                Menu.BackToMenuButton();
                break;
            case 7:
                Menu.PauseButton();
                break;
            case 8:
                Menu.UpgradesButton();
                break;
            case 9:
                Menu.CustomizeButton();
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
