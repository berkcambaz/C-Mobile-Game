using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    // --- SAVE FILE STUFF --- //
    public static int mapLevel;
    public static int level;

    // --- NON-SAVE FILE STUFF --- //
    [System.NonSerialized]
    public static bool isAlive = false;

    // --- SETTINGS --- //
    public static int fps = 60;
}
