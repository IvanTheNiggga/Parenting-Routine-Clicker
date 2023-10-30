using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public StagesManager stagesManager;
    public Clicker clicker;
    public UpgradesManager upgrades;
    public InterfaceManager interfaceManager;

    public Text currencyText;
    public Text experienceText;

    public Text stageText;

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
        BirthTextUpdate();
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

    public void BirthTextUpdate()
    {
        interfaceManager.UpdateUpgrades();
    }
}