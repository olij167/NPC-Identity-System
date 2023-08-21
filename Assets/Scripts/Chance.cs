using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chance
{
    public static bool OddsOn(float maxOdds)
    {
        float r = Random.Range(0, maxOdds);

        if (r == 0) return true;
        else return false;
    }

    public static bool CoinFlip()
    {
        float r = Random.Range(0, 2);

        if (r == 0) return true;
        else return false;
    }

    public static float NumberPicker(float outOf)
    {
        return Random.Range(0, outOf);
    }

    public static bool Percentage(float chancePercentage)
    {
        Mathf.Clamp(chancePercentage, 0f, 100f);

        if (Random.Range(0f, 100f) <= chancePercentage)
        {
            return true;
        }
        else return false;
    }
}
