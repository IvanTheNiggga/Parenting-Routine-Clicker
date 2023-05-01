using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Sprite Battle_Sprite;
    public Sprite Washingmashine_Sprite;
    public GameObject Upgrade_Prefab;

    private Clicker clicker;
    private EnemyManager enemyManager;
    private Inventory inventory;
    private Message message;
    private MinionManager minionManager;
    private SoundManager soundManager;
    private StagesManager stagesManager;
    private TextManager textManager;
    private Upgrades upgrades;
    private Washingmashine washingmashine;

    private ContentSwipe InventoryGridPanel_CSwipe;

    private Image ItemInfo_Image;
    private Image Location_Image;

    private GameObject InventoryGrid;
    private GameObject UpgradesGrid;
    private GameObject SellForCurrencyAgree;
    private GameObject SellForXpAgree;
    private GameObject UseAgree;
    private GameObject ClickPanel;
    private GameObject RightButtons;
    private GameObject ItemOptionsUse;

    private ObjectMovement AgreeWindow_OM;
    private ObjectMovement EnemyParent_OM;
    private ObjectMovement InventoryWindow_OM;
    private ObjectMovement ItemInfoWindow_OM;
    private ObjectMovement SaleWindow_OM;
    private ObjectMovement MainInterface_OM;
    private ObjectMovement RightButtons_OM;
    private ObjectMovement MinionInterface_OM;
    private ObjectMovement MinionSelectInterface_OM;
    private ObjectMovement UpgradeInterface_OM;
    private ObjectMovement SettingsWindow_OM;
    private ObjectMovement SetCountWindow_OM;
    private ObjectMovement WashingInterface_OM;
    private ObjectMovement BirthButton_OM;

    private Slider Count_Slider;

    private Text SellForCurrencyPrice_Text;
    private Text SellForXpPrice_Text;
    private Text AgreeDescription_Text;
    private Text SetCountDescription_Text;
    private Text ItemPrices_Text;
    private Text SaleDescription_Text;
    private Text ItemInfoCount_Text;
    private Text ItemInfoDescription_Text;
    private Text SaleForCurrencyPrice_Text;
    private Text SaleForXpPrice_Text;
    private Text MinionLocked1_Text;
    private Text MinionLocked2_Text;

    public string typeOfAction;
    public string typeOfButtonAction;

    private void Start()
    {
        GameObject cm = GameObject.Find("ClickerManager");
        clicker = cm.GetComponent<Clicker>();
        enemyManager = cm.GetComponent<EnemyManager>();
        inventory = cm.GetComponent<Inventory>();
        upgrades = cm.GetComponent<Upgrades>();
        minionManager = cm.GetComponent<MinionManager>();
        stagesManager = cm.GetComponent<StagesManager>();
        message = GameObject.Find("Message").GetComponent<Message>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        textManager = GameObject.Find("INTERFACE").GetComponent<TextManager>();
        washingmashine = GameObject.Find("Washingmashine").GetComponent<Washingmashine>();

        InventoryGridPanel_CSwipe = GameObject.Find("InventoryGridPanel").GetComponent<ContentSwipe>();

        Location_Image = GameObject.Find("LocationIco").GetComponent<Image>();
        ItemInfo_Image = GameObject.Find("Item Info Image").GetComponent<Image>();

        InventoryGrid = GameObject.Find("InventoryGrid");
        UpgradesGrid = GameObject.Find("UpgradesGrid");
        SellForCurrencyAgree = GameObject.Find("==SellForCurrencyAgree==");
        SellForXpAgree = GameObject.Find("==SellForXpAgree==");
        UseAgree = GameObject.Find("==UseAgree==");
        ClickPanel = GameObject.Find("Click Panel");
        RightButtons = GameObject.Find("Right Buttons");
        ItemOptionsUse = GameObject.Find("==ItemOptionsUse==");

        AgreeWindow_OM = GameObject.Find("Agree Window").GetComponent<ObjectMovement>();
        EnemyParent_OM = GameObject.Find("Enemy Parent").GetComponent<ObjectMovement>();
        SaleWindow_OM = GameObject.Find("Sale Window").GetComponent<ObjectMovement>();
        InventoryWindow_OM = GameObject.Find("Inventory Window").GetComponent<ObjectMovement>();
        ItemInfoWindow_OM = GameObject.Find("Item Info Window").GetComponent<ObjectMovement>();
        MainInterface_OM = GameObject.Find("Main Interface").GetComponent<ObjectMovement>();
        RightButtons_OM = GameObject.Find("Right Buttons").GetComponent<ObjectMovement>();
        MinionSelectInterface_OM = GameObject.Find("Minion Select Interface").GetComponent<ObjectMovement>();
        MinionInterface_OM = GameObject.Find("Minion Interface").GetComponent<ObjectMovement>();
        SetCountWindow_OM = GameObject.Find("SetCount Window").GetComponent<ObjectMovement>();
        SettingsWindow_OM = GameObject.Find("Settings Window").GetComponent<ObjectMovement>();
        BirthButton_OM = GameObject.Find("==Birth==").GetComponent<ObjectMovement>();
        UpgradeInterface_OM = GameObject.Find("Upgrade Interface").GetComponent<ObjectMovement>();
        WashingInterface_OM = GameObject.Find("Washing Interface").GetComponent<ObjectMovement>();

        Count_Slider = GameObject.Find("CountSlider").GetComponent<Slider>();

        AgreeDescription_Text = GameObject.Find("Agree Description").GetComponent<Text>();
        SellForCurrencyPrice_Text = GameObject.Find("SellForCurrencyPrice").GetComponent<Text>();
        SellForXpPrice_Text = GameObject.Find("SellForXpPrice").GetComponent<Text>();
        SetCountDescription_Text = GameObject.Find("SetCount Description").GetComponent<Text>();
        ItemPrices_Text = GameObject.Find("Item Prices").GetComponent<Text>();
        SaleDescription_Text = GameObject.Find("Sale Description").GetComponent<Text>();
        ItemInfoCount_Text = GameObject.Find("ItemCount").GetComponent<Text>();
        ItemInfoDescription_Text = GameObject.Find("Item Information").GetComponent<Text>();
        SaleForCurrencyPrice_Text = GameObject.Find("SaleForCurrencyPrice").GetComponent<Text>();
        SaleForXpPrice_Text = GameObject.Find("SaleForXpPrice").GetComponent<Text>();
        MinionLocked1_Text = GameObject.Find("MinionLockedText1").GetComponent<Text>();
        MinionLocked2_Text = GameObject.Find("MinionLockedText2").GetComponent<Text>();

        AddUpgrades();
    }

    public void CheckRebirth()
    {
        if (minionManager.isAbleToBirth())
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
        if (agreeWindowOpened) { SwitchAgreeWindow(0); }
        if (rightButtonsOpened) { SwitchRightButtons(0); }
        if (settingsOpened) { SwitchSettings(0); }
        if (washingMashineOpened) { WashingInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false); }
        if (upgradeOpened) { SwitchUpgradeMenu(0); }
        if (minionInterfaceOpened) { SwitchMinionInterface(0); }
        if (minionListOpened) { CloseMinionSelect(); }
        if (inventoryOpened) { SwitchInventory(0); }
        if (saleWindowOpened) { SwitchSaleWindow(0); }
        if (itemInfoOpened) { SwitchItemInfo(0); }
        if (setCountOpened) { CloseSetCount(); }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// MAIN INTERFACE
    public bool mainInterfaceOpened;
    public void SwitchMainInterface(int mode)
    {
        switch (mode)
        {
            case 1:
                if (washingMashineOpened)
                {
                    WashingInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false); 
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
                if (washingMashineOpened)
                {
                    WashingInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
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
                SwitchMainInterface(!mainInterfaceOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Agree menu
    public bool agreeWindowOpened;
    public void SwitchAgreeWindow(int mode)
    {
        switch (mode)
        {
            case 1:
                switch (typeOfButtonAction)
                {
                    case "birth":
                        AgreeDescription_Text.text = $"Make another one child for {FormatNumsHelper.FormatNumF1(minionManager.BirthCost())}$ ?";
                        break;
                    case "wmupgrade":
                        if (inventory.ItemTypeFind("Cloth") == null || clicker.WashingmashineLvl == 0)
                        {
                            message.SendMessage($"You need at least one cloth in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade washing mashine for Cloth item?";
                        break;
                    case "minionupgrade1":
                        if (inventory.ItemTypeFind("Toy") == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade fisrt child for one Toy item ?";
                        SwitchMinionInterface(0);
                        break;
                    case "minionupgrade2":
                        if (inventory.ItemTypeFind("Toy") == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            soundManager.PlayBruhSound();
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade second child for one Toy item ?";
                        SwitchMinionInterface(0);
                        break;
                }
                CloseAll();
                agreeWindowOpened = true;
                AgreeWindow_OM.MoveTo(new Vector2(0, 90), 0.3f, 1, false);
                SwitchMainInterface(0);
                break;
            case 0:
                agreeWindowOpened = false;
                AgreeWindow_OM.MoveTo(new Vector2(-720, 90), 0.3f, 1, false);
                SwitchMainInterface(1);
                break;
            default:
                SwitchAgreeWindow(!agreeWindowOpened ? 1 : 0);
                break;
        }
    }
    public void Confirm()
    {
        switch (typeOfButtonAction)
        {
            case "birth":
                if (minionManager.isAbleToBuy())
                {
                    clicker.Birth();
                }
                else
                {
                    message.SendMessage($"You need more money", 2);
                    soundManager.PlayBruhSound();
                }
                break;
            case "wmupgrade":
                washingmashine.UpgradeWashingmashine();
                inventory.ConsumeAnyItemOfType("Cloth");
                break;
            case "minionupgrade1":
                minionManager.UpgradeMinion(1);
                inventory.ConsumeAnyItemOfType("Toy");
                break;
            case "minionupgrade2":
                minionManager.UpgradeMinion(2);
                inventory.ConsumeAnyItemOfType("Toy");
                break;
        }
        SwitchMainInterface(1);
    }

    public void Action(string s)
    {
        typeOfButtonAction = s;
        SwitchAgreeWindow(1);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Right Buttons
    public bool rightButtonsOpened;
    public void SwitchRightButtons(int mode)
    {
        switch (mode)
        {
            case 1:
                rightButtonsOpened = true;
                SwitchButtonsActive(true);
                RightButtons_OM.MoveTo(new Vector2(0, 178), 0.3f, 5, false);
                break;
            case 0:
                rightButtonsOpened = false;
                RightButtons_OM.MoveTo(new Vector2(120, 178), 0.3f, 5, true);
                break;
            default:
                SwitchRightButtons(!rightButtonsOpened ? 1 : 0);
                break;
        }
    }
    public void SwitchButtonsActive(bool b)
    {
        RightButtons.SetActive(b);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Settings
    public bool settingsOpened;
    public void SwitchSettings(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                settingsOpened = true;
                SettingsWindow_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                SwitchMainInterface(0);
                break;
            case 0:
                SwitchMainInterface(1);
                settingsOpened = false;
                SettingsWindow_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                break;
            default:
                SwitchSettings(!settingsOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Washing Mashine
    public bool washingMashineOpened;
    public void SwitchWashingmashine(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                SwitchMainInterface(0);
                washingMashineOpened = true;
                WashingInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                stagesManager.ChangeAmbience(false);
                Location_Image.sprite = Battle_Sprite;
                break;
            case 0:
                CloseAll();
                washingMashineOpened = false;
                SwitchMainInterface(1);
                WashingInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                stagesManager.ChangeAmbience(true);
                Location_Image.sprite = Washingmashine_Sprite;
                break;
            default:
                SwitchWashingmashine(!washingMashineOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Upgrade Menu
    public bool upgradeOpened;
    public void SwitchUpgradeMenu(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                upgradeOpened = true;
                UpdateUpgradeGraphics();
                UpgradeInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                SwitchMainInterface(0);
                break;
            case 0:
                upgradeOpened = false;
                UpgradeInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                SwitchMainInterface(1);
                break;
            default:
                SwitchUpgradeMenu(!upgradeOpened ? 1 : 0);
                break;
        }
    }
    private void AddUpgrades()
    {
        for (int i = 0; i < upgrades.upgradesDataBase.Count; i++)
        {
            UpgradeForXp upgrade = Instantiate(Upgrade_Prefab, UpgradesGrid.transform).GetComponent<UpgradeForXp>();
            upgrade.index = i;
            upgrade.AddGraphics();
        }
    }

    public void UpdateUpgradeGraphics()
    {
        upgrades = GameObject.Find("ClickerManager").GetComponent<Upgrades>();

        for (int i = 0; i < upgrades.upgradesDataBase.Count; i++)
        {
            UpgradeForXp upgradeForXpTemp = GameObject.Find(upgrades.upgradesDataBase[i].upgradeName).GetComponent<UpgradeForXp>();
            upgradeForXpTemp.AddGraphics();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Minion Menu
    public bool minionInterfaceOpened;
    public void SwitchMinionInterface(int mode)
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
                minionInterfaceOpened = true;
                MinionInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                switch (clicker.Births)
                {
                    case 0:
                        MinionLocked1_Text.text = "1 Birth required.";
                        MinionLocked2_Text.text = "2 Birth required.";
                        break;
                    case 1:
                        MinionLocked1_Text.text = "";
                        MinionLocked2_Text.text = "2 Birth required.";
                        break;
                    default:
                        MinionLocked1_Text.text = "";
                        MinionLocked2_Text.text = "";
                        break;
                }
                SwitchMainInterface(0);
                break;
            case 0:
                minionInterfaceOpened = false;
                MinionInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                SwitchMainInterface(1);
                break;
            default:
                SwitchMinionInterface(!minionInterfaceOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Minion List
    public bool minionListOpened;
    public void OpenMinionSelect(int slot)
    {
        if (slot > clicker.Births)
        {
            message.SendMessage($"You need {slot - clicker.Births} births", 2);
            soundManager.PlayBruhSound();
            return;
        }
        CloseAll();
        SwitchMainInterface(0);
        minionListOpened = true;
        minionManager.slotid = slot;
        MinionSelectInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
    }
    public void CloseMinionSelect()
    {
        minionListOpened = false;
        MinionSelectInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
        SwitchMinionInterface(1);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inventory
    public bool inventoryOpened;
    public void SwitchInventory(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                SwitchMainInterface(0);
                inventoryOpened = true;
                inventory.SortInventory();
                InventoryWindow_OM.MoveTo(new Vector2(0, 290), 0.3f, 1, false);
                break;
            case 0:
                if (saleWindowOpened) { SwitchSaleWindow(0); }
                SwitchMainInterface(1);
                inventoryOpened = false;
                InventoryWindow_OM.MoveTo(new Vector2(0, 1480), 0.3f, 1, false);
                InventoryGridPanel_CSwipe.SetBack();
                inventory.SetItemsBack();
                SwitchMainInterface(1);
                break;
            default:
                SwitchInventory(!inventoryOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Sale Window
    public bool saleWindowOpened;
    public void SwitchSaleWindow(int mode)
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
                    saleWindowOpened = true;
                    SaleWindow_OM.MoveTo(new Vector2(0, -305f), 0.3f, 1, false);

                    SaleForCurrencyPrice_Text.text = "";
                    SaleForXpPrice_Text.text = "";
                    SaleDescription_Text.text = "Select items, you want to sell";

                    inventory.ableToInvest = true;
                }
                break;
            case 0:
                saleWindowOpened = false;
                inventory.SetItemsBack();
                inventory.ableToInvest = false;
                SaleWindow_OM.MoveTo(new Vector2(0, -1200), 0.3f, 1, false);
                break;
            default:
                SwitchSaleWindow(!saleWindowOpened ? 1 : 0);
                break;
        }
    }
    public void ResetMultiSellButtonText()
    {
        SaleForXpPrice_Text.text = "";
        SaleForCurrencyPrice_Text.text = "";
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Item Info
    public bool itemInfoOpened;
    public void SwitchItemInfo(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                itemInfoOpened = true;
                ItemInfoWindow_OM.MoveTo(new Vector2(0, 110), 0.3f, 1, false);

                Item item = inventory.SelectedItem.GetComponent<Item>();

                SellForCurrencyAgree.SetActive(true);
                SellForXpAgree.SetActive(true);
                ItemOptionsUse.SetActive(true);

                if (item.useMethodName == "")
                {
                    ItemOptionsUse.SetActive(false);
                }
                ItemInfo_Image.sprite = item.ico.sprite;
                ItemInfoDescription_Text.text = "Type : " + item.type + " \n Name: " + item.nameObject + " \n Price: " + FormatNumsHelper.FormatNumF1(item.xpPrice);
                ItemInfoCount_Text.text = FormatNumsHelper.FormatNumF0F1(item.count);
                break;
            case 0:
                itemInfoOpened = false;
                ItemInfoWindow_OM.MoveTo(new Vector2(720, 110), 0.3f, 1, false);
                break;
            default:
                SwitchItemInfo(!itemInfoOpened ? 1 : 0);
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Set Count Menu
    public bool setCountOpened;
    public void OnCountSliderValChange()
    {
        if (inventory.SelectedItem != null)
        {
            Item item = inventory.SelectedItem.GetComponent<Item>();

            if (typeOfAction == "Use")
            {
                ItemPrices_Text.text = $" Do you wanna use your {FormatNumsHelper.FormatNumF0F1(Count_Slider.value)}\n\"{item.nameObject}\" ?";
                return;
            }
            if (typeOfAction == "Sell")
            {
                ItemPrices_Text.text = " Do you wanna sell your " + FormatNumsHelper.FormatNumF0F1(Count_Slider.value) +
                                      $"\n\"{item.nameObject}\" ?" +
                                       "\nfor " + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.xpPrice) + " experience ?" +
                                       "\n or $" + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.currencyPrice) + " ?";
                SellForCurrencyPrice_Text.text = "+" + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.currencyPrice);
                SellForXpPrice_Text.text = "+" + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.xpPrice);
                return;
            }
        }
    }
    public void OpenItemUsePanel()
    {
        CloseAll();
        setCountOpened = true;
        Item item = inventory.SelectedItem.GetComponent<Item>();
        if (item.type == "Toy")
        {
            SwitchItemInfo(0); SwitchInventory(0); SwitchMinionInterface(1);
            return;
        }
        if (item.type == "Cloth")
        {
            SwitchItemInfo(0); SwitchInventory(0); SwitchWashingmashine(1);
            return;
        }
        SetCountWindow_OM.MoveTo(new Vector2(0, 110), 0.3f, 1, false);

        typeOfAction = "Use";

        UseAgree.SetActive(true);
        SellForCurrencyAgree.SetActive(false);
        SellForXpAgree.SetActive(false);


        Count_Slider.maxValue = item.count;
        if (item.count > 1000)
        {
            Count_Slider.maxValue = 1000;
        }
        Count_Slider.value = 1;
        OnCountSliderValChange();
    }
    public void OpenItemSellPanel()
    {
        CloseAll();
        setCountOpened = true;
        ItemInfoWindow_OM.MoveTo(new Vector2(720, 110), 0.3f, 1, false);
        SetCountWindow_OM.MoveTo(new Vector2(0, 110), 0.3f, 1, false);
        InventoryWindow_OM.MoveTo(new Vector2(0, 1480), 0.3f, 1, false);

        Item item = inventory.SelectedItem.GetComponent<Item>();

        typeOfAction = "Sell";

        UseAgree.SetActive(false);
        SellForCurrencyAgree.SetActive(true);
        SellForXpAgree.SetActive(true);

        SellForCurrencyPrice_Text.text = "+" + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.currencyPrice);
        SellForXpPrice_Text.text = "+" + FormatNumsHelper.FormatNumF1(Count_Slider.value * item.xpPrice);

        Count_Slider.maxValue = item.count;
        Count_Slider.value = 1;
        OnCountSliderValChange();
    }
    public void CloseSetCount()
    {
        setCountOpened = false;
        SetCountWindow_OM.MoveTo(new Vector2(-720, 110), 0.3f, 1, false);
    }
}