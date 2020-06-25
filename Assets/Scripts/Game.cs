using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject player;

    public Text mapLevelText;

    void Start()
    {
        UserData userData = SaveSystem.ReadFile();
        Application.targetFrameRate = UserData.fps;
        Debug.Log(Application.targetFrameRate);
        Debug.Log(UserData.fps);

        UpdateUI();

        //Instantiate(player);    // TODO: Instantiate when pressed play
    }

    void Update()
    {
        // If player is dead, restart the game when touching to the screen
        if (!UserData.isAlive)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) && !GameObject.Find("Obstacle(Clone)"))
            {
                Instantiate(player);
                UserData.isAlive = true;
            }
        }
    }

    void UpdateUI()
    {
        mapLevelText.text = UserData.mapLevel.ToString();
    }
}
