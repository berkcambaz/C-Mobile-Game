using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = SaveSystem.ReadFile();
        Application.targetFrameRate = 60;   // TODO: Settings with settings.bin
    }

    void Update()
    {

    }
}
