﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    #region Appointed on start
    private Clicker clicker;
    private UpgradesManager upgradesManager;
    private RewardManager rewardManager;
    private InterfaceManager interfaceManager;
    private Inventory inventory;
    private Message message;

    private SoundManager soundManager;

    private Text minerLootText;
    private Text minerLevelText;

    private GameObject startButton;
    #endregion

    #region Variables
    private double _incomeMultiplier = 1;
    public double IncomeMultiplier
    {
        get { return _incomeMultiplier; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                _incomeMultiplier = double.MaxValue / 100;
            }
            else
            {
                _incomeMultiplier = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }

    public DateTime LastEntranceTime;
    #endregion

    #region Init
    private void Start()
    {
        InitializeReferences();

        LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        InvokeRepeating(nameof(UpdateLootText), 0f, 1f);
        Invoke(nameof(UpdateLevel), 0.2f);
    }

    private void InitializeReferences()
    {
        startButton = GameObject.Find("Start(btn)");

        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
        rewardManager = GameObject.Find("ClickerManager").GetComponent<RewardManager>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        message = GameObject.Find("Message").GetComponent<Message>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        minerLevelText = GameObject.Find("MinerLvl(txt)").GetComponent<Text>();
        minerLootText = GameObject.Find("LootMiner(txt)").GetComponent<Text>();
    }
    #endregion

    #region UI
    private void UpdateLootText()
    {
        if (interfaceManager.minerOpened && clicker.MinerLvl > 0)
        {
            LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
            TimeSpan timePassed = DateTime.UtcNow - LastEntranceTime;
            int secondsPassed = (int)timePassed.TotalSeconds;
            minerLootText.text = "$" + NumFormat.FormatNumF0F1(rewardManager.GetRewardInfo(secondsPassed * IncomeMultiplier));
        }
    }
    #endregion

    #region Miner Behaviour
    public void StartMiner()
    {
        if (clicker.Currency >= 5000)
        {
            soundManager.PlayBuySound();
            clicker.MinerLvl = 1;
            UpdateLevel();

            clicker.Currency -= 5000;
            Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
        }
        else
        {
            message.SendMessage("You need $5000", 2);
            soundManager.PlayBruhSound();
        }
    }

    public void ResetMiner()
    {
        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
        ResetMinerLoot();

        clicker.MinerLvl = 0;
        IncomeMultiplier = 0;
        minerLevelText.text = $"Level {clicker.MinerLvl}";

        startButton.SetActive(true);
    }

    public void RebirthMiner()
    {
        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
        ResetMinerLoot();

        clicker.MinerLvl = 1 + upgradesManager.betterMineAfterRebirthLvl;
        UpdateLevel();

        startButton.SetActive(true);
    }

    public void UpgradeMiner()
    {
        LootMiner();
        clicker.MinerLvl++;
        clicker.MaxMinerLvl = Math.Max(clicker.MinerLvl, clicker.MaxMinerLvl);
        UpdateLevel();
        clicker.Save();
    }

    public void LootMiner()
    {
        LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        TimeSpan timePassed = DateTime.UtcNow - LastEntranceTime;
        int secondsPassed = (int)timePassed.TotalSeconds;

        if (clicker.MinerLvl == 0)
        {
            message.SendMessage("Your washing machine is not working yet", 2);
            soundManager.PlayBruhSound();
            return;
        }

        soundManager.PlayBuySound();
        rewardManager.GiveCurrency(secondsPassed * IncomeMultiplier);

        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

        interfaceManager.CurrencyTextUpdate();
        UpdateLootText();
    }

    public void ResetMinerLoot()
    {
        LastEntranceTime = DateTime.UtcNow;
        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
        UpdateLootText();
    }

    public void UpdateLevel()
    {
        if (clicker.MinerLvl > 0)
        {
            startButton.SetActive(false);
        }

        minerLevelText.text = $"Level {clicker.MinerLvl}";
        if (clicker.MinerLvl > 0) IncomeMultiplier = Math.Pow(50, clicker.MinerLvl - 1) * (upgradesManager.mineLootLvl + 1);
    }
    #endregion
}