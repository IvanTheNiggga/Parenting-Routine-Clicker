using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Clicker clicker;
    private EnemyManager enemyManager;
    private OnHurt sprSwap;
    private Panel panel;
    private Timer timer;

    public GameObject DmgPart_Prefab;
    public GameObject ClickParent;

    private Text text;
    private Slider hpSlider;

    private Unit unit1;
    private Unit unit2;

    public double startHp;
    public double HP;

    private int addCrit;

    private void Start()
    {
        startHp = HP;

        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        addCrit = clicker.critMultiplierUpgradeLvl;
        panel = GameObject.Find("Clickable(cdr)").GetComponent<Panel>();
        panel.EnemyGetComponent(name);
        text = GameObject.Find("HP(txt)").GetComponent<Text>();
        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);
        hpSlider = GameObject.Find("HP(sld)").GetComponent<Slider>();
        hpSlider.maxValue = 1;
        hpSlider.value = (float)(HP / startHp);

        sprSwap = GetComponent<OnHurt>();
        unit1 = GameObject.Find("Unit1(obj)").GetComponent<Unit>();
        unit2 = GameObject.Find("Unit2(obj)").GetComponent<Unit>();
        ClickParent = GameObject.Find("Clickable(cdr)");
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();

        timer = GameObject.Find("Timer").GetComponent<Timer>();

        Invoke(nameof(AbleToAttack), 0.4f);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Preparation
    public void AbleToAttack()
    {
        if (unit1 != null)
        {
            unit1.GetEnemyComponent(this);
        }
        if (unit2 != null)
        {
            unit2.GetEnemyComponent(this);
        }

        if (name == ("EnemyObj"))
        {
            enemyManager.clickable = true;
        }
        else if (name == ("BossObj"))
        {
            Invoke(nameof(BossAbleToAttack), 1f);
        }
    }
    public void UnableToAttack()
    {
        enemyManager.HideEnemyInf();
        enemyManager.clickable = false;
    }

    public void BossAbleToAttack()
    {
        enemyManager.clickable = true;
        timer.StartTimer(20);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Unit actions
    public void UnitKick(double damage)
    {
        sprSwap.SetInjured();
        sprSwap.Kicked();

        if (HP <= damage)
        {
            clicker.CurrDealedDamage = HP;
            HP = 0;
            hpSlider.value = 0;
        }
        else
        {
            clicker.CurrDealedDamage = damage;
            HP -= damage;
            hpSlider.value = (float)(HP / startHp);
        }

        text.text = (NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp));

        if (ClickParent.transform.childCount < 13)
        {
            CreateDmgPart(false);
        }

        CheckHP();
        return;
    }
    public void UnitSabotage()
    {
        HP += startHp / 2;
        hpSlider.value = 1;

        sprSwap.SetInjured();
        sprSwap.Kicked();
        CheckHP();
        return;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Attack
    public void Kick()
    {
        sprSwap.SetInjured();
        sprSwap.Kicked();

        if (Random.Range(0, 100 / clicker.CritChance) <= 0)
        {
            KickCrit();
            return;
        }
        else
        {
            if (HP <= clicker.Damage)
            {
                clicker.CurrDealedDamage = HP;
                HP = 0;
                hpSlider.value = 0;
            }
            else
            {
                clicker.CurrDealedDamage = clicker.Damage;
                HP -= clicker.Damage;
                hpSlider.value = (float)(HP / startHp);
            }

            text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

            if (ClickParent.transform.childCount < 13)
            {
                CreateDmgPart(false);
            }

            clicker.Experience += 0.05f;
            CheckHP();
            return;
        }
    }
    private void KickCrit()
    {
        if (HP < (clicker.Damage * clicker.CritMultiplier))
        {
            clicker.CurrDealedDamage = HP;
            HP = 0;
            hpSlider.value = 0;
        }
        else
        {
            clicker.CurrDealedDamage = clicker.Damage * (clicker.CritMultiplier + addCrit);
            HP -= clicker.Damage * (clicker.CritMultiplier + addCrit);
            hpSlider.value = (float)(HP / startHp);
        }

        CreateDmgPart(true);

        text.text = NumFormat.FormatNumF1(HP) + " / " + NumFormat.FormatNumF1(startHp);

        CheckHP();
    }
    void CreateDmgPart(bool big)
    {
        DamagePart part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<DamagePart>();

        part.big = big;
        Vector2 v = transform.position;
        part.transform.position = new Vector2(v.x + 25, v.y);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// HP
    private void CheckHP()
    {
        if (HP <= 0.5f)
        {
            if (unit1 != null)
            {
                unit1.GetEnemyComponent(null);
            }
            if (unit2 != null)
            {
                unit2.GetEnemyComponent(null);
            }

            if (name == "BossObj")
            {
                UnableToAttack();
                timer.PauseTimer();
                timer.ClearTimer();
                enemyManager.BossDown();

            }
            else if (name == "EnemyObj")
            {
                UnableToAttack();
                enemyManager.EnemyDown();
            }
        }
    }

    private void OnDestroy()
    {
        if (unit1 != null)
        {
            unit1.GetEnemyComponent(null);
        }
        if (unit2 != null)
        {
            unit2.GetEnemyComponent(null);
        }
    }
}
