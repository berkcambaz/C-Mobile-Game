using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rand
{
    private static int seed;

    // To calculate random seed
    private static int sign;
    private static int number;  // least 0, max 10


    public static void Init()
    {
        sign = -1;
        number = 0;
    }

    public static void InitState(int _seed)
    {
        Random.InitState(_seed);
        seed = _seed;
    }

    public static int[] GetState()
    {
        return new int[3] { seed, sign, number };
    }

    public static void SetState(int[] state){
        seed = state[0];
        sign = state[1];
        number = state[2];
    }

    public static float Range(float min, float max)
    {
        float result = Random.Range(min, max);
        sign *= -1;
        number = (number + 1) % 10 * sign;
        InitState(seed + 10 * number);

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
