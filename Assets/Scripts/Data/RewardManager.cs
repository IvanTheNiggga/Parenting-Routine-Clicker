﻿using UnityEngine;

public class RewardManager : MonoBehaviour
{
    #region Appointed on start
    private Clicker clicker;
    private Inventory inventory;
    private StagesManager stagesManager;
    private InterfaceManager interfaceManager;
    private UpgradesManager upgradesManager;
    #endregion

    #region Variables
    private int toyscount;
    private int clothscount;
    private int randomitemcount;
    private int garbagecount;

    private double killReward;
    public double KillReward
    {
        get { return killReward; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                killReward = double.MaxValue / 100;
            }
            else
            {
                killReward = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }
    #endregion

    #region Init
    void Start()
    {
        clicker = GetComponent<Clicker>();
        upgradesManager = GetComponent<UpgradesManager>();
        stagesManager = GetComponent<StagesManager>();
        inventory = GetComponent<Inventory>();
        interfaceManager = FindObjectOfType<InterfaceManager>().GetComponent<InterfaceManager>();

        GetItemsInfo();
    }

    public void GetItemsInfo()
    {
        toyscount = clothscount = randomitemcount = garbagecount = 0;

        foreach (var itemData in stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase)
        {
            ItemTypes itemType = itemData.type;

            switch (itemType)
            {
                case ItemTypes.Toy:
                    toyscount++;
                    break;
                case ItemTypes.Cloth:
                    clothscount++;
                    break;
                case ItemTypes.ItemPack:
                    randomitemcount++;
                    break;
                case ItemTypes.Garbage:
                    garbagecount++;
                    break;
            }
        }
    }
    #endregion

    #region Get currency
    public void GiveCurrency(double count)
    {
        double reward = count;
        if (Random.Range(0, 100f / upgradesManager.currencyChanceLvl) < 1f)
        {
            reward *= 2;
        }
        clicker.Currency += reward;
    }

    public double GetRewardInfo(double count)
    {
        clicker = GetComponent<Clicker>();
        double reward = count;
        if (reward < double.MaxValue && !double.IsNaN(reward) && !double.IsInfinity(reward))
        {
            return reward;
        }
        return double.MaxValue;
    }

    public void GiveMeReward(int count)
    {
        GiveCurrency(KillReward * count);
        interfaceManager.CurrencyTextUpdate();
        interfaceManager.ExperienceTextUpdate();
    }
    public void GiveMeReward(int count, double KillReward)
    {
        GiveCurrency(KillReward * count);
        interfaceManager.CurrencyTextUpdate();
        interfaceManager.ExperienceTextUpdate();
    }

    public void GetBossLoot(int count)
    {
        int countToSpawn = 1;
        for (int i = 0; i < count; i++)
        {
            if (Random.Range(0, 100f / (upgradesManager.dropRateLvl + 5)) < 1f)
            {
                countToSpawn++;
            }
        }
        SpawnItemRandom(countToSpawn + upgradesManager.packsCountLvl);
    }

    public void GetEnemyLoot()
    {
        if (Random.Range(0, 100f / (upgradesManager.dropRateLvl + 1)) < 1f)
        {
            SpawnItemRandom(1 + upgradesManager.packsCountLvl);
        }
    }
    #endregion

    #region Get items
    public void GetRandomItem(int count)
    {
        if (Random.Range(0, 100f / (upgradesManager.betterPacksLvl + 5f)) < 1f)
        {
            GetRandomCloth(count);
        }
        else if (Random.Range(0, 100f / (upgradesManager.betterPacksLvl + 5f)) < 1f)
        {
            GetRandomToy(count);
        }
        else
        {
            GetRandomGarbage(count);
        }
    }

    public void SpawnItemRandom(int count)
    {
        inventory.SpawnItem(stagesManager.StageIndex, Random.Range(toyscount + clothscount + garbagecount, toyscount + clothscount + garbagecount + randomitemcount), count);
    }

    public void GetItemRandom(int count)
    {
        inventory.AddItem(stagesManager.StageIndex, Random.Range(toyscount + clothscount + garbagecount, toyscount + clothscount + garbagecount + randomitemcount), count);
    }

    public void GetRandomGarbage(int count)
    {
        inventory.AddItem(stagesManager.StageIndex, Random.Range(toyscount + clothscount, toyscount + clothscount + garbagecount), count);
    }

    public void GetRandomCloth(int count)
    {
        inventory.AddItem(stagesManager.StageIndex, Random.Range(toyscount, toyscount + clothscount), count);
    }

    public void GetRandomToy(int count)
    {
        inventory.AddItem(stagesManager.StageIndex, Random.Range(0, toyscount), count);
    }
    #endregion
}