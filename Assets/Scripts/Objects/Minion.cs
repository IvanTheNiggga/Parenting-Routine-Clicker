using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    public Clicker clicker;
    private Enemy enemy;
    public EnemyManager enemyManager;
    public MinionManager minionManager;
    public ObjectMovement objectMovement;

    private Sprite idleSprite;
    private Sprite attackSprite;
    public SpriteRenderer thisSprite;

    private Image imageMinion1;
    private Image imageMinion2;

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

        UpdateMinionData();

        switch (name)
        {
            case "Minion1":
                Invoke(nameof(MoveToEnemy), 2.5f);
                break;
            case "Minion2":
                Invoke(nameof(MoveToEnemy), 5f);
                break;
        }
    }

    public void UpdateMinionData()
    {
        minionManager = GameObject.Find("ClickerManager").GetComponent<MinionManager>();
        text1 = GameObject.Find("MinionDescription1").GetComponent<Text>();
        text2 = GameObject.Find("MinionDescription2").GetComponent<Text>();
        imageMinion1 = GameObject.Find("MinionImage1").GetComponent<Image>();
        imageMinion2 = GameObject.Find("MinionImage2").GetComponent<Image>();
        CurrentLevel = PlayerPrefs.GetInt(name + "CL");

        if (id != -1)
        {
            PlayerPrefs.SetInt($"has_{id}", id);
            PlayerPrefs.SetInt(name + "ID", id);
            idleSprite = minionManager.minionsDataBase[id].Idle;
            attackSprite = minionManager.minionsDataBase[id].Attack;
            thisSprite.sprite = idleSprite;
            double d = CurrentLevel > 0 ? (minionManager.minionsDataBase[id].DamageCoef * (0.2 * CurrentLevel)) : 0;
            DamageCoef = minionManager.minionsDataBase[id].DamageCoef + d;
            nameobj = minionManager.minionsDataBase[id].name;
            switch (name)
            {
                case "Minion1":
                    imageMinion1.sprite = minionManager.minionsDataBase[id].Preview;
                    string s = CurrentLevel > 0 ? $"+ {minionManager.minionsDataBase[id].DamageCoef * (0.2 * CurrentLevel) * 100}% " : "";
                    text1.text = $"{nameobj}\nLevel {CurrentLevel}\n{minionManager.minionsDataBase[id].DamageCoef * 100}% {s}of your damage";
                    break;
                case "Minion2":
                    imageMinion2.sprite = minionManager.minionsDataBase[id].Preview;
                    string s1 = CurrentLevel > 0 ? $"+ {minionManager.minionsDataBase[id].DamageCoef * (0.2 * CurrentLevel) * 100}% " : "";
                    text2.text = $"{nameobj}\nLevel {CurrentLevel}\n{minionManager.minionsDataBase[id].DamageCoef * 100}% {s1}of your damage";
                    break;
            }
            if (loaded)
            {
                minionManager.CheckUnfair();
            }
            loaded = true;
        }
        else
        {
            switch (name)
            {
                case "Minion1":
                    imageMinion1.sprite = minionManager.None;
                    text1.text = "";
                    break;
                case "Minion2":
                    imageMinion2.sprite = minionManager.None;
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
        UpdateMinionData();
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
            if (name == "Minion2")
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
                enemy.MinionSabotage();
            }
            else
            {
                enemy.MinionKick(clicker.Damage * DamageCoef);
            }
        }
    }
}
