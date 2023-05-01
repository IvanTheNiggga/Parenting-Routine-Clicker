using System;
using UnityEngine;
using UnityEngine.UI;

public class Washingmashine : MonoBehaviour
{
    private Clicker clicker;
    private GiveReward giveReward;
    private Interface interfaceManager;
    private Inventory inventory;
    private Message message;

    private SoundManager soundManager;
    private TextManager textManager;

    private GameObject investMenuGrid;
    private GameObject startWashingmashineButton;
    private Text washingmashineIncomingsText;
    private Text washingmashineLevelText;

    public DateTime LastEntranceTime;

    public TimeSpan timePassed;


    public double secondsPassed;

    public double incomeMultiplier;


    private void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        giveReward = GameObject.Find("ClickerManager").GetComponent<GiveReward>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<Interface>();
        message = GameObject.Find("Message").GetComponent<Message>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();

        investMenuGrid = GameObject.Find("InvestMenuGrid");
        startWashingmashineButton = GameObject.Find("==StartWashingmashine==");

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();

        washingmashineLevelText = GameObject.Find("WashingmashineLevelText").GetComponent<Text>();
        washingmashineIncomingsText = GameObject.Find("WashingmashineIncomingsText").GetComponent<Text>();


        LastEntranceTime = EntranceTimeUtils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;
        washingmashineIncomingsText.text = "$" + FormatNumsHelper.FormatNumF0F1(giveReward.GetRewardInfo(secondsPassed * incomeMultiplier));

        InvokeRepeating(nameof(UpdateIncomeText), 0f, 1f);
        Invoke(nameof(CalculateWashingmashineLvl), 0.2f);
    }

    public void StartWashingmashine()
    {
        if (clicker.Currency >= 5000)
        {
            clicker.Currency -= 5000;

            EntranceTimeUtils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

            clicker.WashingmashineLvl = 1;
            CalculateWashingmashineLvl();

            Destroy(startWashingmashineButton);
        }
        else
        {
            message.SendMessage("You need $5000", 2);
            soundManager.PlayBruhSound(); 
        }
    }

    public void UpgradeWashingmashine()
    {
        GetWashingmashineIncome();
        clicker.WashingmashineLvl++;
        CalculateWashingmashineLvl();
    }

    public void GetWashingmashineIncome()
    {
        LastEntranceTime = EntranceTimeUtils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;
        if (clicker.WashingmashineLvl == 0)
        {
            message.SendMessage("Your washing mashine not even works", 2);
            soundManager.PlayBruhSound(); 
            return; 
        }

        soundManager.PlayBuySound();
        giveReward.GiveCurrency(secondsPassed * incomeMultiplier);

        EntranceTimeUtils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

        textManager.CurrencyTextUpdate();

        UpdateIncomeText();
    }
    public void DeleteWashingmashineIncome()
    {
        LastEntranceTime = DateTime.UtcNow;
        EntranceTimeUtils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

        timePassed = DateTime.UtcNow - LastEntranceTime;
        secondsPassed = (int)timePassed.TotalSeconds;

        textManager.CurrencyTextUpdate();

        UpdateIncomeText();
    }
    public void CalculateWashingmashineLvl()
    {
        if (clicker.WashingmashineLvl > 0)
        {
            Destroy(startWashingmashineButton);
        }

        washingmashineLevelText.text = $"Level  {clicker.WashingmashineLvl}";
        CalculateIncomeMultiplier();
    }

    public void CalculateIncomeMultiplier()
    {
        if (clicker.WashingmashineLvl > 0)
        {
            incomeMultiplier = 1;

            for (int i = 1; i < clicker.WashingmashineLvl; i++)
            {
                incomeMultiplier *= 5;
            }
        }
    }
    public void UpdateIncomeText()
    {
        if (interfaceManager.washingMashineOpened)
        {
            LastEntranceTime = EntranceTimeUtils.GetDataTime("LastEntranceTime", DateTime.UtcNow);
            timePassed = DateTime.UtcNow - LastEntranceTime;
            secondsPassed = (int)timePassed.TotalSeconds;
            washingmashineIncomingsText.text = "$" + FormatNumsHelper.FormatNumF0F1(giveReward.GetRewardInfo(secondsPassed * incomeMultiplier));
        }
    }
}
