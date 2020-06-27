using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    // --- SAVE FILE STUFF --- //
    public static int mapLevel;
    public static int level;

    // --- NON-SAVE FILE STUFF --- //
    public static bool isAlive = false;
    public static bool isQuitting = false;

    // --- SETTINGS --- //
    public static int fps;
}

[System.Serializable]
public class SaveData
{
    // --- SAVE FILE STUFF --- //
    public int mapLevel;
    public int level;

    // --- SETTINGS --- //
    public int fps;
}
