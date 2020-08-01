using UnityEngine;

public class UserData
{
    // --- SAVE FILE STUFF --- //
    public static int mapLevel;
    public static int score;
    public static int highScore;
    public static int money;
    public static int moneyToSpawn;

    public static bool[] skins = new bool[64];
    public static int[] upgrades = new int[64];

    public static int unlockedSkinCount;

    public static int playerSkinIndex;

    // --- NON-SAVE FILE STUFF --- //
    public static bool isPlaying = false;
    public static bool isAlive = false;
    public static bool isQuitting = false;

    public static bool isObstacleSetFinished = true;

    public static bool isUnlockingCharacter = false;

    public static bool isUndead = false;

    public static GameObject[] customizables;
    public static Sprite[] skinSprites;

    public static int selectedSkinIndex = 0;
    public static int lastOpenedSkin;
    public static int potionNum;

    // --- SETTINGS --- //
    public static bool quality;     // Quality of the game, false - low, true - high
    public static int particles;    // Number of particles, 10 - high, 5 - medium, 3 - low
    public static int fps;
}

[System.Serializable]
public class SaveData
{
    // --- SAVE FILE STUFF --- //
    public int mapLevel = 1;
    public int score;
    public int highScore;
    public int money = 25;
    public  int moneyToSpawn = 10;

    public bool[] skins = new bool[64];
    public int[] upgrades = new int[64];

    public int openedSkinCount = 1;

    public int playerSkinIndex = 0;

    // --- SETTINGS --- //
    public bool quality = false; // Quality of the game, false - low, true - high
    public int particles = 10;  // Number of particles, 10 - high, 5 - medium, 3 - low
    public int fps;

    // --- CHECKSUM --- //
    public int checksum;

    public bool Checksum()
    {
        bool isEqual = false;

        // Calculate sum
        int sum = mapLevel + score + highScore + money + playerSkinIndex;
        for (int i = 0; i < 64; ++i)
            sum += (skins[i] ? 1 : 0) + upgrades[i];

        // Check sum
        if (checksum == sum)
            isEqual = true;

        checksum = sum;

        return isEqual;
    }
}
