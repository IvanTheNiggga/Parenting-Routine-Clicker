using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<UnitElement> unitsDataBase = new();
    public List<int> instockUnits = new();

    public Unit unit1;
    public Unit unit2;

    public GameObject UnitButton;
    public Sprite None;

    private Clicker clicker;
    private StagesManager stagesManager;

    public int unit1id;
    public int unit2id;
    public int slotid;

    private Transform unitsGrid;

    private void Start()
    {
        unitsGrid = GameObject.Find("UnitsGrid").GetComponent<Transform>();
        GetUnitsList();
        Invoke(nameof(CheckUnfair), 1f);
    }
    /////////////////////////////////////////////////////////////////////////////////////////////// Returners
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
    /////////////////////////////////////////////////////////////////////////////////////////////// Lists
    public void UpdateUnitsList(int i)
    {
        if (!GameObject.Find(i + "Unit"))
        {
            UnitSlot ms = Instantiate(UnitButton).GetComponent<UnitSlot>();
            ms.name = i + "unit";
            ms.transform.SetParent(unitsGrid.transform);
            ms.id = i;
            ms.AddGraphics();
        }
    }

    public List<int> GetUnitsList()
    {
        for (int i = 0; i < unitsDataBase.Count; i++)
        {
            if (PlayerPrefs.HasKey($"has_{i}") && !instockUnits.Contains(i))
            {
                instockUnits.Add(i);
                UpdateUnitsList(i);
            }
        }
        return instockUnits;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////// Units manipulations
    public void AddRandomUnit()
    {
        int random = Random.Range(0, unitsDataBase.Count);

        PlayerPrefs.SetInt($"has_{random}", random);
        if (!instockUnits.Contains(random))
        {
            instockUnits.Add(random);
        }

        GetUnitsList();
    }

    public void EquipUnit(int slot, int id)
    {
        switch (slot)
        {
            case 1:
                unit1.id = id; unit1id = id;
                unit1.UpdateUnitData();
                break;
            case 2:
                unit2.id = id; unit2id = id;
                unit2.UpdateUnitData();
                break;

        }
    }
    public void UnequipUnit(int slot)
    {
        switch (slot)
        {
            case 1:
                unit1 = GameObject.Find("Unit1(obj)").GetComponent<Unit>();
                unit1.id = -1; unit1id = -1;
                unit1.UpdateUnitData();
                break;
            case 2:
                unit2 = GameObject.Find("Unit2(obj)").GetComponent<Unit>();
                unit2.id = -1; unit2id = -1;
                unit2.UpdateUnitData();
                break;

        }
    }
    public void UnequipSelectedUnit()
    {
        switch (slotid)
        {
            case 1:
                unit1.id = -1;
                unit1.UpdateUnitData();
                break;
            case 2:
                unit2.id = -1;
                unit2.UpdateUnitData();
                break;

        }
    }
    public void UpgradeUnit(int slot)
    {
        switch (slot)
        {
            case 1:
                unit1.Upgrade();
                break;
            case 2:
                unit2.Upgrade();
                break;

        }
    }
    public void CheckUnfair()
    {
        if(unit1.id != -1 && unit2.id != -1)
        {
            if (unit2.CurrentLevel > unit1.CurrentLevel + 1)
            {
                unit1.unfairLvl = unit2.CurrentLevel - unit1.CurrentLevel;
                unit1.text1.text = $"{unit1.nameobj} (Lv. {unit1.CurrentLevel})\n\nx{unit1.DamageCoef} from your damage.\nBut he`s mad, his brother have {unit1.unfairLvl} more toys.";
            }
            else
            {
                unit1.unfairLvl = 0;
                unit1.text1.text = $"{unit1.nameobj} (Lv. {unit1.CurrentLevel})\n\nx{unit1.DamageCoef} from your damage.";
            }
            if (unit1.CurrentLevel > unit2.CurrentLevel + 1)
            {
                unit2.unfairLvl = unit1.CurrentLevel - unit2.CurrentLevel;
                unit2.text2.text = $"{unit2.nameobj} (Lv. {unit2.CurrentLevel})\n\nx{unit2.DamageCoef} from your damage.\nBut he`s mad, his brother have {unit2.unfairLvl} more toys.";
            }
            else
            {
                unit2.unfairLvl = 0;
                unit2.text2.text = $"{unit2.nameobj} (Lv. {unit2.CurrentLevel})\n\nx{unit2.DamageCoef} from your damage.";
            }
        }
    }
}

[System.Serializable]
public class UnitElement
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
