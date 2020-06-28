using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rand
{
    private static int seed;

    // To calculate random seed
    private static int sign = -1;
    private static int number;  // least 0, max 10

    public static void InitState(int _seed)
    {
        Random.InitState(_seed);
        seed = _seed;
    }

    public static float Range(float min, float max)
    {
        float result = Random.Range(min, max);
        sign *= -1;
        number = (number + 1) % 10 * sign;
        InitState(seed + 10);

        return result;
    }

    public static int Range(int min, int max)
    {
        int result = Random.Range(min, max);
        sign *= -1;
        number = (number + 1) % 10 * sign;
        InitState(seed + 10 * number);

        return result;
    }
}
