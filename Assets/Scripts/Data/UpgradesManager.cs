using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    #region Appointed on start
    private Clicker clicker;
    private InterfaceManager interfaceManager;
    #endregion

    #region Variables
    public List<Upgrade> UpgradesDataBase = new();
    public int DamageLvl;
    public int CritLvl;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        clicker = GetComponent<Clicker>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
    }
    #endregion

    #region Upgrades
    public void DamageUpgrade()
    {
        DamageLvl++;

        clicker.CalculateDmg();
        interfaceManager.CurrencyTextUpdate();
    }
    public void CritUpgrade()
    {
        CritLvl++;

        clicker.CalculateCrit();
        interfaceManager.CurrencyTextUpdate();
    }
    public int CritDamageLvl; public void CritDamageUpgrade()
    { CritDamageLvl++; }
    public int DropRateLvl; public void DropRateUpgrade()
    { DropRateLvl++; }
    public int PacksCountLvl; public void PacksCountUpgrade()
    { PacksCountLvl++; }
    public int CurrencyChanceLvl; public void CurrencyChanceUpgrade()
    { CurrencyChanceLvl++; }
    public int DoubleCurrencyLvl; public void DoubleCurrencyUpgrade()
    { DoubleCurrencyLvl++; }
    public int BetterPacksLvl; public void BetterPacksUpgrade()
    { BetterPacksLvl++; }
    public int DoubleDamageLvl; public void DoubleDamageUpgrade()
    { DoubleDamageLvl++; }
    public int BetterStartLvl; public void BetterStartUpgrade()
    { BetterStartLvl++; }
    public int MoreRebirthPointsChanceLvl; public void MoreRebirthPointsChanceUpgrade()
    { MoreRebirthPointsChanceLvl++; }
    public int BetterMineAfterRebirthLvl; public void BetterMineAfterRebirthUpgrade()
    { BetterMineAfterRebirthLvl++; }
    public int DoubleXPLvl; public void DoubleXPUpgrade()
    { DoubleXPLvl++; }
    public int MineLootLvl; public void MineLootUpgrade()
    { MineLootLvl++; }
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
    Money, Xp, RebirthPoints
}
