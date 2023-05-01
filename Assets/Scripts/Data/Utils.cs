using System;
using System.Globalization;
using UnityEngine;

public static class EntranceTimeUtils
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
            DateTime result = DateTime.ParseExact(stored,"u", CultureInfo.InvariantCulture);

            return result;
        }
        else
        {
            return defaultValue;
        }
    }
}
