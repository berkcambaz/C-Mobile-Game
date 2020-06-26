using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI
{
    public static Button playButton;
    public static Text mapLevelText;

    public static void Update()
    {
        mapLevelText.text = UserData.mapLevel.ToString();
    }
}
