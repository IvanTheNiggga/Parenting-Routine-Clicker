using System;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clicker : MonoBehaviour
{
    #region Local
    private Miner miner;
    private RewardManager rewardManager;
    private EnemyManager enemyManager;
    private Inventory inventory;
    private UnitManager unitManager;
    private Settings optionsMenu;
    private StagesManager stagesManager;
    private TextManager tm;
    private UpgradesManager upgrades;

    public GameObject timerObject;
    private GameObject upgradesGrid;
    #endregion

    #region Variables
    public int Births;
    public float Experience;
    public int MinerLvl;
    public int MaxMinerLvl;
    public int CritMultiplier;

    private double currDealedDamage; public double CurrDealedDamage
    {
        get { return currDealedDamage; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                currDealedDamage = double.MaxValue / 100;
            }
            else
            {
                currDealedDamage = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }
    private double dmgCost; public double DmgCost
    {
        get { return dmgCost; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                dmgCost = double.MaxValue / 100;
            }
            else
            {
                dmgCost = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }
    private double critCost; public double CritCost
    {
        get { return critCost; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                critCost = double.MaxValue / 100;
            }
            else
            {
                critCost = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }
    private double currency; public double Currency
    {
        get { return currency; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                currency = double.MaxValue / 100;
            }
            else
            {
                currency = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }

    private double damage; public double Damage
    {
        get { return damage; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                damage = double.MaxValue / 100;
            }
            else
            {
                damage = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }

    private int critChance; public int CritChance
    {
        get { return critChance; }
        set
        {
            critChance = (value < 0) ? 0 : (value > 100) ? 100 : value;
        }
    }

    private float afterSaveSeconds; public float AfterSaveSeconds
    {
        get { return afterSaveSeconds; }
        set
        {
            if (value > 60)
            { afterSaveSeconds = 0; Save(); return; }
            afterSaveSeconds = value;
        }
    }
    #endregion

    #region Per-Frame Methods
    private void Start()
    {
        miner = GameObject.Find("Miner").GetComponent<Miner>();
        rewardManager = GetComponent<RewardManager>();
        enemyManager = GetComponent<EnemyManager>();
        inventory = GetComponent<Inventory>();
        unitManager = GetComponent<UnitManager>();
        optionsMenu = GameObject.Find("Settings").GetComponent<Settings>();
        stagesManager = GetComponent<StagesManager>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();
        upgrades = GetComponent<UpgradesManager>();

        upgradesGrid = GameObject.Find("UpgradesGrid");

        if (PlayerPrefs.HasKey("Currency"))
        {
            Load();
        }
        else
        {
            ResetData();
        }
    }

    private void FixedUpdate()
    {
        AfterSaveSeconds += Time.deltaTime;
    }
    #endregion


    #region Data manipalations
    public void Load()
    {
        CritMultiplier = PlayerPrefs.GetInt("CritMultiplier");

        stagesManager.CurrentStage = PlayerPrefs.GetInt("CurrentStage");
        stagesManager.StageIndex = PlayerPrefs.GetInt("StageIndex");
        stagesManager.maxStage = PlayerPrefs.GetInt("maxStage");
        Currency = LoadBig("Currency");
        Experience = PlayerPrefs.GetFloat("Experience");
        Births = PlayerPrefs.GetInt("Births");

        MinerLvl = PlayerPrefs.GetInt("MinerLvl");
        MaxMinerLvl = PlayerPrefs.GetInt("MaxMinerLvl");

        upgrades.DamageLvl = PlayerPrefs.GetInt("DamageLvl");
        upgrades.CritLvl = PlayerPrefs.GetInt("CritLvl");

        moreXPUpgradeLvl = PlayerPrefs.GetInt("doubleXPUpgradeLvl");
        betterMineAfterRebirthUpgradeLvl = PlayerPrefs.GetInt("betterMineAfterRebirthUpgradeLvl");
        moreBirthChanceUpgradeLvl = PlayerPrefs.GetInt("moreBirthChanceUpgradeLvl");
        betterStartUpgradeLvl = PlayerPrefs.GetInt("betterStartUpgradeLvl");
        doubleDamageUpgradeLvl = PlayerPrefs.GetInt("doubleDamageUpgradeLvl");
        critMultiplierUpgradeLvl = PlayerPrefs.GetInt("critMultiplierUpgradeLvl");
        crateDropChanceUpgradeLvl = PlayerPrefs.GetInt("crateDropChanceUpgradeLvl");
        crateMultiplyUpgradeLvl = PlayerPrefs.GetInt("dropMultiplyUpgradeLvl");
        multiplyCurrencyChanceUpgradeLvl = PlayerPrefs.GetInt("multiplyCurrencyChanceUpgradeLvl");
        multiplyCurrencyUpgradeLvl = PlayerPrefs.GetInt("multiplyCurrencyUpgradeLvl");
        betterLootChanceUpgradeLvl = PlayerPrefs.GetInt("betterLootChanceUpgradeLvl");

        CalculateDamages();

        stagesManager.LoadStageData(false);

        inventory.ItemGetData();
    }
    public static double LoadBig(string s)
    {
        string tempPStr = PlayerPrefs.GetString(s);

        double d = Convert.ToDouble(tempPStr, CultureInfo.InvariantCulture);

        return d;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("CritMultiplier", CritMultiplier);
        PlayerPrefs.SetInt("CurrentStage", stagesManager.CurrentStage);
        PlayerPrefs.SetInt("StageIndex", stagesManager.StageIndex);
        PlayerPrefs.SetInt("maxStage", stagesManager.maxStage);
        SaveBig("Currency", Currency);
        PlayerPrefs.SetFloat("Experience", Experience);
        PlayerPrefs.SetInt("Births", Births);

        PlayerPrefs.SetInt("betterMineAfterRebirthUpgradeLvl", betterMineAfterRebirthUpgradeLvl);
        PlayerPrefs.SetInt("MaxMinerLvl", MaxMinerLvl);
        PlayerPrefs.SetInt("MinerLvl", MinerLvl);
        PlayerPrefs.SetInt("DamageLvl", upgrades.DamageLvl);
        PlayerPrefs.SetInt("CritLvl", upgrades.CritLvl);


        PlayerPrefs.SetInt("doubleXPUpgradeLvl", moreXPUpgradeLvl);
        PlayerPrefs.SetInt("moreBirthChanceUpgradeLvl", moreBirthChanceUpgradeLvl);
        PlayerPrefs.SetInt("betterStartUpgradeLvl", betterStartUpgradeLvl);
        PlayerPrefs.SetInt("doubleDamageUpgradeLvl", doubleDamageUpgradeLvl);
        PlayerPrefs.SetInt("critMultiplierUpgradeLvl", critMultiplierUpgradeLvl);
        PlayerPrefs.SetInt("crateDropChanceUpgradeLvl", crateDropChanceUpgradeLvl);
        PlayerPrefs.SetInt("crateMultiplyUpgradeLvl", crateMultiplyUpgradeLvl);
        PlayerPrefs.SetInt("multiplyCurrencyChanceUpgradeLvl", multiplyCurrencyChanceUpgradeLvl);
        PlayerPrefs.SetInt("multiplyCurrencyUpgradeLvl", multiplyCurrencyUpgradeLvl);
        PlayerPrefs.SetInt("betterLootChanceUpgradeLvl", betterLootChanceUpgradeLvl);

        optionsMenu.SaveOptions();
    }
    public void SaveBig(string s, double d)
    {
        string totalPStr = d.ToString("F", CultureInfo.InvariantCulture);

        PlayerPrefs.SetString(s, totalPStr);
    }

    public void StartData()
    {
        unitManager.UnequipUnit(1);
        unitManager.UnequipUnit(2);
        PlayerPrefs.SetInt("Unit1ID", -1);
        PlayerPrefs.SetInt("Unit2ID", -1);

        CritMultiplier = 3;

        stagesManager.CurrentStage = 1;
        stagesManager.maxStage = 1;
        Currency = 0;
        Births = 0;

        miner.ResetMiner();

        enemyManager.EnemyHPMultiplier = 1;

        upgrades.DamageLvl = 1;
        upgrades.CritLvl = 1;

        moreBirthChanceUpgradeLvl = 0;
        betterStartUpgradeLvl = 0;
        doubleDamageUpgradeLvl = 0;
        critMultiplierUpgradeLvl = 0;
        crateDropChanceUpgradeLvl = 0;
        crateMultiplyUpgradeLvl = 0;
        multiplyCurrencyChanceUpgradeLvl = 0;
        multiplyCurrencyUpgradeLvl = 0;
        betterLootChanceUpgradeLvl = 0;

        CalculateDamages();
        stagesManager.LoadStageData(true);

        for (int i = 2; i < upgradesGrid.transform.childCount; i++)
        {
            UpgradeForXp upgradeForXpTemp = upgradesGrid.transform.GetChild(i).GetComponent<UpgradeForXp>();
            upgradeForXpTemp.ResetLvl();
        }

        optionsMenu.SetDefaultCap();
        enemyManager.RespawnEnemy();
        tm.UpdateAllText();
    }
    public void ResetData()
    {
        inventory.DeleteItems();
        PlayerPrefs.DeleteAll();
        StartData();
        Save();
    }
    public void RebirthData()
    {
        inventory.DeleteItems();

        CritMultiplier = 3;

        stagesManager.CurrentStage = 1 + betterStartUpgradeLvl;
        Currency = 0;
        rewardManager.GiveMeReward(5);

        miner.RebirthMiner();

        enemyManager.EnemyHPMultiplier = 1;

        upgrades.DamageLvl = 1;
        upgrades.CritLvl = 1;

        stagesManager.LoadStageData(true);
        tm.UpdateAllText();
    }

    public void Birth()
    {
        int births = Births;

        RebirthData();

        Births = births + 1;
        if (Random.Range(0, 100f / (moreBirthChanceUpgradeLvl * 2.5f)) < 1f)
        {
            unitManager.AddRandomUnit();
        }
        if (Random.Range(0, 100f / (moreBirthChanceUpgradeLvl * 1.25f)) < 1f)
        {
            unitManager.AddRandomUnit();
        }
        unitManager.AddRandomUnit();

        Save();

        CalculateDamages();
        enemyManager.RespawnEnemy();
    }
    #endregion

    #region Calculations
    public void CalculateDamages() { CalculateCrit(); CalculateDmg(); }

    public void CalculateDmg()
    {
        Damage = Utils.Progression(1, 1.6f, upgrades.DamageLvl);
        Damage = Utils.Progression(Damage, 2, doubleDamageUpgradeLvl);
        DmgCost = Utils.Progression(10, 1.6f, upgrades.DamageLvl);

        if (stagesManager.CurrentStage > (Births + 1) * 5)
        {
            int a = stagesManager.CurrentStage - (Births + 1) * 5;
            DmgCost = Utils.Progression(DmgCost, 1.2f, a);
        }
    }
    public void CalculateCrit()
    {
        CritChance = 5 + upgrades.CritLvl - 1;
        CritCost = Utils.Progression(100, 100, upgrades.CritLvl);
    }
    #endregion

    #region Upgrades
    public int critMultiplierUpgradeLvl; public void CritMultiplierUpgrade()
    { critMultiplierUpgradeLvl++; }
    public int crateDropChanceUpgradeLvl; public void CrateDropChanceUpgrade()
    { crateDropChanceUpgradeLvl++; }
    public int crateMultiplyUpgradeLvl; public void CrateMultiplyUpgrade()
    { crateMultiplyUpgradeLvl++; }
    public int multiplyCurrencyChanceUpgradeLvl; public void MultiplyCurrencyChanceUpgrade()
    { multiplyCurrencyChanceUpgradeLvl++; }
    public int multiplyCurrencyUpgradeLvl; public void MultiplyCurrencyUpgrade()
    { multiplyCurrencyUpgradeLvl++; }
    public int betterLootChanceUpgradeLvl; public void BetterLootChanceUpgrade()
    { betterLootChanceUpgradeLvl++; }
    public int doubleDamageUpgradeLvl; public void DoubleDamageUpgrade()
    { doubleDamageUpgradeLvl++; }
    public int betterStartUpgradeLvl; public void BetterStartUpgrade()
    { betterStartUpgradeLvl++; }
    public int moreBirthChanceUpgradeLvl; public void MoreBirthChanceUpgrade()
    { moreBirthChanceUpgradeLvl++; }
    public int betterMineAfterRebirthUpgradeLvl; public void BetterMineAfterRebirthUpgrade()
    { betterMineAfterRebirthUpgradeLvl++; }
    public int moreXPUpgradeLvl; public void DoubleXPUpgrade()
    { moreXPUpgradeLvl++; }
    #endregion

    public void OnApplicationQuit()
    {
        inventory.SetItemsBack();
        if (Currency > 0 || Births >= 0) { Save(); }
    }
}