using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public StagesManager stagesManager;
    public Clicker clicker;
    public UpgradesManager upgrades;

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
        AdjustUITextPosition();
        Invoke(nameof(UpdateAllText), 0.5f);
    }

    private void AdjustUITextPosition()
    {
        float safeAreaOffsetY = Screen.safeArea.yMin / 2;

        currencyText.rectTransform.localPosition -= new Vector3(0f, safeAreaOffsetY, 0f);
        experienceText.rectTransform.localPosition -= new Vector3(0f, safeAreaOffsetY, 0f);
    }

    public void UpdateAllText()
    {
        StageTextUpdate();
        CurrencyTextUpdate();
        ExperienceTextUpdate();
        UpdateCritUpgradeText();
        UpdateDamageUpgradeText();
    }

    public void StageTextUpdate()
    {
        stageText.text = $"Stage: {stagesManager.CurrentStage}";
    }

    public void CurrencyTextUpdate()
    {
        currencyText.text = NumFormat.FormatNumF1(clicker.Currency);
    }

    public void ExperienceTextUpdate()
    {
        experienceText.text = NumFormat.FormatNumF1(clicker.Experience);
    }

    public void UpdateCritUpgradeText()
    {
        critCostText.text = $"${NumFormat.FormatNumF0F1(clicker.CritCost)}";
        critDescText.text = $"{clicker.CritChance}% + 1% to deal x{clicker.CritMultiplier} damage";
        critLvlText.text = clicker.CritChance > 49 ? "max" : $"lv.{upgrades.CritLvl}";
    }

    public void UpdateDamageUpgradeText()
    {
        dmgCostText.text = $"${NumFormat.FormatNumF0F1(clicker.DmgCost)}";
        dmgDescText.text = $"x1.6 your {NumFormat.FormatNumF0F1(clicker.Damage)} damage";
        dmgLvlText.text = upgrades.DamageLvl > 1499 ? "max" : $"lv.{upgrades.DamageLvl}";
    }
}