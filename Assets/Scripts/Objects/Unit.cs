using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    #region Appointed through the inspector
    public int id;
    [SerializeField] private Clicker clicker;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private ObjectMovement objectMovement;
    [SerializeField] private SpriteRenderer thisSprite;
    [SerializeField] private Image UnitInfoImage;
    public Text UnitInfoText;
    #endregion

    #region Variables
    public int CurrentLevel;
    public int unfairLvl;
    private Sprite idleSprite;
    private Sprite attackSprite;
    public double DamageCoef;
    public string nameobj;
    private bool loaded;
    private bool able;
    #endregion

    #region Unity Lifecycle and Initialization
    private void Start()
    {
        id = PlayerPrefs.GetInt(name + "ID");

        UpdateUnitData();
        switch (name)
        {
            case "Unit1(obj)":
                Invoke(nameof(MoveToEnemy), 2.5f);
                break;
            case "Unit2(obj)":
                Invoke(nameof(MoveToEnemy), 5f);
                break;
        }
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
    #endregion

    #region Update/Upgrade
    public void UpdateUnitData()
    {
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

            UnitInfoImage.sprite = unitManager.unitsDataBase[id].Preview;
            UnitInfoText.text = $"{nameobj} (Lv. {CurrentLevel})\n\nx{DamageCoef} from your damage.";

            if (loaded)
            {
                unitManager.CheckUnfair();
            }
            loaded = true;
        }
        else
        {
            UnitInfoImage.sprite = unitManager.None;
            UnitInfoText.text = "";

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
    #endregion

    #region Combat
    private void MoveToEnemy()
    {
        if (able == true && id >= 0 && enemyManager.clickable == true)
        {
            objectMovement.yMoveTo(enemy.transform.localPosition.y - 30, 0.2f, 1, true);
            Invoke(nameof(MoveBack), 0.5f);
        }
        Invoke(nameof(MoveToEnemy), 5);
    }

    private void MoveBack()
    {
        thisSprite.sprite = idleSprite;
        objectMovement.yMoveTo(-70, 0.2f, 1, false);
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
                enemy.DealDamage(clicker.Damage * DamageCoef);
            }
        }
    }
    #endregion
}
