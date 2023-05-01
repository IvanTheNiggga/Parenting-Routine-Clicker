using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public List<MinionElement> minionsDataBase = new();
    public List<int> instockMinions = new();

    private Clicker clicker;
    private Transform minionsGrid;
    private StagesManager stagesManager;
    public Minion minion1;
    public Minion minion2;

    public GameObject MinionButton;

    public Sprite None;

    public int minion1id;
    public int minion2id;
    public int slotid;

    private void Start()
    {
        minionsGrid = GameObject.Find("MinionsGrid").GetComponent<Transform>();
        GetMinionsList();
        Invoke(nameof(CheckUnfair), 1f);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Returners
    public bool isAbleToBirth()
    {
        clicker = GetComponent<Clicker>();
        stagesManager = GetComponent<StagesManager>();

        return stagesManager.CurrentStage > 9;
    }
    public bool isAbleToBuy()
    {
        return clicker.Currency >= BirthCost();
    }
    public double BirthCost()
    {
        double cost = 1;
        for (int i = 1; (clicker.Births + 1) * 5 > i; i++)
        {
            cost *= 5;
        }
        return cost * 50;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Lists
    public void UpdateMinionsList(int i)
    {
        if (!GameObject.Find(i + "Minion"))
        {
            MinionSlot ms = Instantiate(MinionButton).GetComponent<MinionSlot>();
            ms.name = i + "minion";
            ms.transform.SetParent(minionsGrid.transform);
            ms.id = i;
            ms.AddGraphics();
        }
    }

    public List<int> GetMinionsList()
    {
        for (int i = 0; i < minionsDataBase.Count; i++)
        {
            if (PlayerPrefs.HasKey($"has_{i}") && !instockMinions.Contains(i))
            {
                instockMinions.Add(i);
                UpdateMinionsList(i);
            }
        }
        return instockMinions;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Minion manipulations
    public void AddRandomMinion()
    {
        int random = Random.Range(0, minionsDataBase.Count);

        PlayerPrefs.SetInt($"has_{random}", random);
        if (!instockMinions.Contains(random))
        {
            instockMinions.Add(random);
        }

        GetMinionsList();
    }

    public void EquipMinion(int slot, int id)
    {
        switch (slot)
        {
            case 1:
                minion1.id = id; minion1id = id;
                minion1.UpdateMinionData();
                break;
            case 2:
                minion2.id = id; minion2id = id;
                minion2.UpdateMinionData();
                break;

        }
    }
    public void UnequipMinion(int slot)
    {
        switch (slot)
        {
            case 1:
                minion1 = GameObject.Find("Minion1").GetComponent<Minion>();
                minion1.id = -1; minion1id = -1;
                minion1.UpdateMinionData();
                break;
            case 2:
                minion2 = GameObject.Find("Minion2").GetComponent<Minion>();
                minion2.id = -1; minion2id = -1;
                minion2.UpdateMinionData();
                break;

        }
    }
    public void UnequipSelectedMinion()
    {
        switch (slotid)
        {
            case 1:
                minion1.id = -1;
                minion1.UpdateMinionData();
                break;
            case 2:
                minion2.id = -1;
                minion2.UpdateMinionData();
                break;

        }
    }
    public void UpgradeMinion(int slot)
    {
        switch (slot)
        {
            case 1:
                minion1.Upgrade();
                break;
            case 2:
                minion2.Upgrade();
                break;

        }
    }
    public void CheckUnfair()
    {
        if(minion1.id != -1 && minion2.id != -1)
        {
            if (minion2.CurrentLevel > minion1.CurrentLevel)
            {
                minion1.unfairLvl = minion2.CurrentLevel - minion1.CurrentLevel;
                string s = minion1.CurrentLevel > 0 ? $"+ {minion1.minionManager.minionsDataBase[minion1.id].DamageCoef * (0.2 * minion1.CurrentLevel) * 100}% " : "";
                minion1.text1.text = $"{minion1.nameobj}\nLevel {minion1.CurrentLevel}\n{minion1.minionManager.minionsDataBase[minion1.id].DamageCoef * 100}% {s}of your damage.\nBut he`s mad, his brother have {minion1.unfairLvl} more toys.";
            }
            else
            {
                minion1.unfairLvl = 0;
                string s = minion1.CurrentLevel > 0 ? $"+ {minion1.minionManager.minionsDataBase[minion1.id].DamageCoef * (0.2 * minion1.CurrentLevel) * 100}% " : "";
                minion1.text1.text = $"{minion1.nameobj}\nLevel {minion1.CurrentLevel}\n{minion1.minionManager.minionsDataBase[minion1.id].DamageCoef * 100}% {s}of your damage.";
            }
            if (minion1.CurrentLevel > minion2.CurrentLevel)
            {
                minion2.unfairLvl = minion1.CurrentLevel - minion2.CurrentLevel;
                string s1 = minion2.CurrentLevel > 0 ? $"+ {minion2.minionManager.minionsDataBase[minion2.id].DamageCoef * (0.2 * minion2.CurrentLevel) * 100}% " : "";
                minion2.text2.text = $"{minion2.nameobj}\nLevel {minion2.CurrentLevel}\n{minion2.minionManager.minionsDataBase[minion2.id].DamageCoef * 100}% {s1}of your damage.\nBut he`s mad, his brother have {minion2.unfairLvl} more toys.";
            }
            else
            {
                minion2.unfairLvl = 0;
                string s1 = minion2.CurrentLevel > 0 ? $"+ {minion2.minionManager.minionsDataBase[minion2.id].DamageCoef * (0.2 * minion2.CurrentLevel) * 100}% " : "";
                minion2.text2.text = $"{minion2.nameobj}\nLevel {minion2.CurrentLevel}\n{minion2.minionManager.minionsDataBase[minion2.id].DamageCoef * 100}% {s1}of your damage.";
            }
        }
    }
}

[System.Serializable]
public class MinionElement
{
    [Header("Name")]
    public string name;

    [Header("Sprites")]
    public Sprite Idle;
    public Sprite Attack;
    public Sprite Preview;

    [Header("Damage Stats")]
    public double DamageCoef;
}
