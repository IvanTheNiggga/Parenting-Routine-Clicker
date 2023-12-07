using System;
using System.Collections.Generic;
using UnityEngine;

public class StagesManager : MonoBehaviour
{
    public List<Stage> StagesDataBase = new List<Stage>();

    public AudioSource AmbienceSource;
    public AudioSource WMAmbienceSource;
    public AudioClip Ambience;
    public AudioClip WMAmbience;

    private Inventory inventory;
    private Clicker clicker;
    private EnemyManager enemyManager;
    private RewardManager giveReward;
    private GameObject upgradesGrid;
    private InterfaceManager interfaceManager;
    private TextManager tm;

    private SpriteRenderer BG;

    public Sprite WMBG;

    public int StageIndex;
    public int maxStage;
    public int CurrentStage;

    private bool started;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        inventory = GetComponent<Inventory>();
        clicker = GetComponent<Clicker>();
        enemyManager = GetComponent<EnemyManager>();
        giveReward = GetComponent<RewardManager>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();
        upgradesGrid = GameObject.Find("UpgradesGrid");
        AmbienceSource = GameObject.Find("AmbienceSource").GetComponent<AudioSource>();
        WMAmbienceSource = GameObject.Find("WMAmbienceSource").GetComponent<AudioSource>();
        BG = GameObject.Find("BG").GetComponent<SpriteRenderer>();
    }

    public void LoadStageData(bool newStage)
    {
        if (!started)
        {
            started = true;
            tm.UpdateAllText();
        }

        if (newStage)
        {
            StageIndex = UnityEngine.Random.Range(0, StagesDataBase.Count);
        }

        Stage currentStage = StagesDataBase[StageIndex];

        BG.sprite = currentStage.BG;
        enemyManager.Boss = currentStage.Boss;
        enemyManager.EnemyList = currentStage.EnemyList;
        Ambience = currentStage.Ambience;
        AmbienceSource.clip = Ambience;
        enemyManager.enemySpawnSound = currentStage.EnemySpawn;

        giveReward.KillReward = Utils.Progression(1, 5, CurrentStage - 1);
        enemyManager.EnemyHPMultiplier = Utils.Progression(1, 5, CurrentStage - 1);
        inventory.UpdateItemPrices();
        CheckUpgrades();

        enemyManager.enemySpawnSound = currentStage.EnemySpawn;
        maxStage = Math.Max(CurrentStage, maxStage);

        interfaceManager.UpdateUpgrades();
        AmbienceSource.Play();
    }

    private void CheckUpgrades()
    {
        int childCount = upgradesGrid.transform.childCount;
        for (int i = 2; i < childCount; i++)
        {
            upgradesGrid.transform.GetChild(i).GetComponent<UpgradeObject>().AddGraphics();
        }
    }

    public void ChangeAmbience(bool isGame)
    {
        if (isGame)
        {
            BG.sprite = StagesDataBase[StageIndex].BG;
            AmbienceSource.UnPause();
            WMAmbienceSource.Pause();
        }
        else
        {
            BG.sprite = WMBG;
            AmbienceSource.Pause();
            if (WMAmbienceSource.isPlaying) WMAmbienceSource.UnPause();
            else WMAmbienceSource.Play();
        }
    }

    public void NextStage()
    {
        CurrentStage++;
        LoadStageData(true);
        tm.StageTextUpdate();
    }
}

[System.Serializable]
public class Stage
{
    [Header("BackGround")]
    public Sprite BG;

    [Header("Enemies")]
    public GameObject Boss;
    public GameObject[] EnemyList;

    [Header("Audio")]
    public AudioClip Ambience;
    public AudioClip EnemySpawn;

    [Header("Item List")]
    public List<Items> itemsDataBase = new();
}