using UnityEngine;

public class GiveReward : MonoBehaviour
{
    private Clicker clicker;
    private Inventory inventory;
    private StagesManager stagesManager;
    private TextManager tm;

    public int toyscount;
    public int clothscount;
    public int randomitemcount;
    public int garbagecount;

    double killReward;
    public double KillReward
    {   get { return killReward; }
        set
        {
            if (value > double.MaxValue)
            { killReward = double.MaxValue; return; } 
            killReward = value;
        }
    }
    void Start()
    {
        stagesManager = GetComponent<StagesManager>();
        inventory = GetComponent<Inventory>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();

        GetItemsInfo();
    }
    public void GetItemsInfo()
    {
        for (int i = 0; stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase.Count > i; i++)
        {
            if (stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[i].type == "Toy")
            { toyscount++; }
            else if (stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[i].type == "Cloth")
            { clothscount++; }
            else if (stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[i].type == "Random item")
            { randomitemcount++; }
            else if (stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[i].type == "Garbage")
            { garbagecount++; }   
    }   }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET CURRENCY
    public void GiveCurrency(double count)
    {
        double reward;
        reward = count * (clicker.multiplyCurrencyUpgradeLvl + 1);
        if (Random.Range(0, 100f / clicker.multiplyCurrencyChanceUpgradeLvl) < 1f)
        {
            reward *= 2;
        }
        clicker.Currency += reward;
    }
    public double GetRewardInfo(double count)
    {
        clicker = GetComponent<Clicker>();
        double reward;
        reward = count * (clicker.multiplyCurrencyUpgradeLvl + 1);
        if (reward < double.MaxValue || double.IsNaN(reward) == false || double.IsInfinity(reward) == false)
        {
            return reward;
        }
        else
        {
            return double.MaxValue;
        }

    }
    public void GiveMeReward(int count)
    {
        GiveCurrency(KillReward * count);

        tm.CurrencyTextUpdate();
    }
    public void GetBossLoot(int count)
    {
        int countToSpawn = 1;
        for (int i = 0; i < count; i++)
        {
            if (Random.Range(0, 100f / (clicker.crateDropChanceUpgradeLvl + 5)) < 1f)
            {
                countToSpawn++;
            }
        }
        SpawnItemRandom(countToSpawn + clicker.crateMultiplyUpgradeLvl);
    }
    public void GetEnemyLoot()
    {
        if (Random.Range(0, 100f / (clicker.crateDropChanceUpgradeLvl + 1)) < 1f)
        {
            SpawnItemRandom(1 + clicker.crateMultiplyUpgradeLvl);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET/SPAWN ITEMS
    
    public void GetRandomItem(int count)
    {
        if (Random.Range(0, 100f / (clicker.betterLootChanceUpgradeLvl + 5f)) < 1f)
        {
            GetRandomCloth(count);
        }
        else if (Random.Range(0, 100f / (clicker.betterLootChanceUpgradeLvl + 5f)) < 1f)
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
}
