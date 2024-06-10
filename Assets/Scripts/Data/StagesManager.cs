using System;
using System.Collections.Generic;
using UnityEngine;

public class StagesManager : MonoBehaviour
{
    #region Appoint through the inspector
    public List<Stage> StagesDataBase = new List<Stage>();

    [SerializeField] private AudioSource AmbienceSource;
    [SerializeField] private AudioSource WMAmbienceSource;
    [SerializeField] private AudioClip WMAmbience;

    [SerializeField] private Inventory inventory;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private RewardManager giveReward;
    [SerializeField] private InterfaceManager interfaceManager;
    [SerializeField] private GameObject upgradesGrid;
    [SerializeField] private SpriteRenderer BG;
    [SerializeField] private Sprite WMBG;
    #endregion

    #region Variables
    private AudioClip Ambience;
    public Stage currentStage;
    public int CurrentStage;
    public int maxStage;
    public int StageIndex;
    public int BGIndex;
    #endregion

    #region Init
    private bool started;
    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        inventory = GetComponent<Inventory>();
        enemyManager = GetComponent<EnemyManager>();
        giveReward = GetComponent<RewardManager>();
        interfaceManager = FindObjectOfType<InterfaceManager>().GetComponent<InterfaceManager>();
        upgradesGrid = GameObject.Find("UpgradesGrid");
    }
    #endregion

    #region Stage management
    public void LoadStageData(bool newStage)
    {
        if (!started)
        {
            started = true;
            interfaceManager.UpdateAllText();
        }

        if (newStage)
        {
            StageIndex = UnityEngine.Random.Range(0, StagesDataBase.Count);
            BGIndex = UnityEngine.Random.Range(0, currentStage.BG.Length);
        }

        currentStage = StagesDataBase[StageIndex];
        maxStage = Math.Max(CurrentStage, maxStage);

        BG.sprite = currentStage.BG[BGIndex];
        Ambience = currentStage.Ambience;
        AmbienceSource.clip = Ambience;

        enemyManager.BossList = currentStage.Boss;
        enemyManager.EnemyList = currentStage.EnemyList;

        giveReward.KillReward = Utils.Progression(1, 5, CurrentStage - 1);
        enemyManager.EnemyHPMultiplier = Utils.Progression(1, 5, CurrentStage - 1);

        inventory.UpdateItemPrices();
        CheckUpgrades();
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
            BG.sprite = currentStage.BG[BGIndex];
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
        interfaceManager.StageTextUpdate();
    }
    #endregion
}

[Serializable]
public class Stage
{
    [Header("BackGround")]
    public Sprite[] BG;

    [Header("Enemies")]
    public GameObject[] Boss;
    public GameObject[] EnemyList;

    [Header("Audio")]
    public AudioClip Ambience;

    [Header("Item List")]
    public List<ItemPattern> ItemsDataBase = new();
}