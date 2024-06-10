using System.Collections.Generic;
using UnityEngine;

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

    }
    #endregion

    #region Get currency
    public void GiveCurrency(double count)
    {
        double reward = count;
        if (Random.Range(0, 100f / upgradesManager.CurrencyChanceLvl) < 1f)
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
            if (Random.Range(0, 100f / (upgradesManager.DropRateLvl + 5)) < 1f)
            {
                countToSpawn++;
            }
        }
        inventory.SpawnRandomItemByType(ItemTypes.ItemPack, countToSpawn + upgradesManager.PacksCountLvl, true);
    }

    public void GetEnemyLoot()
    {
        if (Random.Range(0, 100f / (upgradesManager.DropRateLvl + 1)) < 1f)
        {
            inventory.SpawnRandomItemByType(ItemTypes.ItemPack, 1 + upgradesManager.PacksCountLvl, true);
        }
    }
    #endregion

    #region Get items
    public void GetRandomItem(int count)
    {
        if (Random.Range(0, 100f / (upgradesManager.BetterPacksLvl + 5f)) < 1f)
        {
            inventory.AddRandomItemByType(ItemTypes.Cloth, count, true);
        }
        else if (Random.Range(0, 100f / (upgradesManager.BetterPacksLvl + 5f)) < 1f)
        {
            inventory.AddRandomItemByType(ItemTypes.Toy, count, true);
        }
        else
        {
            inventory.AddRandomItemByType(ItemTypes.Garbage, count, true);
        }
    }
    #endregion
}