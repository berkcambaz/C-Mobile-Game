﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    // --- SAVE FILE STUFF --- //
    public static int mapLevel;
    public static int level;

    public static bool[] skins = new bool[4];
    public static int[] upgrades;

    public static int playerSkinIndex = 0;

    // --- NON-SAVE FILE STUFF --- //
    public static bool isPlaying = false;
    public static bool isAlive = false;
    public static bool isQuitting = false;

    public static Sprite[] skinSprites = new Sprite[4];
    public static int selectedSkinIndex;

    // --- SETTINGS --- //
    public static int quality; // Number of particles, 10 - high, 5 - medium, 3 - low
    public static int fps;
}

[System.Serializable]
public class SaveData
{
    // --- SAVE FILE STUFF --- //
    public int mapLevel;
    public int level;

    public bool[] skins = new bool[4];
    public int[] upgrades;

    public int playerSkinIndex;

    // --- SETTINGS --- //
    public int quality = 10; // Number of particles, 10 - high, 5 - medium, 3 - low
    public int fps;

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
