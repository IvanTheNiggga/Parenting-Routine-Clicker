using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    #region Аppointed through the inspector
    [SerializeField] private Sprite MainGameSprite;
    [SerializeField] private Sprite MinerSprite;
    [SerializeField] private GameObject Upgrade_Prefab;

    [SerializeField] private Text currencyText;
    [SerializeField] private Text experienceText;
    [SerializeField] private Text stageText;
    #endregion

    #region Аppointed on start
    private Inventory inventory;
    private Clicker clicker;
    private EnemyManager enemyManager;
    private Message message;
    private UnitManager unitManager;
    private StagesManager stagesManager;
    private UpgradesManager upgradesManager;
    private Miner miner;

    private GameObject InventoryGrid;
    private GameObject UpgradesShopGrid;
    private GameObject SellItemForCurrency;
    private GameObject SellItemForXp;
    private GameObject UseItem;
    private GameObject ItemInfoUse;
    private GameObject ClickablePanel;

    private ObjectMovement InventoryWindow_OM;
    private ObjectMovement MainInterface_OM;
    private ObjectMovement ConfrimationWindow_OM;
    private ObjectMovement EnemyParent_OM;
    private ObjectMovement SettingsWindow_OM;
    private ObjectMovement ItemInfoWindow_OM;
    private ObjectMovement MultiSellWindow_OM;
    private ObjectMovement UnitInterface_OM;
    private ObjectMovement UnitSelectInterface_OM;
    private ObjectMovement UpgradesShopInterface_OM;
    private ObjectMovement SetCountWindow_OM;
    private ObjectMovement MinerInterface_OM;

    private ContentSwipe InventoryContent;

    private InputField ItemActionInputField;

    private Image ItemInfoImage;
    private Image Location_Image;
    private Image SetCount_Image;

    private Text SellForCurrencyPrice_Text;
    private Text SellForXpPrice_Text;
    private Text AgreeDescription_Text;
    private Text ItemInfoCount_Text;
    private Text ItemInfoDescription_Text;
    private Text SaleForCurrencyPrice_Text;
    private Text SaleForXpPrice_Text;
    private Text Unit1LockedText;
    private Text Unit2LockedText;
    #endregion

    #region Variables
    public string CurrentItemEventName;
    public string CurrentEventName;
    #endregion

    #region Init
    private void Start()
    {
        InitializeComponents();
        AddUpgrades();

        AdjustTopInsideSafeArea();
        Invoke(nameof(UpdateAllText), 0.5f);
    }

    private void InitializeComponents()
    {
        clicker = FindObjectOfType<Clicker>().GetComponent<Clicker>();
        enemyManager = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
        inventory = FindObjectOfType<Inventory>().GetComponent<Inventory>();
        upgradesManager = FindObjectOfType<UpgradesManager>().GetComponent<UpgradesManager>();
        unitManager = FindObjectOfType<UnitManager>().GetComponent<UnitManager>();
        stagesManager = FindObjectOfType<StagesManager>().GetComponent<StagesManager>();
        message = FindObjectOfType<Message>().GetComponent<Message>();
        miner = FindObjectOfType<Miner>().GetComponent<Miner>();

        InventoryContent = GameObject.Find("InventoryGridPanel").GetComponent<ContentSwipe>();

        Location_Image = GameObject.Find("Location(img)").GetComponent<Image>();
        ItemInfoImage = GameObject.Find("ItemInfo(img)").GetComponent<Image>();
        SetCount_Image = GameObject.Find("SetCount(img)").GetComponent<Image>();

        InventoryGrid = GameObject.Find("InventoryGrid");
        UpgradesShopGrid = GameObject.Find("UpgradesGrid");
        SellItemForCurrency = GameObject.Find("SetCountCurrency(btn)");
        SellItemForXp = GameObject.Find("SetCountXp(btn)");
        UseItem = GameObject.Find("SetCountAgree(btn)");
        ClickablePanel = GameObject.Find("Clickable(cdr)");
        ItemInfoUse = GameObject.Find("ItemActionUse(btn)");

        ConfrimationWindow_OM = GameObject.Find("ActionConfirmation").GetComponent<ObjectMovement>();
        EnemyParent_OM = GameObject.Find("Enemy Parent").GetComponent<ObjectMovement>();
        MultiSellWindow_OM = GameObject.Find("Sale").GetComponent<ObjectMovement>();
        InventoryWindow_OM = GameObject.Find("Inventory").GetComponent<ObjectMovement>();
        ItemInfoWindow_OM = GameObject.Find("ItemInfo").GetComponent<ObjectMovement>();
        MainInterface_OM = GameObject.Find("Battle Interface").GetComponent<ObjectMovement>();
        UnitSelectInterface_OM = GameObject.Find("UnitSelect Interface").GetComponent<ObjectMovement>();
        UnitInterface_OM = GameObject.Find("Units Interface").GetComponent<ObjectMovement>();
        SetCountWindow_OM = GameObject.Find("SetCount").GetComponent<ObjectMovement>();
        SettingsWindow_OM = GameObject.Find("Settings").GetComponent<ObjectMovement>();
        UpgradesShopInterface_OM = GameObject.Find("Upgrades Interface").GetComponent<ObjectMovement>();
        MinerInterface_OM = GameObject.Find("Miner Interface").GetComponent<ObjectMovement>();

        ItemActionInputField = GameObject.Find("InputField").GetComponent<InputField>();

        AgreeDescription_Text = GameObject.Find("Agree(txt)").GetComponent<Text>();
        SellForCurrencyPrice_Text = GameObject.Find("SetCountCurrency(txt)").GetComponent<Text>();
        SellForXpPrice_Text = GameObject.Find("SetCountXp(txt)").GetComponent<Text>();
        ItemInfoCount_Text = GameObject.Find("ItemCount(txt)").GetComponent<Text>();
        ItemInfoDescription_Text = GameObject.Find("ItemInfo(txt)").GetComponent<Text>();
        SaleForCurrencyPrice_Text = GameObject.Find("SaleForCurrency(txt)").GetComponent<Text>();
        SaleForXpPrice_Text = GameObject.Find("SaleForXp(txt)").GetComponent<Text>();
        Unit1LockedText = GameObject.Find("Unit1Locked(lbl)").GetComponent<Text>();
        Unit2LockedText = GameObject.Find("Unit2Locked(lbl)").GetComponent<Text>();
    }
    #endregion
    public void CloseAll()
    {
        if (agreeWindowOpened) { SwitchConfirmation(0); }
        if (settingsOpened) { SwitchSettings(0); }
        if (minerOpened) { MinerInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false); }
        if (upgradesOpened) { SwitchUpgradesMenu(0); }
        if (unitsListOpened) { CloseUnitsSelect(); }
        if (unitsInterfaceOpened) { SwitchUnitsInterface(0); }
        if (inventoryOpened) { SwitchInventory(0); }
        if (saleOpened) { SwitchSale(0); }
        if (setCountOpened) { CloseSetCount(); }
        if (itemInfoOpened) { SwitchItemInfo(0); }
        ResetSelectedItem();
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
                    enemyManager.clickable = true;
                    enemyManager.able = true;
                    ClickablePanel.GetComponent<Image>().raycastTarget = true;

                    MainInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                    EnemyParent_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
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
                    enemyManager.clickable = false;
                    enemyManager.able = false;
                    ClickablePanel.GetComponent<Image>().raycastTarget = false;

                    MainInterface_OM.MoveTo(new Vector2(-720, 0), 0.3f, 1, false);
                    EnemyParent_OM.MoveTo(new Vector2(-360, 0), 0.3f, 1, false);

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
                switch (CurrentEventName)
                {
                    case "birth":
                        AgreeDescription_Text.text = $"Make another one child for {NumFormat.FormatNumF1(unitManager.BirthCost())}$ ?";
                        break;
                    case "minerupgrade":
                        if (inventory.ItemTypeFind(ItemTypes.Cloth) == null)
                        {
                            message.SendMessage($"You need at least one cloth in inventory", 2);
                            return;
                        }
                        else if (clicker.MinerLvl == 0)
                        {
                            message.SendMessage($"Fix washing mashine at first", 2);
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade washing mashine for Cloth item?";
                        break;
                    case "unitupgrade1":
                        if (inventory.ItemTypeFind(ItemTypes.Toy) == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            return;
                        }
                        else if (clicker.Births < 1)
                        {
                            message.SendMessage($"You need at least one birth", 2);
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade fisrt child for one Toy item ?";
                        SwitchUnitsInterface(0);
                        break;
                    case "unitupgrade2":
                        if (inventory.ItemTypeFind(ItemTypes.Toy) == null)
                        {
                            message.SendMessage($"You need at least one toy in inventory", 2);
                            return;
                        }
                        else if (clicker.Births < 2)
                        {
                            message.SendMessage($"You need at least two births", 2);
                            return;
                        }
                        AgreeDescription_Text.text = $"Upgrade second child for one Toy item ?";
                        SwitchUnitsInterface(0);
                        break;
                }
                CloseAll();
                agreeWindowOpened = true;
                ConfrimationWindow_OM.MoveTo(new Vector2(0, 90), 0.3f, 1, false);
                SwitchBattleInterface(0);
                break;
            case 0:
                agreeWindowOpened = false;
                ConfrimationWindow_OM.MoveTo(new Vector2(-720, 90), 0.3f, 1, false);
                SwitchBattleInterface(1);
                break;
            default:
                SwitchConfirmation(!agreeWindowOpened ? 1 : 0);
                break;
        }
    }
    public void Confirm()
    {
        switch (CurrentEventName)
        {
            case "birth":
                if (unitManager.isAbleToBuy())
                {
                    clicker.Birth();
                }
                else
                {
                    message.SendMessage($"You need more money", 2);
                }
                break;
            case "minerupgrade":
                miner.UpgradeMiner();
                inventory.ConsumeAnyItemOfType(ItemTypes.Cloth);
                break;
            case "unitupgrade1":
                unitManager.UpgradeUnit(1);
                inventory.ConsumeAnyItemOfType(ItemTypes.Toy);
                break;
            case "unitupgrade2":
                unitManager.UpgradeUnit(2);
                inventory.ConsumeAnyItemOfType(ItemTypes.Toy);
                break;
        }
        SwitchBattleInterface(1);
    }

    public void Action(string s)
    {
        CurrentEventName = s;
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
                Location_Image.sprite = MainGameSprite;
                break;
            case 0:
                CloseAll();
                minerOpened = false;
                SwitchBattleInterface(1);

                MinerInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);

                stagesManager.ChangeAmbience(true);
                Location_Image.sprite = MinerSprite;
                break;
            default:
                SwitchMiner(!minerOpened ? 1 : 0);
                break;
        }
    }
    #endregion

    #region Upgrades Menu
    public bool upgradesOpened;
    public void SwitchUpgradesMenu(int mode)
    {
        switch (mode)
        {
            case 1:
                CloseAll();
                upgradesOpened = true;
                UpgradesShopInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);
                SwitchBattleInterface(0);
                break;
            case 0:
                upgradesOpened = false;
                UpgradesShopInterface_OM.MoveTo(new Vector2(720, 0), 0.3f, 1, false);
                SwitchBattleInterface(1);
                break;
            default:
                SwitchUpgradesMenu(!upgradesOpened ? 1 : 0);
                break;
        }
    }
    private void AddUpgrades()
    {
        for (int i = 0; i < upgradesManager.UpgradesDataBase.Count; i++)
        {
            UpgradeObject upgrade = Instantiate(Upgrade_Prefab, UpgradesShopGrid.transform).GetComponent<UpgradeObject>();
            upgrade.Index = i;
            upgrade.AddGraphics();
        }
    }

    public void UpdateUpgrades()
    {
        upgradesManager = GameObject.Find("ClickerManager").GetComponent<UpgradesManager>();

        for (int i = 0; i < upgradesManager.UpgradesDataBase.Count; i++)
        {
            UpgradeObject UpgradeObjectTemp = GameObject.Find(upgradesManager.UpgradesDataBase[i].upgradeName).GetComponent<UpgradeObject>();
            UpgradeObjectTemp.AddGraphics();
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
                CloseAll();

                unitsInterfaceOpened = true;
                switch (clicker.Births)
                {
                    case 0:
                        Unit1LockedText.text = "1 Birth required.";
                        Unit2LockedText.text = "2 Birth required.";
                        break;
                    case 1:
                        Unit1LockedText.text = "";
                        Unit2LockedText.text = "2 Birth required.";
                        break;
                    default:
                        Unit1LockedText.text = "";
                        Unit2LockedText.text = "";
                        break;
                }
                UnitInterface_OM.MoveTo(new Vector2(0, 0), 0.3f, 1, false);

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
            message.SendMessage($"You need {slot - clicker.Births} more births", 2);
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
                if (setCountOpened) { CloseSetCount(); }
                SwitchBattleInterface(1);
                inventoryOpened = false;

                InventoryContent.SetBack();
                inventory.SetItemsBack();

                InventoryWindow_OM.MoveTo(new Vector2(0, 1480), 0.3f, 1, false);

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
                }
                else
                {
                    inventory.ableToInvest = true;
                    saleOpened = true;

                    SaleForCurrencyPrice_Text.text = "0";
                    SaleForXpPrice_Text.text = "0";

                    MultiSellWindow_OM.MoveTo(new Vector2(0, -300f), 0.3f, 1, false);
                }
                break;
            case 0:
                saleOpened = false;
                inventory.ableToInvest = false;

                inventory.SetItemsBack();

                MultiSellWindow_OM.MoveTo(new Vector2(0, -1200), 0.3f, 1, false);
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
                Item item = inventory.SelectedItem.GetComponent<Item>();

                itemInfoOpened = true;

                SellItemForCurrency.SetActive(true);
                SellItemForXp.SetActive(true);
                ItemInfoUse.SetActive(true);
                if (item.useMethodName == "")
                {
                    ItemInfoUse.SetActive(false);
                }

                ItemInfoImage.sprite = item.ico.sprite;
                ItemInfoDescription_Text.text = "Type : " + item.type + " \n Name: " + item.nameObject;
                ItemInfoCount_Text.text = NumFormat.FormatNumF0F1(item.count);

                ItemInfoWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);
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
            if (ItemActionInputField.text.Length > 0)
            {
                input = int.Parse(ItemActionInputField.text);
                input = input > item.count ? item.count : input;
            }

            if (CurrentItemEventName == "Sell")
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
            ItemActionInputField.text = item.count.ToString();
            return;
        }
    }
    public void OpenItemUsePanel()
    {
        Item item = inventory.SelectedItem.GetComponent<Item>();


        switch (item.type)
        {
            case ItemTypes.Toy:
                SwitchUnitsInterface(1);
                break;
            case ItemTypes.Cloth:
                SwitchMiner(1);
                break;
            default:
                setCountOpened = true;
                UseItem.SetActive(true);
                SellItemForCurrency.SetActive(false);
                SellItemForXp.SetActive(false);
                CurrentItemEventName = "Use";

                SwitchItemInfo(0);
                ItemActionInputField.text = "1";
                SetCount_Image.sprite = inventory.SelectedItem.GetComponent<Item>().ico.sprite;
                SetCountUpdate();

                SetCountWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);
                break;
        }
    }
    public void OpenItemSellPanel()
    {
        Item item = inventory.SelectedItem.GetComponent<Item>();

        SwitchItemInfo(0);
        setCountOpened = true;
        CurrentItemEventName = "Sell";

        UseItem.SetActive(false);
        SellItemForCurrency.SetActive(true);
        SellItemForXp.SetActive(true);

        ItemActionInputField.text = "1";
        SellForCurrencyPrice_Text.text = "+" + NumFormat.FormatNumF1(item.currencyPrice);
        SellForXpPrice_Text.text = "+" + NumFormat.FormatNumF1(item.xpPrice);
        SetCount_Image.sprite = inventory.SelectedItem.GetComponent<Item>().ico.sprite;
        SetCountUpdate();

        SetCountWindow_OM.MoveTo(new Vector2(0, -200), 0.3f, 1, false);
    }
    public void CloseSetCount()
    {
        setCountOpened = false;
        CurrentItemEventName = "";
        SetCountWindow_OM.MoveTo(new Vector2(-720, -200), 0.3f, 1, false);
    }
    public void ResetSelectedItem()
    { inventory.SelectedItem = null; }
    #endregion

    #region Text Management
    private void AdjustTopInsideSafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Проверяем, находится ли объект вне безопасной зоны
        if (!safeArea.Contains(currencyText.transform.position))
        {
            Vector3 panelOldPos = currencyText.transform.parent.position;
            Vector3 oldPos = currencyText.transform.position;
            float newY = Mathf.Clamp(oldPos.y, safeArea.yMin, safeArea.yMax);
            float difference = Mathf.Abs(oldPos.y - newY) * 1.7f;

            // Применяем новую позицию
            currencyText.transform.parent.position = panelOldPos - (Vector3.up * difference);
        }
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
        UpdateUpgrades();
    }
    #endregion
}