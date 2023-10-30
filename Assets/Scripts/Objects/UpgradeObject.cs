using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour
{
    private Clicker clicker;
    private UpgradesManager upgradesManager;
    private Message message;
    private SoundManager soundManager;
    private StagesManager stagesManager;
    private TextManager textManager;

    public int index;
    private int currentLvl;

    private UpgradeTypes type;
    private double lvlPrice;
    private int maxLvl;
    private float stepCoef;
    private string upgradeName;

    [SerializeField] private Text upgradeDescriptionText;
    [SerializeField] private Text lvlText;
    [SerializeField] private Text priceText;

    [SerializeField]private Image upgradeIco;
    [SerializeField] private Image currencyIco;
    [SerializeField] private Sprite moneyIco;
    [SerializeField] private Sprite xpIco;
    [SerializeField] private Sprite birthIco;

    private bool loaded;

    public void AddGraphics()
    {
        if (loaded == false)
        {
            clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
            upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
            textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();
            message = GameObject.Find("Message").GetComponent<Message>();
            name = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>().upgradesDataBase[index].upgradeName;
            stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

            loaded = true;

            upgradeName = upgradesManager.upgradesDataBase[index].upgradeName;

            name = upgradeName;
            type = upgradesManager.upgradesDataBase[index].type;
            upgradeIco.sprite = upgradesManager.upgradesDataBase[index].upgradeIco;
            if (type == UpgradeTypes.Xp)
            {
                currencyIco.sprite = xpIco;
            }
            else if (type == UpgradeTypes.Birth)
            {
                currencyIco.sprite = birthIco;
            }
            else
            {
                currencyIco.sprite = moneyIco;
            }

            stepCoef = upgradesManager.upgradesDataBase[index].stepCoef;
            maxLvl = upgradesManager.upgradesDataBase[index].maxLvl;
            if (upgradeName == "BetterStartUpgrade")
            {
                maxLvl = PlayerPrefs.GetInt("maxStage");
            }
            upgradeDescriptionText.text = upgradesManager.upgradesDataBase[index].upgradeDescription;
        }


        currentLvl = PlayerPrefs.GetInt(upgradeName);
        if (upgradeName == "BetterStartUpgrade")
        {
            maxLvl = stagesManager.maxStage - 20;
        }
        else if (upgradeName == "BetterMineAfterRebirthUpgrade")
        {
            maxLvl = stagesManager.maxStage - 10;
        }
        if (stepCoef == 1)
        { lvlPrice = upgradesManager.upgradesDataBase[index].lvlPrice + (currentLvl * upgradesManager.upgradesDataBase[index].lvlPrice); }
        if (stepCoef == 0)
        { lvlPrice = upgradesManager.upgradesDataBase[index].lvlPrice; }
        else
        {
            lvlPrice = Utils.Progression(upgradesManager.upgradesDataBase[index].lvlPrice, stepCoef, currentLvl);
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
        if (type == UpgradeTypes.Xp)
        {
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
                textManager.ExperienceTextUpdate();
                soundManager.PlayApplauseSound();
            }
        }
        else if (type == UpgradeTypes.Birth)
        {
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
                textManager.ExperienceTextUpdate();
                soundManager.PlayApplauseSound();
            }
        }
        else
        {
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
                textManager.CurrencyTextUpdate();
                soundManager.PlayBuySound();
            }
        }

        if (name == "DoubleDamageUpgrade" || name == "DamageUpgrade")
        {
            clicker.CalculateDmg();
        }
        if (name == "CritUpgrade")
        {
            clicker.CalculateCrit();
        }

        currentLvl++;
        PlayerPrefs.SetInt(upgradeName, currentLvl);
        AddGraphics();

        textManager.UpdateAllText();
        Use();
        clicker.Save();
    }
    public void Use()
    { upgradesManager.Invoke(upgradeName, 0f); }

    public void ResetLvl()
    {
        currentLvl = 0;
        PlayerPrefs.SetInt(upgradeName, currentLvl);
        AddGraphics();
    }
}