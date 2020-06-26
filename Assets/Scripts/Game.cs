using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game : MonoBehaviour
{
    public GameObject player;

    public Button playButton;
    public Text mapLevelText;

    private SaveData saveData;

    private float durationLimit;
    private float timer;

    void Start()
    {
        // Read "user.save" & write into "SaveData" class
        saveData = SaveSystem.ReadFile();

        // --- SETUP USERDATA --- //
        UserData.mapLevel = saveData.mapLevel;
        UserData.level = saveData.level;
        UserData.fps = saveData.fps;

        // --- SETUP SETTINGS --- //
        Application.targetFrameRate = UserData.fps;

        // --- SETUP UI SYSTEM --- //
        UI.playButton = playButton;
        UI.mapLevelText = mapLevelText;

        // Update UI with data from save file
        UI.Update();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > durationLimit && durationLimit != 0f)
        {
            UI.playButton.gameObject.SetActive(false);
            Play();
            durationLimit = 0f;
        }
    }

    private void Play()
    {
        if (!UserData.isAlive)
        {
            ObstacleManager.SetupMapLevel();
            UserData.isAlive = true;
            Instantiate(player);
        }
    }

    public void PlayButtonClick(float duration)
    {
        timer = 0f;
        durationLimit = duration;
    }

    void OnApplicationQuit()
    {
        SaveSystem.WriteFile(saveData);
    }
}
