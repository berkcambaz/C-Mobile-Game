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

    private float durationLimit;
    private float timer;

    void Start()
    {
        UserData userData = SaveSystem.ReadFile();
        Application.targetFrameRate = UserData.fps;

        UI.playButton = playButton;

        UpdateUI();
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

        // If player is dead, restart the game when touching to the screen
        //if (!UserData.isAlive)
        //{
        //    if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) && !GameObject.Find("Obstacle(Clone)"))
        //    {
        //        Instantiate(player);
        //        UserData.isAlive = true;
        //    }
        //}
    }

    void UpdateUI()
    {
        mapLevelText.text = UserData.mapLevel.ToString();
    }

    private void Play()
    {
        if (!UserData.isAlive)
        {
            ObstacleManager.SetupMapLevel();
            Instantiate(player);
            UserData.isAlive = true;
        }
    }

    public void PlayButtonClick(float duration)
    {
        timer = 0f;
        durationLimit = duration;
    }
}
