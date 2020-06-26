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

    // --- SETTINGS --- //
    public static int fps = 60;
}

[System.Serializable]
public class SaveData
{
    // --- SAVE FILE STUFF --- //
    public int mapLevel;
    public int level;

    // --- SETTINGS --- //
    public int fps = 60;
}
