using UnityEngine;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour
{
    #region Appointed through the inspector
    [SerializeField] private Text upgradeDescriptionText;
    [SerializeField] private Text lvlText;
    [SerializeField] private Text priceText;

    [SerializeField] private Image upgradeIco;
    [SerializeField] private Image currencyIco;
    [SerializeField] private Sprite moneyIco;
    [SerializeField] private Sprite xpIco;
    [SerializeField] private Sprite birthIco;
    #endregion

    #region Appointed on start
    private Clicker clicker;
    private UpgradesManager upgradesManager;
    private Message message;
    private SoundManager soundManager;
    private StagesManager stagesManager;
    private InterfaceManager interfaceManager;
    #endregion

    #region Variables
    public int Index;
    private int currentLvl;
    private UpgradeTypes type;
    private double lvlPrice;
    private int maxLvl;
    private float stepCoef;
    private string upgradeName;
    #endregion


    private bool loaded;
    public void AddGraphics()
    {
        if (!loaded)
        {
            clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
            upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
            interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
            message = GameObject.Find("Message").GetComponent<Message>();
            name = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>().UpgradesDataBase[Index].upgradeName;
            stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

            loaded = true;

            upgradeName = upgradesManager.UpgradesDataBase[Index].upgradeName;
            name = upgradeName;
            type = upgradesManager.UpgradesDataBase[Index].type;
            switch (type)
            {
                case UpgradeTypes.Xp:
                    currencyIco.sprite = xpIco;
                    break;
                case UpgradeTypes.Birth:
                    currencyIco.sprite = birthIco;
                    break;
                case UpgradeTypes.Money:
                    currencyIco.sprite = moneyIco;
                    break;
            }

            currentLvl = PlayerPrefs.GetInt(upgradeName);
            maxLvl = upgradesManager.UpgradesDataBase[Index].maxLvl;
            stepCoef = upgradesManager.UpgradesDataBase[Index].stepCoef;

            upgradeIco.sprite = upgradesManager.UpgradesDataBase[Index].upgradeIco;
            upgradeDescriptionText.text = upgradesManager.UpgradesDataBase[Index].upgradeDescription;
        }

        switch (upgradeName)
        {
            case "BetterStartUpgrade":
                maxLvl = stagesManager.maxStage - 20;
                break;
            case "BetterMineAfterRebirthUpgrade":
                maxLvl = clicker.MaxMinerLvl - 10;
                break;
        }
        switch (stepCoef)
        {
            case 1:
                lvlPrice = upgradesManager.UpgradesDataBase[Index].lvlPrice + (currentLvl * upgradesManager.UpgradesDataBase[Index].lvlPrice);
                break;
            case 0:
                lvlPrice = upgradesManager.UpgradesDataBase[Index].lvlPrice;
                break;
            default:
                lvlPrice = Utils.Progression(upgradesManager.UpgradesDataBase[Index].lvlPrice, stepCoef, currentLvl);
                break;
        }

        if (currentLvl >= maxLvl)
        {
            priceText.text = "Max level."; lvlPrice = 0;
        }
        else
        {
            if (type == UpgradeTypes.Birth)
            {
                priceText.text = clicker.Births.ToString() + "/" + NumFormat.FormatNumF0F1(lvlPrice);
            }
            else
            {
                priceText.text = NumFormat.FormatNumF0F1(lvlPrice);
            }
        }

        lvlText.text = "Lv." + currentLvl.ToString();
    }

    public void Upgrade()
    {
        switch (type)
        {
            case UpgradeTypes.Xp:
                if (currentLvl >= maxLvl)
                {
                    message.SendMessage($"Max level. Sometimes it is getting higher after birth", 3);
                    soundManager.PlayBruhSound();
                    return;
                }
                else if (clicker.Experience < lvlPrice)
                {
                    message.SendMessage($"You need more XP", 2);
                    soundManager.PlayBruhSound();
                    return;
                }
                else
                {
                    clicker.Experience -= (float)lvlPrice;
                    interfaceManager.ExperienceTextUpdate();
                    soundManager.PlayApplauseSound();
                }
                break;
            case UpgradeTypes.Birth:
                if (currentLvl >= maxLvl)
                {
                    message.SendMessage($"Max level. Sometimes it is getting higher after birth", 3);
                    soundManager.PlayBruhSound();
                    return;
                }
                else if (clicker.Births < lvlPrice)
                {
                    message.SendMessage($"You need more birth points", 2);
                    soundManager.PlayBruhSound();
                    return;
                }
                else
                {
                    clicker.Births -= (int)lvlPrice;
                    interfaceManager.ExperienceTextUpdate();
                    soundManager.PlayApplauseSound();
                }
                break;
            case UpgradeTypes.Money:
                if (currentLvl >= maxLvl)
                {
                    message.SendMessage($"Max level. Sometimes it is getting higher after birth", 3);
                    soundManager.PlayBruhSound();
                    return;
                }
                else if (clicker.Currency < lvlPrice)
                {
                    message.SendMessage($"You need more money", 2);
                    soundManager.PlayBruhSound();
                    return;
                }
                else
                {
                    clicker.Currency -= lvlPrice;
                    interfaceManager.CurrencyTextUpdate();
                    soundManager.PlayBuySound();
                }
                break;
        }

        currentLvl++;
        PlayerPrefs.SetInt(upgradeName, currentLvl);
        AddGraphics();

        interfaceManager.UpdateAllText();
        upgradesManager.Invoke(upgradeName, 0f);
        clicker.Save();
        clicker.CalculateDamages();
    }

    public void ResetLvl()
    {
        currentLvl = 0;
        PlayerPrefs.SetInt(upgradeName, currentLvl);
        AddGraphics();
    }
}