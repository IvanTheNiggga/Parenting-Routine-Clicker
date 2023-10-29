using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public List<Upgrade> upgradesDataBase = new List<Upgrade>();

    private Clicker clicker;
    private Message message;
    private SoundManager soundManager;
    private TextManager tm;

    public int DamageLvl;
    public int CritLvl;

    void Start()
    {
        clicker = GetComponent<Clicker>();
        message = GameObject.Find("Message").GetComponent<Message>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        tm = GameObject.Find("INTERFACE").GetComponent<TextManager>();
    }

    public void Upgrade()
    {
        if (clicker.Currency >= clicker.DmgCost && DamageLvl < 1501)
        {
            clicker.Currency -= clicker.DmgCost;
            DamageLvl++;

            clicker.Save();
            clicker.CalculateDmg();
            tm.UpdateDamageUpgradeText();
            tm.CurrencyTextUpdate();
            soundManager.PlayBuySound();
        }
        else
        {
            message.SendMessage($"You need more money", 2);
            soundManager.PlayBruhSound();
        }
    }
    public void UpgradeCrit()
    {
        if (clicker.Currency >= clicker.CritCost && CritLvl < 25)
        {
            clicker.Currency -= clicker.CritCost;
            CritLvl++;

            clicker.Save();
            clicker.CalculateCrit();
            tm.UpdateCritUpgradeText();
            tm.CurrencyTextUpdate();
            soundManager.PlayBuySound();
        }
        else
        {
            message.SendMessage($"You need more money", 2);
            soundManager.PlayBruhSound();
        }
    }
}

[System.Serializable]

public class Upgrade
{
    public Sprite upgradeIco;

    public float lvlPrice;
    public int maxLvl;
    public float stepCoef;
    public string upgradeName;
    public string upgradeDescription;
}
