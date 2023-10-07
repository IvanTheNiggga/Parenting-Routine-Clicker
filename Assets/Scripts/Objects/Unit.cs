using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Clicker clicker;
    private Enemy enemy;
    public EnemyManager enemyManager;
    public UnitManager unitManager;
    public ObjectMovement objectMovement;

    private Sprite idleSprite;
    private Sprite attackSprite;
    public SpriteRenderer thisSprite;

    private Image imageUnit1;
    private Image imageUnit2;

    public Text text1;
    public Text text2;

    public int id;
    public int CurrentLevel;
    public double DamageCoef;
    public string nameobj;
    public int unfairLvl;

    private bool loaded;
    private bool able;

    private void Start()
    {
        id = PlayerPrefs.GetInt(name + "ID");

        UpdateUnitData();
        Invoke(nameof(MoveToEnemy), 2f);
    }

    public void UpdateUnitData()
    {
        unitManager = GameObject.Find("ClickerManager").GetComponent<UnitManager>();
        text1 = GameObject.Find("Unit1(txt)").GetComponent<Text>();
        text2 = GameObject.Find("Unit2(txt)").GetComponent<Text>();
        imageUnit1 = GameObject.Find("Unit1(img)").GetComponent<Image>();
        imageUnit2 = GameObject.Find("Unit2(img)").GetComponent<Image>();
        CurrentLevel = PlayerPrefs.GetInt(name + "CL");

        if (id != -1)
        {
            PlayerPrefs.SetInt($"has_{id}", id);
            PlayerPrefs.SetInt(name + "ID", id);
            idleSprite = unitManager.unitsDataBase[id].Idle;
            attackSprite = unitManager.unitsDataBase[id].Attack;
            thisSprite.sprite = idleSprite;
            double d = CurrentLevel > 0 ? (unitManager.unitsDataBase[id].DamageCoef * (0.2 * CurrentLevel)) : 0;
            DamageCoef = unitManager.unitsDataBase[id].DamageCoef + d;
            nameobj = unitManager.unitsDataBase[id].name;
            switch (name)
            {
                case "Unit1(obj)":
                    imageUnit1.sprite = unitManager.unitsDataBase[id].Preview;
                    string s = CurrentLevel > 0 ? $"+ {unitManager.unitsDataBase[id].DamageCoef * (0.2 * CurrentLevel) * 100}% " : "";
                    text1.text = $"{nameobj}\nLevel {CurrentLevel}\n{unitManager.unitsDataBase[id].DamageCoef * 100}% {s}of your damage";
                    break;
                case "Unit2(obj)":
                    imageUnit2.sprite = unitManager.unitsDataBase[id].Preview;
                    string s1 = CurrentLevel > 0 ? $"+ {unitManager.unitsDataBase[id].DamageCoef * (0.2 * CurrentLevel) * 100}% " : "";
                    text2.text = $"{nameobj}\nLevel {CurrentLevel}\n{unitManager.unitsDataBase[id].DamageCoef * 100}% {s1}of your damage";
                    break;
            }
            if (loaded)
            {
                unitManager.CheckUnfair();
            }
            loaded = true;
        }
        else
        {
            switch (name)
            {
                case "Unit1(obj)":
                    imageUnit1.sprite = unitManager.None;
                    text1.text = "";
                    break;
                case "Unit2(obj)":
                    imageUnit2.sprite = unitManager.None;
                    text2.text = "";
                    break;
            }
            PlayerPrefs.SetInt(name + "ID", id);
            idleSprite = null;
            attackSprite = null;
            thisSprite.sprite = null;
            DamageCoef = 0;
        }
    }

    

    public void Upgrade()
    {
        CurrentLevel++;
        PlayerPrefs.SetInt(name + "CL", CurrentLevel);
        UpdateUnitData();
    }

    public void GetEnemyComponent(Enemy enemyObj)
    {
        if (enemyObj != null)
        {
            enemy = enemyObj;
            able = true;
        }
        else
        {
            enemy = null;
            able = false;
        }
    }

    private void MoveToEnemy()
    {
        if (able == true && id >= 0 && enemyManager.clickable == true)
        {
            float x = enemy.transform.localPosition.x - 20;
            if (name == "Unit2(obj)")
            {
                x += 40;
            }

            objectMovement.MoveTo(new Vector2(x, enemy.transform.localPosition.y - 30), 0.2f, 1, true);
            Invoke(nameof(MoveBack), 0.5f);
        }
        Invoke(nameof(MoveToEnemy), 5);
    }

    private void MoveBack()
    {
        thisSprite.sprite = idleSprite;
        objectMovement.MoveTo(objectMovement.StartPos, 0.2f, 1, false);
    }

    public void Attack()
    {
        if (enemyManager.clickable == true)
        {
            thisSprite.sprite = attackSprite;
            if (unfairLvl > 0 && Random.Range(0, 100f / (unfairLvl * 5)) < 1f)
            {
                enemy.UnitSabotage();
            }
            else
            {
                enemy.UnitKick(clicker.Damage * DamageCoef);
            }
        }
    }
}
