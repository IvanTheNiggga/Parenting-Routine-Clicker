using UnityEngine;
using UnityEngine.UI;


public class TextManager : MonoBehaviour
{
    public StagesManager stagesManager;
    public Clicker clicker;
    public Upgrades upgrades;

    public Text currencyText;
    public Text experienceText;

    public Text stageText;

    public Text critCostText;
    public Text critDescText;
    public Text critLvlText;

    public Text dmgCostText;
    public Text dmgDescText;
    public Text dmgLvlText;

    private void Start()
    {
        currencyText.transform.localPosition = new(currencyText.transform.localPosition.x, currencyText.transform.localPosition.y - Screen.safeArea.yMin / 2, currencyText.transform.localPosition.z);
        experienceText.transform.localPosition = new(experienceText.transform.localPosition.x, experienceText.transform.localPosition.y - Screen.safeArea.yMin / 2, experienceText.transform.localPosition.z);
        Invoke(nameof(UpdateAllText), 0.5f);
    }

    public void UpdateAllText()
    {
        StageTextUpdate(); CurrencyTextUpdate(); ExpirienceTextUpdate(); CritUpgradeTextUpdate(); DmgUpgradeTextUpdate();
    }

    public void StageTextUpdate()
    {
        stageText.text = $"Stage :  {stagesManager.CurrentStage}";
    }
    public void CurrencyTextUpdate()
    {
        currencyText.text = FormatNumsHelper.FormatNumF1(clicker.Currency);
    }
    public void ExpirienceTextUpdate()
    {
        experienceText.text = FormatNumsHelper.FormatNumF1(clicker.Experience);
    }
    public void CritUpgradeTextUpdate()
    {
        critCostText.text = $"${FormatNumsHelper.FormatNumF0F1(clicker.CritCost)}";
        critDescText.text = $"{clicker.CritChance}% + 1% to deal x{clicker.CritMultiplier} damage";
        critLvlText.text = $"lv.{upgrades.CritLvl}";
        if (clicker.CritChance > 49)
        {
            critLvlText.text = $"max";
        }
    }
    public void DmgUpgradeTextUpdate()
    {
        dmgCostText.text = $"${FormatNumsHelper.FormatNumF0F1(clicker.DmgCost)}";
        dmgDescText.text = $"x1.6 your {FormatNumsHelper.FormatNumF0F1(clicker.Damage)} damage";
        dmgLvlText.text = $"lv.{upgrades.DamageLvl}";
        if (upgrades.DamageLvl > 1499)
        {
            dmgLvlText.text = $"max";
        }
    }
}
