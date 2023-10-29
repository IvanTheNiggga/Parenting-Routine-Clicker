using UnityEngine;

public class NumFormat : MonoBehaviour
{
    private static string[] names = new[]
    {
        "",
        "K",
        "M",
        "B",
        "T",
        "q",
        "Q",
        "s",
        "S",
        "O",
        "N",
        "D",
        "ab",
        "ac",
        "ad",
        "ae",
        "af",
        "ah",
        "ai",
        "aj",
        "ak",
        "al",
        "am",
        "an",
        "ao",
        "ap",
        "aq",
        "ar",
        "as",
        "at",
        "au",
        "av",
        "aw",
        "ax",
        "ay",
        "az",
        "ba",
        "bb",
        "bc",
        "bd",
        "be",
        "bf",
        "bh",
        "bi",
        "bj",
        "bk",
        "bl",
        "bm",
        "bn",
        "bo",
        "bp",
        "bq",
        "br",
        "bs",
        "bt",
        "bu",
        "bv",
        "bw",
        "bx",
        "by",
        "bz",
        "ca",
        "cb",
        "cc",
        "cd",
        "ce",
        "cf",
        "ch",
        "ci",
        "cj",
        "ck",
        "cl",
        "cm",
        "cn",
        "co",
        "cp",
        "cq",
        "cr",
        "cs",
        "ct",
        "cu",
        "cv",
        "cw",
        "cx",
        "cy",
        "cz",
        "da",
        "db",
        "dc",
        "dd",
        "de",
        "df",
        "dh",
        "di",
        "dj",
        "dk",
        "dl",
        "dm",
        "dn",
        "do",
        "dp",
        "dq",
        "dr",
        "ds",
        "dt",
        "du",
        "dv",
        "dw",
    };
    public static string FormatNumF0(double num)
    {
        if (num == 0) return "0";

        int i = 0;
        while (i + 1 < names.Length && num >= 1000)
        {
            num /= 1000;
            i++;
        }
        return num.ToString(format: "F0") + names[i];
    }
    public static string FormatNumF0F1(double num)
    {
        if (num < 1000)
        {
            if (num == 0) return "0";

            int i = 0;

            while (i + 1 < names.Length && num >= 1000)
            {
                num /= 1000;
                i++;
            }
            return num.ToString(format: "F0") + names[i];
        }
        else
        {
            if (num == 0) return "0";

            int i = 0;

            while (i + 1 < names.Length && num >= 1000)
            {
                num /= 1000;
                i++;
            }
            return num.ToString(format: "F1") + names[i];
        }

    }
    public static string FormatNumF1(double num)
    {
        if (num == 0) return "0";

        int i = 0;

        while (i + 1 < names.Length && num >= 1000)
        {
            num /= 1000;
            i++;
        }
        return num.ToString(format: "F1") + names[i];
    }
}

