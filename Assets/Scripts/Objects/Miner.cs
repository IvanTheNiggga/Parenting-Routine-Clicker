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

    private GameObject investMenuGrid;
    private GameObject startButton;
    private Text minerLootText;
    private Text minerLevelText;

    public DateTime LastEntranceTime;

    public TimeSpan timePassed;


    public int secondsPassed;

    public double incomeMultiplier;


    private void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        rewardManager = GameObject.Find("ClickerManager").GetComponent<RewardManager>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        message = GameObject.Find("Message").GetComponent<Message>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();

        investMenuGrid = GameObject.Find("InvestMenuGrid");
        startButton = GameObject.Find("Start(btn)");

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();

        minerLevelText = GameObject.Find("MinerLvl(txt)").GetComponent<Text>();
        minerLootText = GameObject.Find("LootMiner(txt)").GetComponent<Text>();


        LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;
        minerLootText.text = "$" + NumFormat.FormatNumF0F1(rewardManager.GetRewardInfo(secondsPassed * incomeMultiplier));

        InvokeRepeating(nameof(UpdateLootText), 0f, 1f);
        Invoke(nameof(UpdateLevel), 0.2f);
    }

    public void StartMiner()
    {
        if (clicker.Currency >= 5000)
        {
            clicker.MinerLvl = 1;
            UpdateLevel();

            clicker.Currency -= 5000;
            Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);
            Destroy(startButton);
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
        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;
        if (clicker.MinerLvl == 0)
        {
            message.SendMessage("Your washing mashine not even works", 2);
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

        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;

        textManager.CurrencyTextUpdate();
        UpdateLootText();
    }
    public void UpdateLevel()
    {
        if (clicker.MinerLvl > 0)
        {
            Destroy(startButton);
        }

        minerLevelText.text = $"Level  {clicker.MinerLvl}";
        CalculateIncomeMultiplier();
    }

    public void CalculateIncomeMultiplier()
    {
        if (clicker.MinerLvl > 0)
        {
            incomeMultiplier = 1;

            for (int i = 1; i < clicker.MinerLvl; i++)
            {
                incomeMultiplier *= 5;
            }
        }
    }
    public void UpdateLootText()
    {
        if (interfaceManager.minerOpened)
        {
            LastEntranceTime = Utils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
            timePassed = DateTime.UtcNow - LastEntranceTime;
            secondsPassed = (int)timePassed.TotalSeconds;
            minerLootText.text = "$" + NumFormat.FormatNumF0F1(rewardManager.GetRewardInfo(secondsPassed * incomeMultiplier));
        }
    }
}
