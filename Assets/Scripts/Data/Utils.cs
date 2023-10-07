using System;
using System.Globalization;
using UnityEngine;

public static class Utils
{
    public static void SetDataTime(string key, DateTime value)
    {
        string convertedToString = value.ToString("u", CultureInfo.InvariantCulture);
        PlayerPrefs.SetString(key, convertedToString);
    }

    public static DateTime GetDataTime(string key, DateTime defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string stored = PlayerPrefs.GetString(key);
            DateTime result = DateTime.ParseExact(stored, "u", CultureInfo.InvariantCulture);

            return result;
        }
        else
        {
            return defaultValue;
        }
    }

    public static double Progression(double num, float multiplier, int times)
    {
        for (int i = 0; times > i; i++)
        {
            num *= multiplier;
        }
        return num;
    }
}
