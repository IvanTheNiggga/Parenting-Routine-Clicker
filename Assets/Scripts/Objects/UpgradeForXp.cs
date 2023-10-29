using UnityEngine;
using UnityEngine.UI;

public class UpgradeForXp : MonoBehaviour
{
    private Clicker clicker;
    private InterfaceManager interfaceManager;
    private UpgradesManager upgrades;
    private Message message;
    private SoundManager soundManager;
    private StagesManager stagesManager;
    private TextManager textManager;


    public int index;
    public int currentLvl;
    public float lvlPrice;
    public int maxLvl;
    public float stepCoef;
    public string upgradeName;
    public string methodName;

    public Text upgradeDescriptionText;
    public Text lvlText;
    public Text priceText;
    public Text xpText;

    public Image upgradeIco;

    bool loaded;

    public void AddGraphics()
    {
        if (loaded == false)
        {
            clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
            upgrades = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();
            interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
            textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();
            message = GameObject.Find("Message").GetComponent<Message>();
            name = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>().upgradesDataBase[index].upgradeName;
            stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

            loaded = true;

            upgradeName = upgrades.upgradesDataBase[index].upgradeName;
            currentLvl = PlayerPrefs.GetInt(upgradeName);
            methodName = upgradeName;
            name = upgradeName;
            stepCoef = upgrades.upgradesDataBase[index].stepCoef;
            maxLvl = upgrades.upgradesDataBase[index].maxLvl;
            if(methodName == "BetterStartUpgrade")
            {
                maxLvl = PlayerPrefs.GetInt("maxStage");
            }
            upgradeDescriptionText.text = upgrades.upgradesDataBase[index].upgradeDescription;
        }
        if (methodName == "BetterStartUpgrade")
        {
            maxLvl = stagesManager.maxStage - 20;
        }
        else if (methodName == "BetterMineAfterRebirthUpgrade")
        {
            maxLvl = stagesManager.maxStage - 10;
        }
        if (stepCoef == 1)
        { lvlPrice = upgrades.upgradesDataBase[index].lvlPrice + (currentLvl * upgrades.upgradesDataBase[index].lvlPrice); }
        if (stepCoef == 0)
        { lvlPrice = upgrades.upgradesDataBase[index].lvlPrice; }
        else
        {
            lvlPrice = upgrades.upgradesDataBase[index].lvlPrice;
            for (int i = 0; currentLvl > i; i++)
            { lvlPrice *= stepCoef; }
        }

        if (currentLvl >= maxLvl)
        {
            priceText.text = "Max level."; lvlPrice = 0;
        }
        else
        {
            priceText.text = lvlPrice.ToString();
        }

        lvlText.text = "Lv." + currentLvl.ToString();
        upgradeIco.sprite = upgrades.upgradesDataBase[index].upgradeIco;
    }

    public void Upgrade()
    {
        if (clicker.Experience >= lvlPrice && currentLvl < maxLvl)
        {
            if (name == "DoubleDamageUpgrade")
            {
                clicker.CalculateDmg();
            }

            clicker.Experience -= lvlPrice;
            textManager.ExperienceTextUpdate();
            currentLvl++;

            PlayerPrefs.SetInt(upgradeName, currentLvl);
            Use();

            interfaceManager.UpdateUpgrades();
            soundManager.PlayApplauseSound();
            clicker.Save();
        }
        else
        {
            message.SendMessage($"You need more XP", 2);
            soundManager.PlayBruhSound();
        }
    }
    public void Use()
    { clicker.Invoke(methodName, 0f); }

    public void ResetLvl()
    {
        currentLvl = 0;
        PlayerPrefs.SetInt(upgradeName, currentLvl);
        AddGraphics();
    }
}