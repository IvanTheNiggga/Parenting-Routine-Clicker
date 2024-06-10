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
    [SerializeField] private Image UnitInfoImage;
    public Text UnitInfoText;
    #endregion

    #region Variables
    public int CurrentLevel;
    public int unfairLvl;
    public double DamageCoef;
    public string nameobj;
    private bool loaded;
    #endregion

    #region Unity Lifecycle and Initialization
    private void Start()
    {
        id = PlayerPrefs.GetInt(name + "ID");

        UpdateUnitData();
        switch (name)
        {
            case "Unit1(obj)":
                InvokeRepeating(nameof(Attack), 1.5f, 1);
                break;
            case "Unit2(obj)":
                InvokeRepeating(nameof(Attack), 2, 1);
                break;
        }
    }
    public void GetEnemyComponent(Enemy enemyObj)
    {
        enemy = enemyObj;
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
            UnitInfoText.text = "No minion selected.";

            PlayerPrefs.SetInt(name + "ID", id);
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

    public void Attack()
    {
        if (enemy && id >= 0 && enemyManager.clickable == true)
        {
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
