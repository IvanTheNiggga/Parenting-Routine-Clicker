using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new();
    public List<Item> investItems = new();

    private Clicker clicker;
    private RewardManager giveReward;
    private InterfaceManager interfaceManager;
    private StagesManager stagesManager;
    private SoundManager soundManager;
    private Miner miner;

    private ContentSwipe InventoryGridPanel_CSwipe;
    private ContentSwipe SaleGridPanel_CSwipe;

    public GameObject DroppedItem_Prefab;
    public GameObject Item_Prefab;

    private GameObject CurrencyParent;
    private GameObject InventoryGrid;
    private GameObject SaleGrid;
    public GameObject SelectedItem;

    private Slider Count_Slider;

    private Text Currency_Text;
    private Text Experience_Text;
    private Text SaleDescription_Text;
    private Text SaleForCurrency_Text;
    private Text SaleForXp_Text;

    public bool ableToInvest;

    public void Start()
    {
        ableToInvest = false;

        stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        giveReward = GameObject.Find("ClickerManager").GetComponent<RewardManager>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        Count_Slider = GameObject.Find("SetCount(sld)").GetComponent<Slider>();

        SaleDescription_Text = GameObject.Find("Sale(lbl)").GetComponent<Text>();
        Currency_Text = GameObject.Find("Currency(txt)").GetComponent<Text>();
        Experience_Text = GameObject.Find("Experience(txt)").GetComponent<Text>();


        SaleGrid = GameObject.Find("SaleGrid");
        InventoryGrid = GameObject.Find("InventoryGrid");
        CurrencyParent = GameObject.Find("Drop Parent");

        miner = GameObject.Find("Miner").GetComponent<Miner>();

        SaleForCurrency_Text = GameObject.Find("SaleForCurrency(txt)").GetComponent<Text>();
        SaleForXp_Text = GameObject.Find("SaleForXp(txt)").GetComponent<Text>();
    }
    void CheckFloors()
    {
        InventoryGridPanel_CSwipe.CheckFloor();
        SaleGridPanel_CSwipe.CheckFloor();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Item manipulation
    public Item ItemTypeFind(string itemtype)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].type == itemtype)
            {
                return items[i];
            }

        }
        return null;
    }
    public bool ConsumeItem(string itemname)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].nameObject == itemname)
            {
                items[i].count--;
                items[i].AddGraphics();
                return true;
            }
        }
        return false;
    }
    public bool ConsumeAnyItemOfType(string itemtype)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].type == itemtype)
            {
                items[i].count--;
                items[i].AddGraphics();
                return true;
            }
        }
        return false;
    }
    public void DeleteItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].count = 0;
        }
    }
    public void SetItemsBack()
    {
        if (investItems.Count > 0)
        {
            for (int i = 0; i < investItems.Count; i++)
            {
                MoveItem(investItems[i], investItems[i].count);
            }
        }
    }
    public void MoveItem(Item itemScript, int count)
    {
        if (itemScript.name == itemScript.itemName)
        {
            if (GameObject.Find(itemScript.investItemName) == null)
            {
                GameObject i = Instantiate(Item_Prefab);
                Item g = i.GetComponent<Item>();
                g.name = itemScript.investItemName;
                g.count = count;
                g.index = itemScript.index;
                g.stage = itemScript.stage;
                g.AddGraphics();
                itemScript.count -= count;
                SortInventory();
            }
            else
            {
                GameObject i = GameObject.Find(itemScript.investItemName);
                Item g = i.GetComponent<Item>();
                g.count += count;
                itemScript.count -= count;
                g.AddGraphics();
            }
        }
        else
        {
            AddItem(itemScript.stage, itemScript.index, count);
            itemScript.count -= count;
            itemScript.AddGraphics();
        }
    }
    public void SortInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.SetAsFirstSibling();
        }
        InventoryGridPanel_CSwipe = GameObject.Find("InventoryGridPanel").GetComponent<ContentSwipe>();
        SaleGridPanel_CSwipe = GameObject.Find("SaleGridPanel").GetComponent<ContentSwipe>();

        Invoke(nameof(CheckFloors), 0.01f);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Item sell/use
    public void MultiSellForXP()
    {
        for (int i = investItems.Count - 1; i >= 0; i--)
        {
            Item item = investItems[i];
            clicker.Experience += item.count * item.xpPrice;
            Experience_Text.text = NumFormat.FormatNumF1(clicker.Experience);
            investItems[i].count = 0;
            investItems[i].AddGraphics();

            SaleDescription_Text.text = "Select items, you want to sell";
            SaleForCurrency_Text.text = "";
            SaleForXp_Text.text = "";
        }
        SortInventory();
        soundManager.PlayBuySound();
    }
    public void MultiSellForCurrency()
    {
        for (int i = investItems.Count - 1; i >= 0; i--)
        {
            Item item = investItems[i];
            clicker.Currency += item.count * item.currencyPrice;
            Currency_Text.text = NumFormat.FormatNumF1(clicker.Currency);
            investItems[i].count = 0;
            investItems[i].AddGraphics();

            SaleDescription_Text.text = "Select items, you want to sell";
            SaleForCurrency_Text.text = "";
            SaleForXp_Text.text = "";
        }
        SortInventory();
        soundManager.PlayBuySound();
    }
    public void SellForXP()
    {
        if (SelectedItem != null)
        {
            Item item = SelectedItem.GetComponent<Item>();
            if (interfaceManager.typeOfAction == "Buy")
            {
                clicker.Experience -= (int)Count_Slider.value * item.xpPrice;
                Experience_Text.text = NumFormat.FormatNumF1(clicker.Experience);
                AddItem(item.stage, item.index, (int)Count_Slider.value);
                item.AddGraphics();
                Count_Slider.maxValue = Mathf.FloorToInt(clicker.Experience / item.xpPrice);

                if (clicker.Experience < item.xpPrice)
                {
                    interfaceManager.SwitchInventory(1);
                }
                return;
            }
            if (interfaceManager.typeOfAction == "Sell")
            {
                if (SelectedItem == null)
                {
                    interfaceManager.SwitchInventory(1);
                }
                else
                {
                    clicker.Experience += (int)Count_Slider.value * item.xpPrice;
                    Experience_Text.text = NumFormat.FormatNumF1(clicker.Experience);
                    item.count -= (int)Count_Slider.value;
                    item.AddGraphics();
                    Count_Slider.maxValue = item.count;
                }
            }
            soundManager.PlayBuySound();
        }
        if (SelectedItem == null)
        {
            interfaceManager.SwitchInventory(1);
        }
    }
    public void SellForCurrency()
    {
        if (SelectedItem != null)
        {
            Item item = SelectedItem.GetComponent<Item>();
            clicker.Currency += (int)Count_Slider.value * item.currencyPrice;
            Currency_Text.text = NumFormat.FormatNumF1(clicker.Currency);
            item.count -= (int)Count_Slider.value;
            item.AddGraphics();
            Count_Slider.maxValue = item.count;
            soundManager.PlayBuySound();
        }
        if (SelectedItem == null)
        {
            interfaceManager.SwitchInventory(1);
        }
    }
    public void Use()
    {
        if (SelectedItem != null)
        {
            int ii = (int)Count_Slider.value;
            Item item = SelectedItem.GetComponent<Item>();
            for (int i = 0; i < ii; i++)
            {
                item.Use();
            }

            item.count -= ii;
            item.AddGraphics();

            if (item.count > 1000)
            {
                Count_Slider.maxValue = 1000;
            }
            else
            {
                Count_Slider.maxValue = item.count;
            }
        }
        if (SelectedItem == null)
        {
            interfaceManager.SwitchInventory(1);
        }
    }
    public void UseCrate()
    {
        giveReward.GetRandomItem(1);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Item spawning
    public void AddItem(int stage, int id, int count)
    {
        string gameObjectName = $"Item_S{stage}ID{id}";

        if (GameObject.Find(gameObjectName) != null)
        {
            GameObject g = GameObject.Find(gameObjectName);
            Item it = g.GetComponent<Item>();
            it.count += count;
            it.AddGraphics();
        }
        else if (GameObject.Find(gameObjectName) == null)
        {
            GameObject g = Instantiate(Item_Prefab);
            Item it = g.GetComponent<Item>();
            it.name = gameObjectName;
            it.count = count;

            it.stage = stage;
            it.index = id;

            it.AddGraphics();
        }
        SortInventory(); 
    }
    public void SpawnItem(int stage, int index, int count)
    {
        DroppedItem it = Instantiate(DroppedItem_Prefab, CurrencyParent.transform).GetComponent<DroppedItem>();
        it.stage = stage;
        it.index = index;
        it.count = count;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// LOAD ITEMS
    public void ItemGetData()
    {
        stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

        for (int i = 0; i < stagesManager.StagesDataBase.Count; i++)
        {
            for (int ii = 0; ii < stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase.Count; ii++)
            {
                string keyName = $"Item_S{i}ID{ii}Count";

                if (PlayerPrefs.HasKey(keyName))
                {
                    int count = PlayerPrefs.GetInt(keyName);

                    AddItem(i, ii, count);
    }   }   }   }
}
