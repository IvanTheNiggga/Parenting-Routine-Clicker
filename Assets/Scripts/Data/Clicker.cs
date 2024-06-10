using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Clicker : MonoBehaviour
{
    #region Local
    private Miner miner;
    private EnemyManager enemyManager;
    private Inventory inventory;
    private UnitManager unitManager;
    private Settings optionsMenu;
    private StagesManager stagesManager;
    private InterfaceManager interfaceManager;
    private UpgradesManager upgradesManager;

    public GameObject timerObject;
    #endregion

    #region Variables
    public int Rebirths;
    public int RebirthsPoints;
    public int MinerLvl;
    public int MaxMinerLvl;
    public int CritMultiplier;
    public float Experience;

    private double lastDealedDamage; public double LastDealedDamage
    {
        get { return lastDealedDamage; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                lastDealedDamage = double.MaxValue / 100;
            }
            else
            {
                lastDealedDamage = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
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

    #region Unity Lifecycle
    private void Start()
    {
        miner = GameObject.Find("Miner").GetComponent<Miner>();
        enemyManager = GetComponent<EnemyManager>();
        inventory = GetComponent<Inventory>();
        unitManager = GetComponent<UnitManager>();
        optionsMenu = GameObject.Find("Settings").GetComponent<Settings>();
        stagesManager = GetComponent<StagesManager>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        upgradesManager = GetComponent<UpgradesManager>();

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
        CritMultiplier = PlayerPrefs.GetInt(nameof(CritMultiplier));

        stagesManager.CurrentStage = PlayerPrefs.GetInt(nameof(stagesManager.CurrentStage));
        stagesManager.StageIndex = PlayerPrefs.GetInt(nameof(stagesManager.StageIndex));
        stagesManager.BGIndex = PlayerPrefs.GetInt(nameof(stagesManager.BGIndex));
        stagesManager.maxStage = PlayerPrefs.GetInt(nameof(stagesManager.maxStage));
        Currency = LoadBig(nameof(Currency));
        Experience = PlayerPrefs.GetFloat(nameof(Experience));
        RebirthsPoints = PlayerPrefs.GetInt(nameof(RebirthsPoints));
        Rebirths = PlayerPrefs.GetInt(nameof(Rebirths));

        MinerLvl = PlayerPrefs.GetInt(nameof(MinerLvl));
        MaxMinerLvl = PlayerPrefs.GetInt(nameof(MaxMinerLvl));

        upgradesManager.DamageLvl = PlayerPrefs.GetInt(nameof(upgradesManager.DamageLvl));
        upgradesManager.CritLvl = PlayerPrefs.GetInt(nameof(upgradesManager.CritLvl));

        upgradesManager.DoubleXPLvl = PlayerPrefs.GetInt(nameof(upgradesManager.DoubleXPLvl));
        upgradesManager.BetterMineAfterRebirthLvl = PlayerPrefs.GetInt(nameof(upgradesManager.BetterMineAfterRebirthLvl));
        upgradesManager.MoreRebirthPointsChanceLvl = PlayerPrefs.GetInt(nameof(upgradesManager.MoreRebirthPointsChanceLvl));
        upgradesManager.BetterStartLvl = PlayerPrefs.GetInt(nameof(upgradesManager.BetterStartLvl));
        upgradesManager.DoubleDamageLvl = PlayerPrefs.GetInt(nameof(upgradesManager.DoubleDamageLvl));
        upgradesManager.CritDamageLvl = PlayerPrefs.GetInt(nameof(upgradesManager.CritDamageLvl));
        upgradesManager.DropRateLvl = PlayerPrefs.GetInt(nameof(upgradesManager.DropRateLvl));
        upgradesManager.PacksCountLvl = PlayerPrefs.GetInt(nameof(upgradesManager.PacksCountLvl));
        upgradesManager.CurrencyChanceLvl = PlayerPrefs.GetInt(nameof(upgradesManager.CurrencyChanceLvl));
        upgradesManager.DoubleCurrencyLvl = PlayerPrefs.GetInt(nameof(upgradesManager.DoubleCurrencyLvl));
        upgradesManager.BetterPacksLvl = PlayerPrefs.GetInt(nameof(upgradesManager.BetterPacksLvl));

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
        PlayerPrefs.SetInt(nameof(CritMultiplier), CritMultiplier);
        PlayerPrefs.SetInt(nameof(stagesManager.CurrentStage), stagesManager.CurrentStage);
        PlayerPrefs.SetInt(nameof(stagesManager.StageIndex), stagesManager.StageIndex);
        PlayerPrefs.SetInt(nameof(stagesManager.BGIndex), stagesManager.BGIndex);
        PlayerPrefs.SetInt(nameof(stagesManager.maxStage), stagesManager.maxStage);
        SaveBig(nameof(Currency), Currency);
        PlayerPrefs.SetFloat(nameof(Experience), Experience);
        PlayerPrefs.SetInt(nameof(RebirthsPoints), RebirthsPoints);
        PlayerPrefs.SetInt(nameof(Rebirths), Rebirths);

        PlayerPrefs.SetInt(nameof(MaxMinerLvl), MaxMinerLvl);
        PlayerPrefs.SetInt(nameof(MinerLvl), MinerLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.DamageLvl), upgradesManager.DamageLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.CritLvl), upgradesManager.CritLvl);

        PlayerPrefs.SetInt(nameof(upgradesManager.BetterMineAfterRebirthLvl), upgradesManager.BetterMineAfterRebirthLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.DoubleXPLvl), upgradesManager.DoubleXPLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.MoreRebirthPointsChanceLvl), upgradesManager.MoreRebirthPointsChanceLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.BetterStartLvl), upgradesManager.BetterStartLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.DoubleDamageLvl), upgradesManager.DoubleDamageLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.CritDamageLvl), upgradesManager.CritDamageLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.DropRateLvl), upgradesManager.DropRateLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.PacksCountLvl), upgradesManager.PacksCountLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.CurrencyChanceLvl), upgradesManager.CurrencyChanceLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.DoubleCurrencyLvl), upgradesManager.DoubleCurrencyLvl);
        PlayerPrefs.SetInt(nameof(upgradesManager.BetterPacksLvl), upgradesManager.BetterPacksLvl);

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

        stagesManager.CurrentStage = 1 + upgradesManager.BetterStartLvl;
        Currency = 0;

        miner.RebirthMiner();

        enemyManager.DestroyLoot();

        upgradesManager.DamageLvl = 0;
        upgradesManager.CritLvl = 0;

        stagesManager.LoadStageData(true);
        interfaceManager.UpdateAllText();
    }

    public void Rebirth()
    {
        RebirthsPoints += 1;
        Rebirths += 1;

        RebirthData();

        if (Random.Range(0, 100f / (upgradesManager.MoreRebirthPointsChanceLvl * 2.5f)) < 1f)
        {
            unitManager.AddRandomUnit();
        }
        if (Random.Range(0, 100f / (upgradesManager.MoreRebirthPointsChanceLvl * 1.25f)) < 1f)
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
        Damage = Utils.Progression(Damage, 2, upgradesManager.DoubleDamageLvl);
    }
    public void CalculateCrit()
    {
        CritChance = 5 + upgradesManager.CritLvl;
    }
    #endregion

    public void OnApplicationQuit()
    {
        inventory.SetItemsBack();
        if (Currency > 0 || Rebirths >= 0) { Save(); }
    }
}