using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public List<Upgrade> upgradesDataBase = new();

    private Clicker clicker;
    private TextManager tm;

    public int DamageLvl;
    public int CritLvl;

    void Start()
    {
        clicker = GetComponent<Clicker>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();
    }

    public void DamageUpgrade()
    {
        DamageLvl++;

        clicker.CalculateDmg();
        tm.CurrencyTextUpdate();
    }
    public void CritUpgrade()
    {
        CritLvl++;

        clicker.CalculateCrit();
        tm.CurrencyTextUpdate();
    }

    #region Upgrades
    public int critDamageLvl; public void CritDamageUpgrade()
    { critDamageLvl++; }
    public int dropRateLvl; public void DropRateUpgrade()
    { dropRateLvl++; }
    public int packsCountLvl; public void PacksCountUpgrade()
    { packsCountLvl++; }
    public int currencyChanceLvl; public void CurrencyChanceUpgrade()
    { currencyChanceLvl++; }
    public int doubleCurrencyLvl; public void DoubleCurrencyUpgrade()
    { doubleCurrencyLvl++; }
    public int betterPacksLvl; public void BetterPacksUpgrade()
    { betterPacksLvl++; }
    public int doubleDamageLvl; public void DoubleDamageUpgrade()
    { doubleDamageLvl++; }
    public int betterStartLvl; public void BetterStartUpgrade()
    { betterStartLvl++; }
    public int moreBirthChanceLvl; public void MoreBirthChanceUpgrade()
    { moreBirthChanceLvl++; }
    public int betterMineAfterRebirthLvl; public void BetterMineAfterRebirthUpgrade()
    { betterMineAfterRebirthLvl++; }
    public int doubleXPLvl; public void DoubleXPUpgrade()
    { doubleXPLvl++; }
    public int mineLootLvl; public void MineLootUpgrade()
    { mineLootLvl++; }
    #endregion
}

[System.Serializable]

public class Upgrade
{
    public Sprite upgradeIco;

    public float lvlPrice;
    public UpgradeTypes type;
    public int maxLvl;
    public float stepCoef;
    public string upgradeName;
    public string upgradeDescription;
}
public enum UpgradeTypes
{
    Money, Xp, Birth
}
