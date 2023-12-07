using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private InterfaceManager interfaceManager;
    private TextManager tm;
    private UpgradesManager upgradesManager;

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
    [SerializeField] private double currency; public double Currency
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
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();
        upgradesManager = GetComponent<UpgradesManager>();

        upgradesGrid = GameObject.Find("UpgradesGrid");

        if (PlayerPrefs.HasKey("Currency"))
        {
            Load();
        }
        else
        {
            StartData();
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

        upgradesManager.DamageLvl = PlayerPrefs.GetInt("DamageUpgrade");
        upgradesManager.CritLvl = PlayerPrefs.GetInt("CritUpgrade");

        upgradesManager.doubleXPLvl = PlayerPrefs.GetInt("DoubleXPUpgrade");
        upgradesManager.betterMineAfterRebirthLvl = PlayerPrefs.GetInt("BetterMineAfterRebirthUpgrade");
        upgradesManager.moreBirthChanceLvl = PlayerPrefs.GetInt("MoreBirthChanceUpgrade");
        upgradesManager.betterStartLvl = PlayerPrefs.GetInt("BetterStartUpgrade");
        upgradesManager.doubleDamageLvl = PlayerPrefs.GetInt("DoubleDamageUpgrade");
        upgradesManager.critDamageLvl = PlayerPrefs.GetInt("CritDamageUpgrade");
        upgradesManager.dropRateLvl = PlayerPrefs.GetInt("DropRateUpgrade");
        upgradesManager.packsCountLvl = PlayerPrefs.GetInt("PacksCountUpgrade");
        upgradesManager.currencyChanceLvl = PlayerPrefs.GetInt("CurrencyChanceUpgrade");
        upgradesManager.doubleCurrencyLvl = PlayerPrefs.GetInt("BoubleCurrencyUpgrade");
        upgradesManager.betterPacksLvl = PlayerPrefs.GetInt("BetterPacksUpgrade");

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

        PlayerPrefs.SetInt("BetterMineAfterRebirthUpgrade", upgradesManager.betterMineAfterRebirthLvl);
        PlayerPrefs.SetInt("MaxMinerLvl", MaxMinerLvl);
        PlayerPrefs.SetInt("MinerLvl", MinerLvl);
        PlayerPrefs.SetInt("DamageUpgrade", upgradesManager.DamageLvl);
        PlayerPrefs.SetInt("CritUpgrade", upgradesManager.CritLvl);


        PlayerPrefs.SetInt("DoubleXPUpgrade", upgradesManager.doubleXPLvl);
        PlayerPrefs.SetInt("MoreBirthChanceUpgrade", upgradesManager.moreBirthChanceLvl);
        PlayerPrefs.SetInt("BetterStartUpgrade", upgradesManager.betterStartLvl);
        PlayerPrefs.SetInt("DoubleDamageUpgrade", upgradesManager.doubleDamageLvl);
        PlayerPrefs.SetInt("CritDamageUpgrade", upgradesManager.critDamageLvl);
        PlayerPrefs.SetInt("DropRateUpgrade", upgradesManager.dropRateLvl);
        PlayerPrefs.SetInt("PacksCountUpgrade", upgradesManager.packsCountLvl);
        PlayerPrefs.SetInt("CurrencyChanceUpgrade", upgradesManager.currencyChanceLvl);
        PlayerPrefs.SetInt("DoubleCurrencyUpgrade", upgradesManager.doubleCurrencyLvl);
        PlayerPrefs.SetInt("BetterPacksUpgrade", upgradesManager.betterPacksLvl);

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
        PlayerPrefs.SetInt("Unit1(obj)ID", -1);
        PlayerPrefs.SetInt("Unit2(obj)ID", -1);

        CritMultiplier = 3;
        stagesManager.CurrentStage = 1;
        stagesManager.maxStage = 1;
        enemyManager.EnemyHPMultiplier = 1;

        CalculateDamages();
        stagesManager.LoadStageData(true);
        optionsMenu.SetDefaultCap();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RebirthData()
    {
        inventory.DeleteItems();

        CritMultiplier = 3;

        stagesManager.CurrentStage = 1 + upgradesManager.betterStartLvl;
        Currency = 0;
        int childCount = enemyManager.DropParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = enemyManager.DropParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        miner.RebirthMiner();

        enemyManager.EnemyHPMultiplier = 1;

        upgradesManager.DamageLvl = 0;
        upgradesManager.CritLvl = 0;
        PlayerPrefs.SetInt("DamageUpgrade", upgradesManager.DamageLvl);
        PlayerPrefs.SetInt("CritUpgrade", upgradesManager.CritLvl);

        stagesManager.LoadStageData(true);
        interfaceManager.UpdateUpgrades();
        tm.UpdateAllText();
    }

    public void Birth()
    {
        int births = Births;

        RebirthData();

        Births = births + 1;
        if (Random.Range(0, 100f / (upgradesManager.moreBirthChanceLvl * 2.5f)) < 1f)
        {
            unitManager.AddRandomUnit();
        }
        if (Random.Range(0, 100f / (upgradesManager.moreBirthChanceLvl * 1.25f)) < 1f)
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
        Damage = Utils.Progression(1, 1.6f, upgradesManager.DamageLvl);
        Damage = Utils.Progression(Damage, 2, upgradesManager.doubleDamageLvl);
    }
    public void CalculateCrit()
    {
        CritChance = 5 + upgradesManager.CritLvl;
    }
    #endregion

    public void OnApplicationQuit()
    {
        inventory.SetItemsBack();
        if (Currency > 0 || Births >= 0) { Save(); }
    }
}