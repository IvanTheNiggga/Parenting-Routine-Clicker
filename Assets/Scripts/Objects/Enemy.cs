using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Clicker clicker;
    private EnemyManager enemyManager;
    private Injured sprSwap;
    private Panel panel;
    private Timer timer;

    public GameObject DmgPart_Prefab;
    public GameObject ClickParent;

    private Text text;
    private Slider hpSlider;

    private Minion minion1;
    private Minion minion2;

    public double startHp;
    public double HP;

    private int addCrit;

    private void Start()
    {
        startHp = HP;

        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        addCrit = clicker.critMultiplierUpgradeLvl;
        panel = GameObject.Find("Click Panel").GetComponent<Panel>();
        panel.EnemyGetComponent(name);
        text = GameObject.Find("HpTEXT").GetComponent<Text>();
        text.text = FormatNumsHelper.FormatNumF1(HP) + " / " + FormatNumsHelper.FormatNumF1(startHp);
        hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        hpSlider.maxValue = 1;
        hpSlider.value = (float)(HP / startHp);

        sprSwap = GetComponent<Injured>();
        minion1 = GameObject.Find("Minion1").GetComponent<Minion>();
        minion2 = GameObject.Find("Minion2").GetComponent<Minion>();
        ClickParent = GameObject.Find("Click Parent");
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();

        timer = GameObject.Find("Timer").GetComponent<Timer>();

        Invoke(nameof(AbleToAttack), 0.4f);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Preparation
    public void AbleToAttack()
    {
        if (minion1 != null)
        {
            minion1.GetEnemyComponent(this);
        }
        if (minion2 != null)
        {
            minion2.GetEnemyComponent(this);
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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Minion actions
    public void MinionKick(double damage)
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

        text.text = (FormatNumsHelper.FormatNumF1(HP) + " / " + FormatNumsHelper.FormatNumF1(startHp));

        if (ClickParent.transform.childCount < 13)
        {
            CreateDmgPart(false);
        }

        CheckHP();
        return;
    }
    public void MinionSabotage()
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

            text.text = FormatNumsHelper.FormatNumF1(HP) + " / " + FormatNumsHelper.FormatNumF1(startHp);

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

        text.text = FormatNumsHelper.FormatNumF1(HP) + " / " + FormatNumsHelper.FormatNumF1(startHp);

        CheckHP();
    }
    void CreateDmgPart(bool big)
    {
        ClickObj part = Instantiate(DmgPart_Prefab, ClickParent.transform).GetComponent<ClickObj>();

        part.big = big;
        Vector2 v = transform.position;
        part.transform.position = new Vector2(v.x + 25, v.y);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// HP
    private void CheckHP()
    {
        if (HP <= 0.5f)
        {
            if (minion1 != null)
            {
                minion1.GetEnemyComponent(null);
            }
            if (minion2 != null)
            {
                minion2.GetEnemyComponent(null);
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
        if (minion1 != null)
        {
            minion1.GetEnemyComponent(null);
        }
        if (minion2 != null)
        {
            minion2.GetEnemyComponent(null);
        }
    }
}
