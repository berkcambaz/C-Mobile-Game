using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add user settings to this class
[System.Serializable]
public class UserData
{
    public static int mapLevel;
    public static int level;

    [System.NonSerialized]
    public static bool isAlive = false;

    // --- SETTINGS --- //
    public static int fps = 60;
}
