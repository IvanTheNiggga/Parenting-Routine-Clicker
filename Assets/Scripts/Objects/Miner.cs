using System;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    private Clicker clicker;
    private RewardManager rewardManager;
    private InterfaceManager interfaceManager;
    private Inventory inventory;
    private Message message;

    private SoundManager soundManager;
    private TextManager textManager;

    private Text minerLootText;
    private Text minerLevelText;

    private double incomeMultiplier = 1;

    public DateTime LastEntranceTime;

    private void Start()
    {
        InitializeReferences();

        LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        UpdateLootText();
        InvokeRepeating(nameof(UpdateLootText), 0f, 1f);
        Invoke(nameof(UpdateLevel), 0.2f);
    }

    private void InitializeReferences()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        rewardManager = GameObject.Find("ClickerManager").GetComponent<RewardManager>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        message = GameObject.Find("Message").GetComponent<Message>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();
        minerLevelText = GameObject.Find("MinerLvl(txt)").GetComponent<Text>();
        minerLootText = GameObject.Find("LootMiner(txt)").GetComponent<Text>();
    }

    private void UpdateLootText()
    {
        if (interfaceManager.minerOpened)
        {
            LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
            TimeSpan timePassed = DateTime.UtcNow - LastEntranceTime;
            int secondsPassed = (int)timePassed.TotalSeconds;
            minerLootText.text = "$" + NumFormat.FormatNumF0F1(rewardManager.GetRewardInfo(secondsPassed * incomeMultiplier));
        }
    }

    public void StartMiner()
    {
        if (clicker.Currency >= 5000)
        {
            soundManager.PlayBuySound();
            clicker.MinerLvl = 1;
            UpdateLevel();

            clicker.Currency -= 5000;
            Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
            Destroy(GameObject.Find("Start(btn)"));
        }
        else
        {
            message.SendMessage("You need $5000", 2);
            soundManager.PlayBruhSound();
        }
    }

    public void UpgradeMiner()
    {
        LootMiner();
        clicker.MinerLvl++;
        UpdateLevel();
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
        rewardManager.GiveCurrency(secondsPassed * incomeMultiplier);

        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

        textManager.CurrencyTextUpdate();
        UpdateLootText();
    }

    public void ResetMinerLoot()
    {
        LastEntranceTime = DateTime.UtcNow;
        Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

        TimeSpan timePassed = DateTime.UtcNow - LastEntranceTime;
        int secondsPassed = (int)timePassed.TotalSeconds;

        textManager.CurrencyTextUpdate();
        UpdateLootText();
    }

    public void UpdateLevel()
    {
        if (clicker.MinerLvl > 0)
        {
            Destroy(GameObject.Find("Start(btn)"));
        }

        minerLevelText.text = $"Level {clicker.MinerLvl}";
        CalculateIncomeMultiplier();
    }

    private void CalculateIncomeMultiplier()
    {
        if (clicker.MinerLvl > 0)
        {
            incomeMultiplier = Math.Pow(5, clicker.MinerLvl - 1);
        }
    }
}