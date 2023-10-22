using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagesManager : MonoBehaviour
{
    public List<Stage> StagesDataBase = new List<Stage>();

    public AudioSource AmbienceSource;
    public AudioSource WMAmbienceSource;
    public AudioClip Ambience;
    public AudioClip WMAmbience;

    private EnemyManager enemyManager;
    private RewardManager giveReward;
    private GameObject upgradesGrid;
    private InterfaceManager interfaceManager;
    private TextManager tm;

    private SpriteRenderer BG;

    public Sprite WMBG;

    public int StageIndex;
    public int CurrentStage;

    private bool started;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
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

        interfaceManager.CheckRebirth();

        if (newStage)
        {
            StageIndex = Random.Range(0, StagesDataBase.Count);
        }

        Stage currentStage = StagesDataBase[StageIndex];

        BG.sprite = currentStage.BG;
        enemyManager.Boss = currentStage.Boss;
        enemyManager.EnemyList = currentStage.EnemyList;
        Ambience = currentStage.Ambience;
        AmbienceSource.clip = Ambience;
        enemyManager.enemySpawnSound = currentStage.EnemySpawn;

        giveReward.KillReward = 1;
        enemyManager.EnemyHPMultiplier = 1;
        for (int i = 1; CurrentStage > i; i++)
        {
            giveReward.KillReward *= 5;
            enemyManager.EnemyHPMultiplier *= 5.1;
        }
        CheckUpgrades();

        enemyManager.enemySpawnSound = currentStage.EnemySpawn;

        interfaceManager.UpdateUpgrades();
        AmbienceSource.Play();
    }

    private void CheckUpgrades()
    {
        int childCount = upgradesGrid.transform.childCount;
        for (int i = 2; i < childCount; i++)
        {
            upgradesGrid.transform.GetChild(i).GetComponent<UpgradeForXp>().AddGraphics();
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

[System.Serializable]
public class Items
{
    public Sprite ico;

    public string nameObject;

    public string type;

    public string stage;
    public float currencyPrice;

    public float xpPrice;

    public string useMethodName;
}