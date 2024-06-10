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

    private Text SaleForCurrencyPrice_Text;
    private Text SaleForXpPrice_Text;
    private GameObject SaleGrid;
    private GameObject InventoryGrid;

    public Text CountText;
    public Image Ico;

    public ItemPattern itemPattern;

    public string ItemName;
    public string ObjectName;
    public string InvestIObjectName;
    public string Description;
    public ItemTypes Type;

    public double CurrencyPrice;
    public float XpPrice;
    #endregion

    #region Variables
    public int Count;
    public bool Clickable;
    private int taps;
    bool loaded;
    #endregion

    #region Base
    public void Start()
    {
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        transform.localScale = new Vector3(1, 1, 1);
        taps = 0;
        Clickable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Clickable == true)
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
                        if(itemPattern.UseMethodName != "")
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
        if (Count <= 0 && loaded)
        {
            if (name == InvestIObjectName)
            {
                inventory.investItems.Remove(this);
            }
            else
            {
                string keyName = $"{ObjectName}Count";
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
        Clickable = false;
        GetComponent<Image>().raycastTarget = false;
    }
    private void OnBecameVisible()
    {
        Clickable = true;
        GetComponent<Image>().raycastTarget = true;
    }
    void CheckName()
    {
        if (name == ObjectName)
        {
            inventory.items.Add(this);
            gameObject.transform.SetParent(InventoryGrid.transform);
        }
        else if (name == InvestIObjectName)
        {
            inventory.investItems.Add(this);
            gameObject.transform.SetParent(SaleGrid.transform);
        }
    }
    #endregion

    #region UI/Data
    public void AddGraphics()
    {
        if (!loaded)
        {
            stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

            ObjectName = $"Item_{itemPattern.ID}";
            InvestIObjectName = $"InvestItem_{itemPattern.ID}";
            Description = itemPattern.Description;
            SaleGrid = GameObject.Find("SaleGrid");
            InventoryGrid = GameObject.Find("InventoryGrid");
            inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
            clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
            giveReward = GameObject.Find("ClickerManager").GetComponent<RewardManager>();

            Ico.sprite = itemPattern.ico;
            ItemName = itemPattern.ItemName;
            Type = itemPattern.Type;
            UpdatePrice();
            XpPrice = itemPattern.XpPrice;

            CheckName();
            loaded = true;
        }

        DestroyOnEmpty();
        if (name == ObjectName && Count > 0)
        {
            string keyName = $"{ObjectName}Count";
            PlayerPrefs.SetInt(keyName, Count);
        }
        CountText.text = NumFormat.FormatNumF0F1(Count);
    }

    public void UpdatePrice()
    {
        CurrencyPrice = giveReward.KillReward * itemPattern.CurrencyPrice;
    }

    public void MultiSellAddgraphics()
    {
        SaleForCurrencyPrice_Text = GameObject.Find("SaleForCurrency(txt)").GetComponent<Text>();
        SaleForXpPrice_Text = GameObject.Find("SaleForXp(txt)").GetComponent<Text>();

        int itemsCount = 0;
        float xpPrice = 0;
        double currencyPrice = 0;
        for (int i = 0; i < inventory.investItems.Count; i++)
        {
            Item item = SaleGrid.transform.GetChild(i).GetComponent<Item>();
            itemsCount += item.Count;
            xpPrice += item.Count * item.XpPrice;
            currencyPrice += item.Count * item.CurrencyPrice;
        }
        if (itemsCount == 0)
        {
            SaleForCurrencyPrice_Text.text = "0";
            SaleForXpPrice_Text.text = "0";
        }
        else
        {
            SaleForCurrencyPrice_Text.text = "+" + NumFormat.FormatNumF1(currencyPrice);
            SaleForXpPrice_Text.text = "+" + NumFormat.FormatNumF1(xpPrice);
        }
    }
    #endregion

    #region Item Manipulations
    public void Use()
    {
        inventory.Invoke(itemPattern.UseMethodName, 0f);
    }
    public void AddToInvestGrid()
    {
        taps++;
        Invoke(nameof(TapEquals0), 0.2f);
        if (taps >= 2)
        {
            GameObject g = name == ObjectName ? GameObject.Find(ObjectName) : GameObject.Find(InvestIObjectName);

            inventory.MoveItem(this, Count);

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

