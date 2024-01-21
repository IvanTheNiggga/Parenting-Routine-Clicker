using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    #region Spawn Settings
    public float enemySpawnInvoke = 0.2f;
    #endregion

    #region Local
    public GameObject[] BossList;
    public GameObject[] EnemyList;
    public GameObject Panel;
    public GameObject MoneyItem;
    private double _enemyHPMultiplier;
    public double EnemyHPMultiplier
    {
        get { return _enemyHPMultiplier; }
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                _enemyHPMultiplier = double.MaxValue / 100;
            }
            else
            {
                _enemyHPMultiplier = (value > double.MaxValue / 100) ? double.MaxValue / 100 : value;
            }
        }
    }

    private GameObject EnemyParent;
    public GameObject DropParent;
    private StagesManager stagesManager;
    private UpgradesManager upgradesManager;
    private RewardManager giveReward;
    private Inventory inventory;
    private InterfaceManager interfaceManager;
    private Slider hpSlider;
    private Clicker clicker;
    private Text text;

    public bool clickable;
    public bool able;
    #endregion

    #region Init
    void Start()
    {
        InitializeComponents();
        HideEnemyInf();
        EnemySpawn();
    }

    private void InitializeComponents()
    {
        upgradesManager = GetComponent<UpgradesManager>();
        giveReward = GetComponent<RewardManager>();
        inventory = GetComponent<Inventory>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        clicker = GetComponent<Clicker>();
        stagesManager = GetComponent <StagesManager>();

        EnemyParent = GameObject.Find("Enemy Parent");
        DropParent = GameObject.Find("Drop Parent");

        clickable = false;
        able = true;
        enemySpawnInvoke = 0.2f;

        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        text = GameObject.Find("HP(txt)").GetComponent<Text>();
    }
    #endregion

    #region Enemy Management
    public void EnemySpawn()
    {
        if (!able) Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        if (!clickable) Invoke(nameof(EnemySpawn), enemySpawnInvoke);

        if (EnemyParent.transform.childCount < 3)
        {

            int rnd = Random.Range(0, EnemyList.Length);
            Enemy enemyObj = Instantiate(EnemyList[rnd], EnemyParent.transform).GetComponent<Enemy>();

            ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();

            int xStart = Random.Range(0, 2) == 1 ? 200 : -200;
            enemyObj.transform.localPosition = new Vector2(xStart, 35);
            enemy_OM.xMoveTo(0, 0.2f, 1, false);
        }
    }

    public void EnemyDown()
    {
        Destroy(FindObjectOfType<Enemy>().gameObject);
        MoneyItem money = Instantiate(MoneyItem, DropParent.transform).GetComponent<MoneyItem>();
        money.count = 1 + upgradesManager.doubleCurrencyLvl;
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        giveReward.GetEnemyLoot();
    }

    public void RespawnEnemy()
    {
        Destroy(FindObjectOfType<Enemy>().gameObject);
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
    }
    #endregion

    #region Boss Management
    public void BossSpawnInv()
    {
        if (interfaceManager.minerOpened) interfaceManager.SwitchMiner(0);
        if (FindObjectOfType<Enemy>() != null)
        {
            CancelInvoke();
            Destroy(FindObjectOfType<Enemy>().gameObject);
            interfaceManager.SwitchUpgradesMenu(0);
            Invoke(nameof(BossSpawn), 1f);
        }
    }

    public void BossSpawn()
    {
        interfaceManager.SwitchBattleInterface(1);

        int rnd = Random.Range(0, BossList.Length);
        GameObject Boss = BossList[rnd];
        Enemy enemyObj = Instantiate(Boss, EnemyParent.transform).GetComponent<Enemy>();
        enemyObj.transform.localPosition = Boss.transform.localPosition;

        ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();

        int xStart = Random.Range(0, 2) == 1 ? 250 : -250;
        enemyObj.transform.localPosition = new Vector2(xStart, 35);
        enemy_OM.xMoveTo(0, 0.2f, 1, false);
        enemyObj.isBoss = true;
    }

    public void BossDown()
    {
        Destroy(FindObjectOfType<Enemy>().gameObject);
        stagesManager.NextStage();
        clicker.Save();
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        giveReward.GetBossLoot(20);
    }

    public void BossFailed()
    {
        Destroy(FindObjectOfType<Enemy>().gameObject);
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
    }
    #endregion
    public void HideEnemyInf()
    {
        hpSlider.value = 1;
        text.text = ("");
    }
    public void DestroyLoot()
    {
        int childCount = DropParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = DropParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}