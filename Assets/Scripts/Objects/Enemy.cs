using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Clicker clicker;
    private EnemyManager enemyManager;
    private OnHurt sprSwap;
    private Timer timer;

    public GameObject DmgPart_Prefab;
    public GameObject ClickParent;
    public Text text;
    public Slider hpSlider;
    public Unit unit1;
    public Unit unit2;

    public double startHp;
    public double HP;

    private int addCrit;
    private bool isBoss;

    private void Start()
    {
        isBoss = name == "BossObj";
        startHp = HP;

        InitializeComponents();

        if (isBoss)
            Invoke(nameof(BossAbleToAttack), 1f);
        else
            Invoke(nameof(AbleToAttack), 0.4f);
    }

    private void InitializeComponents()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        addCrit = clicker.critMultiplierUpgradeLvl;

        var clickable = GameObject.Find("Clickable(cdr)").GetComponent<Panel>();
        clickable.EnemyGetComponent(name);

        text = GameObject.Find("HP(txt)").GetComponent<Text>();
        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        hpSlider.maxValue = 1;
        hpSlider.value = (float)(HP / startHp);

        sprSwap = GetComponent<OnHurt>();
        ClickParent = GameObject.Find("Clickable(cdr)");
    }

    public void AbleToAttack()
    {
        AssignUnits();
        enemyManager.clickable = true;
    }

    private void AssignUnits()
    {
        if (unit1 != null) unit1.GetEnemyComponent(this);
        if (unit2 != null) unit2.GetEnemyComponent(this);
    }

    public void BossAbleToAttack()
    {
        enemyManager.clickable = true;
        timer.StartTimer(isBoss ? 20 : 0);
    }

    public void UnitKick(double damage)
    {
        sprSwap.Kicked();

        if (HP <= damage)
        {
            HandleDeath(damage);
        }
        else
        {
            HandleDamageDealt(damage);
        }

        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
        CreateDmgPart(false);
        CheckHP();
    }

    public void UnitSabotage()
    {
        HP += startHp / 2;
        hpSlider.value = 1;
        sprSwap.Kicked();
        CheckHP();
    }

    public void Kick()
    {
        sprSwap.Kicked();

        if (IsCriticalHit())
        {
            HandleCriticalHit();
        }
        else
        {
            HandleNormalHit();
        }

        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
        CreateDmgPart(false);
        clicker.Experience += 0.05f;
        CheckHP();
    }

    private void HandleDeath(double damage)
    {
        clicker.CurrDealedDamage = HP;
        HP = 0;
        hpSlider.value = 0;
    }

    private void HandleDamageDealt(double damage)
    {
        clicker.CurrDealedDamage = damage;
        HP -= damage;
        hpSlider.value = (float)(HP / startHp);
    }

    private void HandleCriticalHit()
    {
        if (HP < (clicker.Damage * clicker.CritMultiplier))
        {
            HandleDeath(HP);
        }
        else
        {
            clicker.CurrDealedDamage = clicker.Damage * (clicker.CritMultiplier + addCrit);
            HP -= clicker.Damage * (clicker.CritMultiplier + addCrit);
            hpSlider.value = (float)(HP / startHp);
            CreateDmgPart(true);
        }
    }

    private void HandleNormalHit()
    {
        if (HP <= clicker.Damage)
        {
            HandleDeath(HP);
        }
        else
        {
            HandleDamageDealt(clicker.Damage);
        }
    }

    private bool IsCriticalHit()
    {
        return Random.Range(0, 100 / clicker.CritChance) <= 0;
    }

    private void CreateDmgPart(bool big)
    {
        DamagePart part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<DamagePart>();

        part.big = big;
        Vector2 v = transform.position;
        part.transform.position = new Vector2(v.x + 25, v.y);
    }

    private void CheckHP()
    {
        if (HP <= 0.5f)
        {
            ClearUnits();
            if (isBoss)
            {
                UnableToAttack();
                timer.PauseTimer();
                timer.ClearTimer();
                enemyManager.BossDown();
            }
            else
            {
                UnableToAttack();
                enemyManager.EnemyDown();
            }
        }
    }

    private void ClearUnits()
    {
        if (unit1 != null) unit1.GetEnemyComponent(null);
        if (unit2 != null) unit2.GetEnemyComponent(null);
    }

    private void UnableToAttack()
    {
        enemyManager.HideEnemyInf();
        enemyManager.clickable = false;
    }
}