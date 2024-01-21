using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    #region Appointed on start
    private Clicker clicker;
    private RewardManager giveReward;
    private Inventory inventory;
    private StagesManager stagesManager;
    private SoundManager soundManager;
    private InterfaceManager interfaceManager;

    public Text text;
    public Image ico;

    private Text SaleForCurrencyPrice_Text;
    private Text SaleForXpPrice_Text;

    public GameObject item;
    public GameObject SaleGrid;
    public GameObject InventoryGrid;

    private Items itemData;

    public int stage;
    public int index;
    public string nameObject;
    public string itemName;
    public string description;
    public string investItemName;
    public string slotItemName;
    public ItemTypes type;
    public double currencyPrice;
    public float xpPrice;
    public string useMethodName;
    #endregion

    #region Variables
    public int count;
    private int taps;
    public bool clickable;
    bool Loaded;
    #endregion

    #region Base
    public void Start()
    {
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        transform.localScale = new Vector3(1, 1, 1);
        taps = 0;
        clickable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickable == true)
        {

            if (interfaceManager.saleOpened)
            {
                AddToInvestGrid();
                MultiSellAddgraphics();
            }
            else
            {
                if (inventory.SelectedItem == gameObject)
                {
                    interfaceManager.SwitchItemInfo(0);
                    inventory.SelectedItem = null;
                }
                else if (interfaceManager.setCountOpened)
                {
                    if (interfaceManager.CurrentItemEventName == "Sell")
                    {
                        inventory.SelectedItem = gameObject;
                        interfaceManager.OpenItemSellPanel();
                    }
                    if (interfaceManager.CurrentItemEventName == "Use")
                    {
                        if(useMethodName != "")
                        {
                            inventory.SelectedItem = gameObject;
                            interfaceManager.OpenItemUsePanel();
                        }
                    }
                }
                else
                {
                    inventory.SelectedItem = gameObject;
                    interfaceManager.SwitchItemInfo(1);
                    interfaceManager.SwitchBattleInterface(0);
                }
            }
            soundManager.PlayClickSound();
        }
    }

    private void DestroyOnEmpty()
    {
        if (count <= 0 && Loaded)
        {
            if (name == investItemName)
            {
                inventory.investItems.Remove(this);
            }
            else
            {
                string keyName = $"{itemName}Count";
                PlayerPrefs.DeleteKey(keyName);
                inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
                inventory.SelectedItem = null;
                inventory.items.Remove(this);
            }
            Destroy(gameObject);
            inventory.SortInventory();
        }
    }
    private void OnBecameInvisible()
    {
        clickable = false;
        GetComponent<Image>().raycastTarget = false;
    }
    private void OnBecameVisible()
    {
        clickable = true;
        GetComponent<Image>().raycastTarget = true;
    }
    void CheckName()
    {
        if (name == itemName)
        {
            inventory.items.Add(this);
            gameObject.transform.SetParent(InventoryGrid.transform);
        }
        else if (name == investItemName)
        {
            inventory.investItems.Add(this);
            gameObject.transform.SetParent(SaleGrid.transform);
        }
    }
    #endregion

    #region UI/Data
    public void AddGraphics()
    {
        if (!Loaded)
        {
            stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

            itemData = stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[index];
            itemName = $"Item_S{stage}ID{index}";
            investItemName = $"InvestItem_S{stage}ID{index}";
            description = itemData.description;
            SaleGrid = GameObject.Find("SaleGrid");
            InventoryGrid = GameObject.Find("InventoryGrid");
            inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
            clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
            giveReward = GameObject.Find("ClickerManager").GetComponent<RewardManager>();

            ico.sprite = itemData.ico;
            nameObject = itemData.nameObject;
            type = itemData.type;
            UpdatePrice();
            xpPrice = itemData.xpPrice;
            useMethodName = itemData.useMethodName;

            CheckName();
            Loaded = true;
        }

        DestroyOnEmpty();
        if (name == itemName && count > 0)
        {
            string keyName = $"{itemName}Count";
            PlayerPrefs.SetInt(keyName, count);
        }
        text.text = NumFormat.FormatNumF0F1(count);
    }

    public void UpdatePrice()
    {
        currencyPrice = giveReward.KillReward * itemData.currencyPrice;
    }

    public void MultiSellAddgraphics()
    {
        SaleForCurrencyPrice_Text = GameObject.Find("SaleForCurrency(txt)").GetComponent<Text>();
        SaleForXpPrice_Text = GameObject.Find("SaleForXp(txt)").GetComponent<Text>();

        int itemsCount = 0;
        float esPrice = 0;
        double pcPrice = 0;
        for (int i = 0; i < inventory.investItems.Count; i++)
        {
            Item item = SaleGrid.transform.GetChild(i).GetComponent<Item>();
            itemsCount += item.count;
            esPrice += item.count * item.xpPrice;
            pcPrice += item.count * item.currencyPrice;
        }
        if (itemsCount == 0)
        {
            SaleForCurrencyPrice_Text.text = "0";
            SaleForXpPrice_Text.text = "0";
        }
        else
        {
            SaleForCurrencyPrice_Text.text = "+" + NumFormat.FormatNumF1(pcPrice);
            SaleForXpPrice_Text.text = "+" + NumFormat.FormatNumF1(esPrice);
        }
    }
    #endregion

    #region Item Manipulations
    public void Use()
    {
        inventory.Invoke(useMethodName, 0f);
    }
    public void AddToInvestGrid()
    {
        taps++;
        Invoke(nameof(TapEquals0), 0.2f);
        if (taps >= 2)
        {
            GameObject g = name == itemName ? GameObject.Find(itemName) : GameObject.Find(investItemName);

            inventory.MoveItem(this, count);

            g.GetComponent<Item>().AddGraphics();

            taps = 0;
        }
        else
        {
            inventory.MoveItem(this, 1);
        }
        AddGraphics();
        inventory.SortInventory();
    }
    public void TapEquals0() => taps = 0;
    #endregion
}

