using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    #region Spawn Settings
    public float enemySpawnInvoke = 0.2f;
    #endregion

    #region Local
    public GameObject Boss;
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
    private GameObject CurrencyParent;
    private StagesManager stagesManager;
    private RewardManager giveReward;
    private Inventory inventory;
    private InterfaceManager interfaceManager;
    private Slider hpSlider;
    private Clicker clicker;
    private Text text;

    public AudioClip enemySpawnSound;
    private AudioSource enemySpawnSource;

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
        giveReward = GetComponent<RewardManager>();
        inventory = GetComponent<Inventory>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        clicker = GetComponent<Clicker>();
        stagesManager = GetComponent <StagesManager>();

        EnemyParent = GameObject.Find("Enemy Parent");
        CurrencyParent = GameObject.Find("Drop Parent");

        clickable = false;
        able = true;
        enemySpawnInvoke = 0.2f;

        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        text = GameObject.Find("HP(txt)").GetComponent<Text>();
        enemySpawnSource = GameObject.Find("EnemySpawnSource").GetComponent<AudioSource>();
    }
    #endregion

    #region Enemy Management
    public void EnemySpawn()
    {
        if (!able) Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        if (!clickable) Invoke(nameof(EnemySpawn), enemySpawnInvoke);

        if (EnemyParent.transform.childCount < 3)
        {
            HideEnemyInf();

            enemySpawnSource.pitch = Random.Range(0.9f, 1.15f);
            enemySpawnSource.PlayOneShot(enemySpawnSound);

            int rnd = Random.Range(0, EnemyList.Length);
            Enemy enemyObj = Instantiate(EnemyList[rnd], EnemyParent.transform).GetComponent<Enemy>();

            ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();
            enemyObj.GetComponent<OnHurt>().startPos = new Vector2(0, 35);

            int xStart = Random.Range(0, 2) == 1 ? 200 : -200;
            enemyObj.transform.localPosition = new Vector2(xStart, 35);
            enemy_OM.MoveTo(new Vector2(0, 35), 0.2f, 1, false);

            enemyObj.gameObject.name = "EnemyObj";
            enemyObj.HP *= EnemyHPMultiplier;
        }
    }

    public void EnemyDown()
    {
        HideEnemyInf();
        Destroy(GameObject.Find("EnemyObj"));
        MoneyItem mi = Instantiate(MoneyItem, CurrencyParent.transform).GetComponent<MoneyItem>();
        mi.objectName = "Enemy";
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        giveReward.GetEnemyLoot();
    }

    public void RespawnEnemy()
    {
        Destroy(GameObject.Find("EnemyObj"));
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
    }
    #endregion

    #region Boss Management
    public void BossSpawnInv()
    {
        if (interfaceManager.minerOpened) interfaceManager.SwitchMiner(0);
        if (GameObject.Find("BossObj") == null)
        {
            HideEnemyInf();
            CancelInvoke();
            Destroy(GameObject.Find("EnemyObj"));
            interfaceManager.SwitchUpgradesMenu(0);
            Invoke(nameof(BossSpawn), 1f);
        }
    }

    public void BossSpawn()
    {
        interfaceManager.SwitchBattleInterface(1);
        HideEnemyInf();

        enemySpawnSource.pitch = Random.Range(0.9f, 1.15f);
        enemySpawnSource.PlayOneShot(enemySpawnSound);

        Enemy enemyObj = Instantiate(Boss, EnemyParent.transform).GetComponent<Enemy>();
        enemyObj.transform.localPosition = Boss.transform.localPosition;

        ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();
        enemyObj.GetComponent<OnHurt>().startPos = new Vector2(0, 35);

        int xStart = Random.Range(0, 2) == 1 ? 250 : -250;
        enemyObj.transform.localPosition = new Vector2(xStart, 35);
        enemy_OM.MoveTo(new Vector2(0, 35), 0.2f, 1, false);

        enemyObj.gameObject.name = "BossObj";
        enemyObj.HP = 400 * EnemyHPMultiplier;
    }

    public void BossDown()
    {
        HideEnemyInf();
        Destroy(GameObject.Find("BossObj"));
        stagesManager.NextStage();
        clicker.Save();
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        MoneyItem mi = Instantiate(MoneyItem, CurrencyParent.transform).GetComponent<MoneyItem>();
        mi.count = 5;
        mi.objectName = "Boss";
        giveReward.GetBossLoot(20);
    }

    public void BossFailed()
    {
        Destroy(GameObject.Find("BossObj"));
        Invoke(nameof(EnemySpawn), enemySpawnInvoke);
        HideEnemyInf();
    }
    #endregion
    public void HideEnemyInf()
    {
        hpSlider.value = 1;
        hpSlider.maxValue = 1;
        text.text = ("");
    }

}