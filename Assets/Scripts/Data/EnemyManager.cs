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
    private Message message;
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
        interfaceManager = FindAnyObjectByType<InterfaceManager>();
        clicker = GetComponent<Clicker>();
        stagesManager = GetComponent<StagesManager>();
        message = FindAnyObjectByType<Message>();

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

    public Enemy CurrentEnemy;
    public void EnemySpawn()
    {
        if (!able) Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        if (!clickable) Invoke(nameof(EnemySpawn), enemySpawnInvoke);

        if (EnemyParent.transform.childCount < 3)
        {

            int rnd = Random.Range(0, EnemyList.Length);
            CurrentEnemy = Instantiate(EnemyList[rnd], EnemyParent.transform).GetComponent<Enemy>();

            ObjectMovement enemy_OM = CurrentEnemy.GetComponent<ObjectMovement>();

            int xStart = Random.Range(0, 2) == 1 ? 200 : -200;
            CurrentEnemy.transform.localPosition = new Vector2(xStart, 35);
            enemy_OM.xMoveTo(0, 0.2f, 1, false);
        }
    }

    public void EnemyDown()
    {
        Destroy(CurrentEnemy.gameObject);
        MoneyItem money = Instantiate(MoneyItem, DropParent.transform).GetComponent<MoneyItem>();
        money.count = 1 + upgradesManager.DoubleCurrencyLvl;
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        giveReward.GetEnemyLoot();
    }

    public void RespawnEnemy()
    {
        Destroy(CurrentEnemy.gameObject);
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
    }
    #endregion

    #region Boss Management
    public void BossSpawnInv()
    {
        if (!CurrentEnemy) return;
        if (!CurrentEnemy.isBoss)
        {
            if (interfaceManager.minerOpened) interfaceManager.SwitchMiner(0);
            CancelInvoke();
            Destroy(CurrentEnemy.gameObject);
            interfaceManager.SwitchUpgradesMenu(0);
            Invoke(nameof(BossSpawn), 1f);
        }
        else
        {
            CurrentEnemy.StopBossTimer();
            BossFailed();
        }
    }

    public void BossSpawn()
    {
        interfaceManager.SwitchBattleInterface(1);

        int rnd = Random.Range(0, BossList.Length);
        GameObject Boss = BossList[rnd];
        CurrentEnemy = Instantiate(Boss, EnemyParent.transform).GetComponent<Enemy>();
        CurrentEnemy.transform.localPosition = Boss.transform.localPosition;

        ObjectMovement enemy_OM = CurrentEnemy.GetComponent<ObjectMovement>();

        int xStart = Random.Range(0, 2) == 1 ? 250 : -250;
        CurrentEnemy.transform.localPosition = new Vector2(xStart, 35);
        enemy_OM.xMoveTo(0, 0.2f, 1, false);
        CurrentEnemy.isBoss = true;
    }

    public void BossDown()
    {
        Destroy(CurrentEnemy.gameObject);
        stagesManager.NextStage();
        clicker.Save();
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        giveReward.GetBossLoot(20);
    }

    public void BossFailed()
    {
        Destroy(CurrentEnemy.gameObject);
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        message.SendMessage("Boss failed", 1);
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