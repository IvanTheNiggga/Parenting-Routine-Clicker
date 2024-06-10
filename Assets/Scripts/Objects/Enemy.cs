using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
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

    private AudioSource audioSource;

    private GameObject ClickParent;
    private Text hpText;
    private Slider hpSlider;

    private Unit unit1;
    private Unit unit2;

    private int addCrit;
    private double startHp;

    private float cooldown;
    #endregion

    #region Variables
    [SerializeField] private double _hp;
    public double HP
    {
        get { return _hp; }
        set
        {
            if (value <= 0)
            {
                clicker.LastDealedDamage = _hp;
                _hp = 0;

                onHurt.Kicked(true);
                HandleDeath();
            }
            else
            {
                hpSlider.value = (float)(value / startHp);
                hpText.text = NumFormat.FormatNumF1(value) + " / " + NumFormat.FormatNumF1(startHp);

                if(value < _hp)
                {
                    onHurt.Kicked(false);
                }
                _hp = value;
            }
        }
    }
    public bool isBoss;
    #endregion

    private void FixedUpdate() => cooldown += Time.deltaTime;

    #region Init and Start
    private void Start()
    {
        InitializeComponents();

        audioSource.pitch = Random.Range(0.9f, 1.15f);
        audioSource.PlayOneShot(spawnAudioClip);

        startHp = HP * enemyManager.EnemyHPMultiplier;
        HP = startHp;
        addCrit = upgradesManager.CritDamageLvl;

        hpSlider.maxValue = 1;
        hpSlider.value = (float)(HP / startHp);

        clickable.EnemyGetComponent();
        if (isBoss) Invoke(nameof(OnBossStart), 1f);
        else Invoke(nameof(OnStart), 0.4f);
    }

    private void InitializeComponents()
    {
        audioSource = GetComponent<AudioSource>();
        onHurt = GetComponent<OnHurt>();

        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();

        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        ClickParent = GameObject.Find("Clickable(cdr)");
        clickable = ClickParent.GetComponent<Panel>();

        unit1 = GameObject.Find("Unit1(obj)").GetComponent<Unit>();
        unit2 = GameObject.Find("Unit2(obj)").GetComponent<Unit>();

        hpText = GameObject.Find("HP(txt)").GetComponent<Text>();
        hpText.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
    }

    private void AssignUnits()
    {
        if (unit1) unit1.GetEnemyComponent(this);
        if (unit2) unit2.GetEnemyComponent(this);
    }
    private void ClearUnits()
    {
        if (unit1) unit1.GetEnemyComponent(null);
        if (unit2) unit2.GetEnemyComponent(null);
    }

    public void OnStart()
    {
        enemyManager.clickable = true;
        AssignUnits();
    }
    public void OnBossStart()
    {
        enemyManager.clickable = true;
        AssignUnits();
        timer.StartTimer(20);
    }
    #endregion

    #region Damaging Behaviour
    public void UnitSabotage()
    {
        HP += startHp / 2;
        hpSlider.value = 1;

        hpText.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

        onHurt.Sabotaged();
    }

    public void Kick()
    {
        if (cooldown < 0.05f) return;
        cooldown = 0;

        DealDamage(CalculateDamage());

        clicker.Experience += 0.05f * (1 + upgradesManager.DoubleXPLvl);
    }

    private double CalculateDamage()
    {
        double damage;
        if (Random.Range(0, 100 / clicker.CritChance) <= 0) damage = clicker.Damage * (clicker.CritMultiplier + addCrit);
        else damage = clicker.Damage;
        return damage;
    }
    #endregion

    #region Handlers
    public void DealDamage(double damage)
    {
        clicker.LastDealedDamage = damage;
        HP -= damage;
        hpSlider.value = (float)(HP / startHp);
        hpText.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

        DamagePart part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<DamagePart>();
        part.big = clicker.Damage < damage;
    }

    private void HandleDeath()
    {
        ClearUnits();
        if (isBoss)
        {
            StopBossTimer();
            enemyManager.BossDown();
        }
        else enemyManager.EnemyDown();
    }
    public void StopBossTimer()
    {
        timer.PauseTimer();
        timer.ClearTimer();
    }
    #endregion

    private void OnDestroy()
    {
        enemyManager.HideEnemyInf();
        enemyManager.clickable = false;
        enemyManager.CurrentEnemy = null;
    }
}