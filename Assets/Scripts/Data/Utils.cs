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
        if (double.IsNaN(num) || double.IsInfinity(num))
        {
            num = double.MaxValue / 100;
        }
        else
        {
            num = (num > double.MaxValue / 100) ? double.MaxValue / 100 : num;

            for (int i = 0; i < times; i++)
            {
                num *= multiplier;

                if (double.IsNaN(num) || double.IsInfinity(num))
                {
                    num = double.MaxValue / 100;
                    break; // Выход из цикла при обнаружении NaN или Infinity.
                }
                else
                {
                    num = (num > double.MaxValue / 100) ? double.MaxValue / 100 : num;
                }
            }
        }

        return num;
    }
}
