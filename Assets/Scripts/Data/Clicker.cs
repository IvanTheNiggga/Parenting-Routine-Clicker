using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Clicker : MonoBehaviour
{
    private EnemyManager enemyManager;
    private Inventory inventory;
    private UnitManager unitManager;
    private Settings optionsMenu;
    private StagesManager stagesManager;
    private TextManager tm;
    private UpgradesManager upgrades;

    public GameObject timerObject;
    private GameObject upgradesGrid;

    public int Births;
    public float Experience;
    public int MinerLvl;
    public int CritMultiplier;

    private double currDealedDamage; public double CurrDealedDamage
    {
        get { return currDealedDamage; }
        set
        {
            if (value > double.MaxValue || double.IsNaN(value) == true || double.IsInfinity(value) == true)
            { currDealedDamage = double.MaxValue * 0.9; return; }
            currDealedDamage = value;
        }
    }
    private double dmgCost; public double DmgCost
    {
        get { return dmgCost; }
        set
        {
            if (value > double.MaxValue || double.IsNaN(value) == true || double.IsInfinity(value) == true)
            { dmgCost = double.MaxValue * 0.9; return; }
            dmgCost = value;
        }
    }
    private double critCost; public double CritCost
    {
        get { return critCost; }
        set
        {
            if (value > double.MaxValue || double.IsNaN(value) == true || double.IsInfinity(value) == true)
            { critCost = double.MaxValue * 0.9; return; }
            critCost = value;
        }
    }
    private double currency; public double Currency
    {
        get { return currency; }
        set
        {
            if (value > double.MaxValue || double.IsNaN(value) == true || double.IsInfinity(value) == true)
            { currency = double.MaxValue * 0.9; return; }
            currency = value;
    }   }

    private double damage; public double Damage
    {
        get { return damage; }
        set
        {
            if (value > double.MaxValue || double.IsNaN(value) == true || double.IsInfinity(value) == true)
            { damage = double.MaxValue * 0.9; return; }
            damage = value;
        }
    }

    private int critChance; public int CritChance
    {   get { return critChance; }
        set
        {
            if (value > 100)
            { critChance = 100; return; }
            critChance = value;
    }   }

    private float afterSaveSeconds; public float AfterSaveSeconds
    {
        get { return afterSaveSeconds; }
        set
        {
            if (value > 60)
            { afterSaveSeconds = 0; Save(); return; }
            afterSaveSeconds = value;
    }   }

    private void Start()
    {
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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Data manipalations
    public void Load()
    {
        CritMultiplier = PlayerPrefs.GetInt("CritMultiplier");

        stagesManager.CurrentStage = PlayerPrefs.GetInt("CurrentStage");
        stagesManager.StageIndex = PlayerPrefs.GetInt("StageIndex");
        Currency = LoadBig("Currency");
        Experience = PlayerPrefs.GetFloat("Experience");
        Births = PlayerPrefs.GetInt("Births");

        MinerLvl = PlayerPrefs.GetInt("MinerLvl");

        upgrades.DamageLvl = PlayerPrefs.GetInt("DamageLvl");
        upgrades.CritLvl = PlayerPrefs.GetInt("CritLvl");

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
        SaveBig("Currency", Currency);
        PlayerPrefs.SetFloat("Experience", Experience);
        PlayerPrefs.SetInt("Births", Births);

        PlayerPrefs.SetInt("MinerLvl", MinerLvl);
        PlayerPrefs.SetInt("DamageLvl", upgrades.DamageLvl);
        PlayerPrefs.SetInt("CritLvl", upgrades.CritLvl);


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
        Currency = 0;
        Births = 0;
        Experience = 0;

        MinerLvl = 0;

        enemyManager.EnemyHPMultiplier = 1;

        upgrades.DamageLvl = 1;
        upgrades.CritLvl = 1;

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

    public void Birth()
    {
        int births = Births;

        ResetData();

        Births = births + 1;
        unitManager.AddRandomUnit();

        for (int i = 2; i < upgradesGrid.transform.childCount; i++)
        {
            UpgradeForXp upgradeForXpTemp = upgradesGrid.transform.GetChild(i).GetComponent<UpgradeForXp>();
            upgradeForXpTemp.ResetLvl();
        }

        Save();

        CalculateDamages();
        enemyManager.RespawnEnemy();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Calculations
    public void CalculateDamages() { CalculateCrit(); CalculateDmg(); }

    public void CalculateDmg()
    {
        Damage = Utils.Progression(1, 1.6f, upgrades.DamageLvl);
        Damage = Utils.Progression(Damage, 2, doubleDamageUpgradeLvl);
        DmgCost = Utils.Progression(10, 1.6f, upgrades.DamageLvl);
        upgrades.UpdateIco();
    }
    public void CalculateCrit()
    {
        CritChance = 5 + upgrades.CritLvl - 1;
        CritCost = Utils.Progression(100, 100, upgrades.CritLvl);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Data manipulations
    public int critMultiplierUpgradeLvl;         public void CritMultiplierUpgrade()
                                                 { critMultiplierUpgradeLvl++; }
    public int crateDropChanceUpgradeLvl;        public void CrateDropChanceUpgrade()
                                                 { crateDropChanceUpgradeLvl++; }
    public int crateMultiplyUpgradeLvl;          public void CrateMultiplyUpgrade()
                                                 { crateMultiplyUpgradeLvl++; }
    public int multiplyCurrencyChanceUpgradeLvl; public void MultiplyCurrencyChanceUpgrade()
                                                 { multiplyCurrencyChanceUpgradeLvl++; }
    public int multiplyCurrencyUpgradeLvl;       public void MultiplyCurrencyUpgrade()
                                                 { multiplyCurrencyUpgradeLvl++; }
    public int betterLootChanceUpgradeLvl;       public void BetterLootChanceUpgrade()
                                                 { betterLootChanceUpgradeLvl++; }
    public int doubleDamageUpgradeLvl;           public void DoubleDamageUpgrade()
                                                 { doubleDamageUpgradeLvl++; }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Exit and postgame
public void Exit() { Application.Quit(); }
    public void OnApplicationQuit()
    {
        inventory.SetItemsBack();
        if (Currency > 0 || Births >= 0) { Save(); } 
    }
}