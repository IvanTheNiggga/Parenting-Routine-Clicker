using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float enemySpawnInvoke;

    public GameObject Boss;
    public GameObject[] EnemyList;

    public GameObject Panel;
    public GameObject MoneyItem;

    private GameObject EnemyParent;
    private GameObject CurrencyParent;

    private StagesManager stagesManager;
    private GiveReward giveReward;
    private Inventory inventory;
    private Interface interfaceManager;
    private Slider hpSlider;
    private Clicker clicker;
    private Text text;

    public AudioClip enemySpawnSound;
    private AudioSource enemySpawnSource;


    public bool clickable;
    public bool able;

    public double EnemyHPMultiplier;


    void Start()
    {
        giveReward = GetComponent<GiveReward>();
        inventory = GetComponent<Inventory>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<Interface>();
        clicker = GetComponent<Clicker>();
        stagesManager = GetComponent<StagesManager>();

        EnemyParent = GameObject.Find("Enemy Parent");
        CurrencyParent = GameObject.Find("Currency Parent");

        clickable = false;
        able = true;

        enemySpawnInvoke = 0.2f;

        hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        text = GameObject.Find("HpTEXT").GetComponent<Text>();
        enemySpawnSource = GameObject.Find("EnemySpawnSource").GetComponent<AudioSource>();

        HideEnemyInf();
        Invoke(nameof(EnemySpawn), 1);

    }
    public void HideEnemyInf()
    {
        hpSlider.value = 1;
        hpSlider.maxValue = 1;

        text.text = ("There's nobody here :(");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Spawn/Die
    public void EnemySpawn()
    {
        if (able == true)
        {
            if (EnemyParent.transform.childCount == 2)
            {
                HideEnemyInf();

                enemySpawnSource.pitch = Random.Range(0.9f, 1.15f);
                enemySpawnSource.PlayOneShot(enemySpawnSound);

                int rnd = Random.Range(0, EnemyList.Length);
                Enemy enemyObj = Instantiate(EnemyList[rnd], EnemyParent.transform).GetComponent<Enemy>();

                ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();
                enemyObj.GetComponent<Injured>().startPos = new(0,35);

                int xStart = 200;
                if (Random.Range(0, 2) == 1)
                {
                    xStart = -xStart;
                }
                enemyObj.transform.localPosition = new Vector2(xStart, 35);
                enemy_OM.MoveTo(new Vector2(0, 35), 0.2f, 1, false);

                enemyObj.gameObject.name = "EnemyObj";
                enemyObj.HP *= EnemyHPMultiplier;
            }
            return;
        }
        if (clickable == false)
        {
            Invoke(nameof(EnemySpawn), enemySpawnInvoke);
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


    public void BossSpawnInv()
    {
        if (interfaceManager.washingMashineOpened)
        {
            interfaceManager.SwitchWashingmashine(0);
        }
        if (GameObject.Find("BossObj") == false)
        {
            HideEnemyInf();

            CancelInvoke();

            Destroy(GameObject.Find("EnemyObj"));

            interfaceManager.SwitchUpgradeMenu(0);

            Invoke(nameof(BossSpawn), 1f);
        }
    }
    public void BossSpawn()
    {
        interfaceManager.SwitchMainInterface(1);

        HideEnemyInf();

        enemySpawnSource.pitch = Random.Range(0.9f, 1.15f);
        enemySpawnSource.PlayOneShot(enemySpawnSound);

        Enemy enemyObj = Instantiate(Boss, EnemyParent.transform).GetComponent<Enemy>();
        enemyObj.transform.localPosition = Boss.transform.localPosition;

        ObjectMovement enemy_OM = enemyObj.GetComponent<ObjectMovement>();
        enemyObj.GetComponent<Injured>().startPos = new(0, 35);

        int xStart = 250;
        if (Random.Range(0, 2) == 1)
        {
            xStart = -xStart;
        }
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
}

