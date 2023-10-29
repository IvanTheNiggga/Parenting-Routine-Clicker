using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    #region Local
    public Sprite Battle_Sprite;
    public Sprite miner_Sprite;
    public GameObject Upgrade_Prefab;

    private Clicker clicker;
    private EnemyManager enemyManager;
    private Inventory inventory;
    private Message message;
    private UnitManager unitManager;
    private SoundManager soundManager;
    private StagesManager stagesManager;
    private UpgradesManager upgradesManager;
    private Miner miner;

    private ContentSwipe InventorySwipe;

    private Image ItemInfo_Image;
    private Image Location_Image;
    private Image SetCount_Image;

    private GameObject InventoryGrid;
    private GameObject UpgradesGrid;
    private GameObject SellForCurrency;
    private GameObject SellForXp;
    private GameObject UseAgree;
    private GameObject ClickPanel;
    private GameObject ItemOptionsUse;

    private ObjectMovement AgreeWindow_OM;
    private ObjectMovement EnemyParent_OM;
    private ObjectMovement InventoryWindow_OM;
    private ObjectMovement ItemInfoWindow_OM;
    private ObjectMovement SaleWindow_OM;
    private ObjectMovement MainInterface_OM;
    private ObjectMovement UnitInterface_OM;
    private ObjectMovement UnitSelectInterface_OM;
    private ObjectMovement UpgradeInterface_OM;
    private ObjectMovement SettingsWindow_OM;
    private ObjectMovement SetCountWindow_OM;
    private ObjectMovement MinerInterface_OM;
    private ObjectMovement BirthButton_OM;

    private InputField Count_Input;

    private Text SellForCurrencyPrice_Text;
    private Text SellForXpPrice_Text;
    private Text AgreeDescription_Text;
    private Text ItemInfoCount_Text;
    private Text ItemInfoDescription_Text;
    private Text SaleForCurrencyPrice_Text;
    private Text SaleForXpPrice_Text;
    private Text UnitLocked1_Text;
    private Text UnitLocked2_Text;

    public string typeOfAction;
    public string typeOfButtonAction;
    #endregion

    #region Init
    private void Start()
    {
        InitializeComponents();
        AddUpgrades();
    }

    private void InitializeComponents()
    {
        GameObject cm = GameObject.Find("ClickerManager");
        clicker = cm.GetComponent<Clicker>();
        enemyManager = cm.GetComponent<EnemyManager>();
        inventory = cm.GetComponent<Inventory>();
        upgradesManager = cm.GetComponent<UpgradesManager>();
        unitManager = cm.GetComponent<UnitManager>();
        stagesManager = cm.GetComponent<StagesManager>();
        message = GameObject.Find("Message").GetComponent<Message>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        miner = GameObject.Find("Miner").GetComponent<Miner>();

        InventorySwipe = GameObject.Find("InventoryGridPanel").GetComponent<ContentSwipe>();

        Location_Image = GameObject.Find("Location(img)").GetComponent<Image>();
        ItemInfo_Image = GameObject.Find("ItemInfo(img)").GetComponent<Image>();
        SetCount_Image = GameObject.Find("SetCount(img)").GetComponent<Image>();

        InventoryGrid = GameObject.Find("InventoryGrid");
        UpgradesGrid = GameObject.Find("UpgradesGrid");
        SellForCurrency = GameObject.Find("SetCountCurrency(btn)");
        SellForXp = GameObject.Find("SetCountXp(btn)");
        UseAgree = GameObject.Find("SetCountAgree(btn)");
        ClickPanel = GameObject.Find("Clickable(cdr)");
        ItemOptionsUse = GameObject.Find("ItemActionUse(btn)");

        AgreeWindow_OM = GameObject.Find("ActionConfirmation").GetComponent<ObjectMovement>();
        EnemyParent_OM = GameObject.Find("Enemy Parent").GetComponent<ObjectMovement>();
        SaleWindow_OM = GameObject.Find("Sale").GetComponent<ObjectMovement>();
        InventoryWindow_OM = GameObject.Find("Inventory").GetComponent<ObjectMovement>();
        ItemInfoWindow_OM = GameObject.Find("ItemInfo").GetComponent<ObjectMovement>();
        MainInterface_OM = GameObject.Find("Battle Interface").GetComponent<ObjectMovement>();
        UnitSelectInterface_OM = GameObject.Find("UnitSelect Interface").GetComponent<ObjectMovement>();
        UnitInterface_OM = GameObject.Find("Units Interface").GetComponent<ObjectMovement>();
        SetCountWindow_OM = GameObject.Find("SetCount").GetComponent<ObjectMovement>();
        SettingsWindow_OM = GameObject.Find("Settings").GetComponent<ObjectMovement>();
        BirthButton_OM = GameObject.Find("Birth(btn)").GetComponent<ObjectMovement>();
        UpgradeInterface_OM = GameObject.Find("Upgrades Interface").GetComponent<ObjectMovement>();
        MinerInterface_OM = GameObject.Find("Miner Interface").GetComponent<ObjectMovement>();

        Count_Input = GameObject.Find("InputField").GetComponent<InputField>();

        AgreeDescription_Text = GameObject.Find("Agree(txt)").GetComponent<Text>();
        SellForCurrencyPrice_Text = GameObject.Find("SetCountCurrency(txt)").GetComponent<Text>();
        SellForXpPrice_Text = GameObject.Find("SetCountXp(txt)").GetComponent<Text>();
        ItemInfoCount_Text = GameObject.Find("ItemCount(txt)").GetComponent<Text>();
        ItemInfoDescription_Text = GameObject.Find("ItemInfo(txt)").GetComponent<Text>();
        SaleForCurrencyPrice_Text = GameObject.Find("SaleForCurrency(txt)").GetComponent<Text>();
        SaleForXpPrice_Text = GameObject.Find("SaleForXp(txt)").GetComponent<Text>();
        UnitLocked1_Text = GameObject.Find("Unit1Locked(lbl)").GetComponent<Text>();
        UnitLocked2_Text = GameObject.Find("Unit2Locked(lbl)").GetComponent<Text>();
    }
    #endregion
    public void CheckRebirth()
    {
        if (unitManager.isAbleToBirth())
        {
            BirthButton_OM.MoveTo(new Vector2(-298.5f, 170), 0.3f, 1, false);
        }
        else
        {
            BirthButton_OM.MoveTo(new Vector2(-498.5f, 170), 0.3f, 1, false);
        }
    }

    public void CloseAll()
    {
        if (agreeWindowOpened) { SwitchConfirmation(0); }
        if (settingsOpened) { SwitchSettings(0); }
        if (minerOpened) { MinerInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false); }
        if (upgradeOpened) { SwitchUpgradesMenu(0); }
        if (unitsListOpened) { CloseUnitsSelect(); }
        if (unitsInterfaceOpened) { SwitchUnitsInterface(0); }
        if (inventoryOpened) { SwitchInventory(0); }
        if (saleOpened) { SwitchSale(0); }
        if (setCountOpened) { CloseSetCount(); }
        if (itemInfoOpened) { SwitchItemInfo(0); }
    }

    #region MAIN INTERFACE
    public bool mainInterfaceOpened;
    public void SwitchBattleInterface(int mode)
    {
        switch (mode)
        {
            case 1:
                if (minerOpened)
                {
                    MinerInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                }
                else
                {
                    mainInterfaceOpened = true;
                    ClickPanel.SetActive(true);

                    MainInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                    EnemyParent_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);

                    enemyManager.clickable = true;
                    enemyManager.able = true;
                }
                break;
            case 0:
                if (minerOpened)
                {
                    MinerInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                }
                else
                {
                    mainInterfaceOpened = false;
                    ClickPanel.SetActive(false);

                    MainInterface_OM.MoveTo(new Vector2(-720, 0), 0.3f, 1, false);
                    EnemyParent_OM.MoveTo(new Vector2(-360, 0), 0.3f, 1, false);

                    enemyManager.clickable = false;
                    enemyManager.able = false;
                }
                break;

            default:
                SwitchBattleInterface(!mainInterfaceOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Agree menu
    public bool agreeWindowOpened;
    public void SwitchConfirmation(int mode)
    {
        switch (mode)
        {
            case 1:
                switch (typeOfButtonAction)
                {
                    case "birth":
                        AgreeDescription_Text.text = $"Make another one child for {NumFormat.FormatNumF1(unitManager.BirthCost())}$ ?";
                        break;
                    case "minerupgrade":
                        if (inventory.ItemTypeFind("Cloth") == null)
                        {
                            message.SendMessage($"You need at least one cloth in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        else if (clicker.MinerLvl == 0)
                        {
                            message.SendMessage($"Fix washing mashine at first", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade washing mashine for Cloth item?";
                        break;
                    case "unitupgrade1":
                        if (inventory.ItemTypeFind("Toy") == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        else if (clicker.Births < 1)
                        {
                            message.SendMessage($"You need at least one children", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade fisrt child for one Toy item ?";
                        SwitchUnitsInterface(0);
                        break;
                    case "unitupgrade2":
                        if (inventory.ItemTypeFind("Toy") == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        else if (clicker.Births < 2)
                        {
                            message.SendMessage($"You need at least two children", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade second child for one Toy item ?";
                        SwitchUnitsInterface(0);
                        break;
                }
                CloseAll();
                agreeWindowOpened = true;
                AgreeWindow_OM.MoveTo(new Vector2(0, 90), 0.3f, 1, false);
                SwitchBattleInterface(0);
                break;
            case 0:
                agreeWindowOpened = false;
                AgreeWindow_OM.MoveTo(new Vector2(-720, 90), 0.3f, 1, false);
                SwitchBattleInterface(1);
                break;
            default:
                SwitchConfirmation(!agreeWindowOpened ? 1 : 0);
                break;
        }
    }
    public void Confirm()
    {
        switch (typeOfButtonAction)
        {
            case "birth":
                if (unitManager.isAbleToBuy())
                {
                    clicker.Birth();
                }
                else
                {
                    message.SendMessage($"You need more money", 2);
                    soundManager.PlayBruhSound();
                }
                break;
            case "minerupgrade":
                miner.UpgradeMiner();
                inventory.ConsumeAnyItemOfType("Cloth");
                break;
            case "unitupgrade1":
                unitManager.UpgradeUnit(1);
                inventory.ConsumeAnyItemOfType("Toy");
                break;
            case "unitupgrade2":
                unitManager.UpgradeUnit(2);
                inventory.ConsumeAnyItemOfType("Toy");
                break;
        }
        SwitchBattleInterface(1);
    }

    public void Action(string s)
    {
        typeOfButtonAction = s;
        SwitchConfirmation(1);
    }
    #endregion

    #region Settings
    public bool settingsOpened;
    public void SwitchSettings(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                settingsOpened = true;
                SettingsWindow_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                SwitchBattleInterface(0);
                break;
            case 0:
                SwitchBattleInterface(1);
                settingsOpened = false;
                SettingsWindow_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                break;
            default:
                SwitchSettings(!settingsOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Washing Mashine
    public bool minerOpened;
    public void SwitchMiner(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                SwitchBattleInterface(0);
                minerOpened = true;
                MinerInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                stagesManager.ChangeAmbience(false);
                Location_Image.sprite = Battle_Sprite;
                break;
            case 0:
                CloseAll();
                minerOpened = false;
                SwitchBattleInterface(1);
                MinerInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                stagesManager.ChangeAmbience(true);
                Location_Image.sprite = miner_Sprite;
                break;
            default:
                SwitchMiner(!minerOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Upgrade Menu
    public bool upgradeOpened;
    public void SwitchUpgradesMenu(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                upgradeOpened = true;
                UpdateUpgrades();
                UpgradeInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                SwitchBattleInterface(0);
                break;
            case 0:
                upgradeOpened = false;
                UpgradeInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                SwitchBattleInterface(1);
                break;
            default:
                SwitchUpgradesMenu(!upgradeOpened ? 1 : 0);
                break;
        }
    }
    private void AddUpgrades()
    {
        for (int i = 0; i < upgradesManager.upgradesDataBase.Count; i++)
        {
            UpgradeForXp upgrade = Instantiate(Upgrade_Prefab, UpgradesGrid.transform).GetComponent<UpgradeForXp>();
            upgrade.index = i;
            upgrade.AddGraphics();
        }
    }

    public void UpdateUpgrades()
    {
        upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();

        for (int i = 0; i < upgradesManager.upgradesDataBase.Count; i++)
        {
            UpgradeForXp upgradeForXpTemp = GameObject.Find(upgradesManager.upgradesDataBase[i].upgradeName).GetComponent<UpgradeForXp>();
            upgradeForXpTemp.AddGraphics();
        }
    }
    #endregion

    #region Unit Menu
    public bool unitsInterfaceOpened;
    public void SwitchUnitsInterface(int mode)
    {
        switch (mode)
        {
            case 1:
                if (clicker.Births < 1)
                {
                    message.SendMessage($"You need at least one child", 2);
                    soundManager.PlayBruhSound();
                    return;
                }
                CloseAll();
                unitsInterfaceOpened = true;
                UnitInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                switch (clicker.Births)
                {
                    case 0:
                        UnitLocked1_Text.text = "1 Birth required.";
                        UnitLocked2_Text.text = "2 Birth required.";
                        break;
                    case 1:
                        UnitLocked1_Text.text = "";
                        UnitLocked2_Text.text = "2 Birth required.";
                        break;
                    default:
                        UnitLocked1_Text.text = "";
                        UnitLocked2_Text.text = "";
                        break;
                }
                SwitchBattleInterface(0);
                break;
            case 0:
                unitsInterfaceOpened = false;
                UnitInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                SwitchBattleInterface(1);
                break;
            default:
                SwitchUnitsInterface(!unitsInterfaceOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Unit List
    public bool unitsListOpened;
    public void OpenUnitsSelect(int slot)
    {
        if (slot > clicker.Births)
        {
            message.SendMessage($"You need {slot - clicker.Births} births", 2);
            soundManager.PlayBruhSound();
            return;
        }
        CloseAll();
        SwitchBattleInterface(0);
        unitsListOpened = true;
        unitManager.slotid = slot;
        UnitSelectInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
    }
    public void CloseUnitsSelect()
    {
        unitsListOpened = false;
        UnitSelectInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
        SwitchUnitsInterface(1);
    }
    #endregion

    #region Inventory
    public bool inventoryOpened;
    public void SwitchInventory(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                SwitchBattleInterface(0);
                inventoryOpened = true;
                inventory.SortInventory();
                InventoryWindow_OM.MoveTo(new Vector2(0, 270), 0.3f, 1, false);
                break;
            case 0:
                if (saleOpened) { SwitchSale(0); }
                SwitchBattleInterface(1);
                inventoryOpened = false;
                InventoryWindow_OM.MoveTo(new Vector2(0, 1480), 0.3f, 1, false);
                InventorySwipe.SetBack();
                inventory.SetItemsBack();
                SwitchBattleInterface(1);
                break;
            default:
                SwitchInventory(!inventoryOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Sale Window
    public bool saleOpened;
    public void SwitchSale(int mode)
    {
        switch (mode)
        {
            case 1:
                if (InventoryGrid.transform.childCount < 1)
                {
                    message.SendMessage($"You need at least one item", 2);
                    soundManager.PlayBruhSound();
                }
                else
                {
                    saleOpened = true;
                    SaleWindow_OM.MoveTo(new Vector2(0, -300f), 0.3f, 1, false);

                    SaleForCurrencyPrice_Text.text = "0";
                    SaleForXpPrice_Text.text = "0";

                    inventory.ableToInvest = true;
                }
                break;
            case 0:
                saleOpened = false;
                inventory.SetItemsBack();
                inventory.ableToInvest = false;
                SaleWindow_OM.MoveTo(new Vector2(0, -1200), 0.3f, 1, false);
                break;
            default:
                SwitchSale(!saleOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Item Info
    public bool itemInfoOpened;
    public void SwitchItemInfo(int mode)
    {
        switch (mode)
        {
            case 1:
                itemInfoOpened = true;
                ItemInfoWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);

                Item item = inventory.SelectedItem.GetComponent<Item>();

                SellForCurrency.SetActive(true);
                SellForXp.SetActive(true);
                ItemOptionsUse.SetActive(true);

                if (item.useMethodName == "")
                {
                    ItemOptionsUse.SetActive(false);
                }
                ItemInfo_Image.sprite = item.ico.sprite;
                ItemInfoDescription_Text.text = "Type : " + item.type + " \n Name: " + item.nameObject;
                ItemInfoCount_Text.text = NumFormat.FormatNumF0F1(item.count);
                break;
            case 0:
                itemInfoOpened = false;
                ItemInfoWindow_OM.MoveTo(new Vector2(720, -200), 0.3f, 1, false);
                break;
            default:
                SwitchItemInfo(!itemInfoOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Set Count Menu
    public bool setCountOpened;
    public void SetCountUpdate()
    {
        if (inventory.SelectedItem != null)
        {
            Item item = inventory.SelectedItem.GetComponent<Item>();
            int input = 1;
            if (Count_Input.text.Length > 0)
            {
                input = int.Parse(Count_Input.text);
                input = input > item.count ? item.count : input;
            }

            if (typeOfAction == "Sell")
            {
                SellForCurrencyPrice_Text.text = "+" + NumFormat.FormatNumF1(input * item.currencyPrice);
                SellForXpPrice_Text.text = "+" + NumFormat.FormatNumF1(input * item.xpPrice);
                return;
            }
        }
    }
    public void SetCountMax()
    {
        if (inventory.SelectedItem != null)
        {
            Item item = inventory.SelectedItem.GetComponent<Item>();
            Count_Input.text = item.count.ToString();
            return;
        }
    }
    public void OpenItemUsePanel()
    {
        setCountOpened = true;
        Item item = inventory.SelectedItem.GetComponent<Item>();
        if (item.type == "Toy")
        {
            SwitchUnitsInterface(1);
            return;
        }
        if (item.type == "Cloth")
        {
            SwitchMiner(1);
            return;
        }
        SwitchItemInfo(0);
        SetCountWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);

        typeOfAction = "Use";

        UseAgree.SetActive(true);
        SellForCurrency.SetActive(false);
        SellForXp.SetActive(false);

        SetCount_Image.sprite = inventory.SelectedItem.GetComponent<Item>().ico.sprite;
        SetCountUpdate();
    }
    public void OpenItemSellPanel()
    {
        setCountOpened = true;
        SwitchItemInfo(0);
        SetCountWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);

        typeOfAction = "Sell";

        UseAgree.SetActive(false);
        SellForCurrency.SetActive(true);
        SellForXp.SetActive(true);

        SetCount_Image.sprite = inventory.SelectedItem.GetComponent<Item>().ico.sprite;
        SetCountUpdate();
    }
    public void CloseSetCount()
    {
        setCountOpened = false;
        typeOfAction = "";
        SetCountWindow_OM.MoveTo(new Vector2(-720, -200), 0.3f, 1, false);
    }
    public void ResetSelectedItem()
    { inventory.SelectedItem = null; }
    #endregion
}