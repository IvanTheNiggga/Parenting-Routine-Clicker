using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Appointed through the inspector
    public AudioClip spawnAudioClip;
    public GameObject DmgPart_Prefab;
    #endregion

    #region Local
    private Panel clickable;
    private Clicker clicker;
    private UpgradesManager upgradesManager;
    private EnemyManager enemyManager;
    private OnHurt onHurt;
    private Timer timer;

    private GameObject ClickParent;
    private Text text;
    private Slider hpSlider;
    private Unit unit1;
    private Unit unit2;
    private int addCrit;
    private double startHp;

    private float cooldown;
    #endregion

    #region Variables
    public double HP;
    public bool isBoss;
    #endregion

    #region Init
    private void Start()
    {
        AudioSource enemySpawnSource = GameObject.Find("EnemySpawnSource").GetComponent<AudioSource>();
        enemySpawnSource.pitch = Random.Range(0.9f, 1.15f);
        enemySpawnSource.PlayOneShot(spawnAudioClip);
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        addCrit = upgradesManager.critDamageLvl;

        if (isBoss)
        {
            HP = 400 * enemyManager.EnemyHPMultiplier;
            Invoke(nameof(BossAbleToAttack), 1f);
        }
        else
        {
            HP = 15 * enemyManager.EnemyHPMultiplier;
            Invoke(nameof(AbleToAttack), 0.4f);
        }
        startHp = HP;

        if (GameObject.Find("Clickable(cdr)") != null)
        {
            clickable = GameObject.Find("Clickable(cdr)").GetComponent<Panel>();
            clickable.EnemyGetComponent();
        }

        unit1 = GameObject.Find("Unit1(obj)").GetComponent<Unit>();
        unit2 = GameObject.Find("Unit2(obj)").GetComponent<Unit>();

        text = GameObject.Find("HP(txt)").GetComponent<Text>();
        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        hpSlider.maxValue = 1;
        hpSlider.value = (float)(HP / startHp);

        onHurt = GetComponent<OnHurt>();
        ClickParent = GameObject.Find("Clickable(cdr)");
    }

    private void AssignUnits()
    {
        if (unit1 != null) unit1.GetEnemyComponent(this);
        if (unit2 != null) unit2.GetEnemyComponent(this);
    }
    private void ClearUnits()
    {
        if (unit1 != null) unit1.GetEnemyComponent(null);
        if (unit2 != null) unit2.GetEnemyComponent(null);
    }
    #endregion

    #region Enemy Control
    public void AbleToAttack()
    {
        enemyManager.clickable = true;
        AssignUnits();
    }
    public void BossAbleToAttack()
    {
        enemyManager.clickable = true;
        AssignUnits();
        timer.StartTimer(isBoss ? 20 : 0);
    }
    #endregion

    #region Damaging Behaviour
    public void UnitKick(double damage)
    {
        if (clickable == null)
        {
            clickable = GameObject.Find("Clickable(cdr)").GetComponent<Panel>();
            clickable.EnemyGetComponent();
        }

        clicker.CurrDealedDamage = damage;
        HP -= damage;
        hpSlider.value = (float)(HP / startHp);

        DamagePart part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<DamagePart>();
        part.big = false;
        Vector2 v = transform.position;
        part.transform.position = new Vector2(v.x + 25, v.y);

        if (HP <= damage)
        {
            onHurt.Kicked(true);
            HandleDeath();
        }
        else onHurt.Kicked(false);

        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
        CheckHP();
    }

    public void UnitSabotage()
    {
        HP += startHp / 2;
        hpSlider.value = 1;
        CheckHP();
    }

    private void FixedUpdate()=> cooldown += Time.deltaTime;
    public void Kick()
    {
        if (cooldown < 0.03) return;
        cooldown = 0;
        double damage;
        if (Random.Range(0, 100 / clicker.CritChance) <= 0) damage = clicker.Damage * (clicker.CritMultiplier + addCrit);
        else damage = clicker.Damage;
        HandleDamageDealt(damage);

        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
        clicker.Experience += 0.05f * (1 + upgradesManager.doubleXPLvl);
        CheckHP();
    }
    #endregion

    #region Handlers

    private void HandleDamageDealt(double damage)
    {
        clicker.CurrDealedDamage = damage;

        if (HP <= damage)
        {
            clicker.CurrDealedDamage = HP;
            onHurt.Kicked(true);
            HandleDeath(); 
        }
        else onHurt.Kicked(false);

        HP -= damage;
        hpSlider.value = (float)(HP / startHp);

        DamagePart part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<DamagePart>();
        part.big = clicker.Damage < damage;
        Vector2 v = transform.position;
        part.transform.position = new Vector2(v.x + 25, v.y);
    }

    private void HandleDeath()
    {
        clicker.CurrDealedDamage = HP;
        HP = 0;
        hpSlider.value = 0;
    }
    private void CheckHP()
    {
        if (HP <= 0.5f)
        {
            ClearUnits();
            if (isBoss)
            {
                timer.PauseTimer();
                timer.ClearTimer();
                enemyManager.BossDown();
            }
            else
            {
                enemyManager.EnemyDown();
            }
        }
    }
    #endregion

    private void OnDestroy()
    {
        enemyManager.HideEnemyInf();
        enemyManager.clickable = false;
    }
}