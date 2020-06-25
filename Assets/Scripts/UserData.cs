using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public static int mapLevel;
    public static int level;

    [System.NonSerialized]
    public static bool isAlive = true;
}
