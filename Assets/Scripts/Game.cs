using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player;

    private UserData userData;

    void Start()
    {
        userData = SaveSystem.ReadFile();
        Application.targetFrameRate = 60;   // TODO: Settings with settings.bin

        Instantiate(player);    // TODO: Instantiate when pressed play
    }

    void Update()
    {
        // If player is dead, restart the game when touching to the screen
        if (!UserData.isAlive)
        {
            if (Input.touchCount > 0 && !GameObject.Find("Obstacle(Clone)"))
            {
                Instantiate(player);
                UserData.isAlive = true;
            }
        }
    }
}
