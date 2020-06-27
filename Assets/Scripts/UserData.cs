using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    // --- SAVE FILE STUFF --- //
    public static int mapLevel;
    public static int level;

    // --- NON-SAVE FILE STUFF --- //
    public static bool isPlaying = false;
    public static bool isAlive = false;
    public static bool isQuitting = false;

    // --- SETTINGS --- //
    public static bool quality; // "bool", because there is only "high" & "low" setting, false = low, true = high
    public static bool fps;     // "bool", because there is only "60" & "30" setting, false = 30, true = 60
}

[System.Serializable]
public class SaveData
{
    // --- SAVE FILE STUFF --- //
    public int mapLevel;
    public int level;

    // --- SETTINGS --- //
    public bool quality = true; // "bool", because there is only "high" & "low" setting, false = low, true = high
    public bool fps = true;     // "bool", because there is only "60" & "30" setting, false = 30, true = 60

    // --- CHECKSUM --- //
    public int checksum;

    public bool Checksum()
    {
        bool isEqual = false;

        int sum = mapLevel + level;

        if (checksum == sum)
            isEqual = true;

        checksum = sum;

        return isEqual;
    }
}
