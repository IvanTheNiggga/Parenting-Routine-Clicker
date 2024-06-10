using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    #region Appointed through the inspector
    public List<ItemPattern> GlobalItemsDataBase = new();
    public List<ItemPattern> ItemsDataBase()
    {
        if (!stagesManager) stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
        List<ItemPattern> combinedList = new List<ItemPattern>(GlobalItemsDataBase);
        foreach (Stage stage in stagesManager.StagesDataBase)
        {
            combinedList.AddRange(stage.ItemsDataBase);
        }
        return combinedList;
    }
    [SerializeField] private GameObject DroppedItem_Prefab;
    [SerializeField] private GameObject Item_Prefab;
    #endregion

    #region Appointed on start
    private Clicker clicker;
    private RewardManager giveReward;
    private InterfaceManager interfaceManager;
    private StagesManager stagesManager;
    private SoundManager soundManager;
    private Miner miner;

    private ContentSwipe InventoryGridPanel_CSwipe;
    private ContentSwipe SaleGridPanel_CSwipe;

    private GameObject CurrencyParent;
    private GameObject InventoryGrid;
    private GameObject SaleGrid;

    private InputField Count_Input;

    private Text Currency_Text;
    private Text Experience_Text;
    private Text SaleForCurrency_Text;
    private Text SaleForXp_Text;
    #endregion

    #region Variables
    public List<Item> items = new();
    public List<Item> investItems = new();
    public GameObject SelectedItem;
    public bool ableToInvest;
    #endregion

    #region Init
    public void Start()
    {
        ableToInvest = false;

        stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
        giveReward = GameObject.Find("ClickerManager").GetComponent<RewardManager>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        Count_Input = GameObject.Find("InputField").GetComponent<InputField>();

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
        InventoryGridPanel_CSwipe.GetCeilValue();
        SaleGridPanel_CSwipe.GetCeilValue();
    }
    #endregion

    #region Item manipulation
    public Item ItemTypeFind(ItemTypes itemtype)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Type == itemtype)
            {
                return items[i];
            }

        }
        return null;
    }
    public bool ConsumeItem(string itemname)
    {
        foreach (Item item in items)
        {
            if (item.ItemName == itemname)
            {
                item.Count--;
                item.AddGraphics();
                return true;
            }
        }
        return false;
    }
    public bool ConsumeAnyItemOfType(ItemTypes itemtype)
    {
        foreach (Item item in items)
        {
            if (item.Type == itemtype)
            {
                item.Count--;
                item.AddGraphics();
                return true;
            }
        }
        return false;
    }
    public void DeleteItems()
    {
        foreach (Item item in items) item.Count = 0;
    }
    public void SetItemsBack()
    {
        if (investItems.Count > 0)
        {
            foreach (Item item in investItems) MoveItem(item, item.Count);
        }
    }
    public void MoveItem(Item itemScript, int count)
    {
        if (itemScript.name == itemScript.ObjectName)
        {
            if (GameObject.Find(itemScript.InvestIObjectName) == null)
            {
                GameObject i = Instantiate(Item_Prefab);
                Item g = i.GetComponent<Item>();

                g.name = itemScript.InvestIObjectName;
                g.itemPattern = itemScript.itemPattern;
                g.Count = count;

                g.AddGraphics();
                itemScript.Count -= count;
                SortInventory();
            }
            else
            {
                GameObject i = GameObject.Find(itemScript.InvestIObjectName);
                Item g = i.GetComponent<Item>();
                g.Count += count;
                itemScript.Count -= count;
                g.AddGraphics();
            }
        }
        else
        {
            AddItem(itemScript.itemPattern, count);
            itemScript.Count -= count;
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

    public void UpdateItemPrices()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].GetComponent<Item>().UpdatePrice();
        }
    }
    #endregion

    #region Item sell/use
    public void MultiSellForXP()
    {
        for (int i = investItems.Count - 1; i >= 0; i--)
        {
            Item item = investItems[i];
            clicker.Experience += item.Count * item.XpPrice;
            Experience_Text.text = NumFormat.FormatNumF1(clicker.Experience);
            investItems[i].Count = 0;
            investItems[i].AddGraphics();

            SaleForCurrency_Text.text = "0";
            SaleForXp_Text.text = "0";
        }
        SortInventory();
        soundManager.PlayBuySound();
    }
    public void MultiSellForCurrency()
    {
        for (int i = investItems.Count - 1; i >= 0; i--)
        {
            Item item = investItems[i];
            clicker.Currency += item.Count * item.CurrencyPrice;
            Currency_Text.text = NumFormat.FormatNumF1(clicker.Currency);
            investItems[i].Count = 0;
            investItems[i].AddGraphics();

            SaleForCurrency_Text.text = "0";
            SaleForXp_Text.text = "0";
        }
        SortInventory();
        soundManager.PlayBuySound();
    }
    public void SellForXP()
    {
        if (SelectedItem != null)
        {
            Item item = SelectedItem.GetComponent<Item>();
            int input = 1;
            if (Count_Input.text.Length > 0)
            {
                input = int.Parse(Count_Input.text);
                input = input > item.Count ? item.Count : input;
            }

            if (SelectedItem == null)
            {
                interfaceManager.SwitchInventory(1);
            }
            else
            {
                clicker.Experience += input * item.XpPrice;
                Experience_Text.text = NumFormat.FormatNumF1(clicker.Experience);
                item.Count -= input;
                item.AddGraphics();
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
            int input = 1;
            if (Count_Input.text.Length > 0)
            {
                input = int.Parse(Count_Input.text);
                input = input > item.Count ? item.Count : input;
            }

            clicker.Currency += input * item.CurrencyPrice;
            Currency_Text.text = NumFormat.FormatNumF1(clicker.Currency);
            item.Count -= input;
            item.AddGraphics();
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
            Item item = SelectedItem.GetComponent<Item>();
            int input = 1;
            if (Count_Input.text.Length > 0)
            {
                input = int.Parse(Count_Input.text);
                input = input > item.Count ? item.Count : input;
            }

            for (int i = 0; i < input; i++)
            {
                item.Use();
            }

            item.Count -= input;
            item.AddGraphics();
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
    #endregion

    #region Item spawning
    public void AddItem(ItemPattern item, int count)
    {
        string gameObjectName = $"Item_{item.ID}";

        if (GameObject.Find(gameObjectName) != null)
        {
            GameObject g = GameObject.Find(gameObjectName);
            Item it = g.GetComponent<Item>();
            it.Count += count;
            it.AddGraphics();
        }
        else
        {
            GameObject g = Instantiate(Item_Prefab);
            Item it = g.GetComponent<Item>();

            it.itemPattern = item;
            it.name = gameObjectName;
            it.Count = count;

            it.AddGraphics();
        }
        SortInventory();
    }
    public void SpawnItem(ItemPattern item, int count)
    {
        DroppedItem it = Instantiate(DroppedItem_Prefab, CurrencyParent.transform).GetComponent<DroppedItem>();
        it.itemPattern = item;
        it.Count = count;
    }

    public void AddRandomItemByType(ItemTypes itemType, int count, bool localItem)
    {
        List<ItemPattern> filteredItems = new();
        List<ItemPattern> items = new();
        if (localItem) items = stagesManager.currentStage.ItemsDataBase;
        else items = GlobalItemsDataBase;

        foreach (ItemPattern item in items)
        {
            if (item.Type == itemType)
            {
                filteredItems.Add(item);
            }
        }

        if (filteredItems.Count == 0)
        {
            if (localItem)
            {
                AddRandomItemByType(itemType, count, false);
            }
        }
        else
        {
            int randomIndex = Random.Range(0, filteredItems.Count);
            AddItem(filteredItems[randomIndex], count);
        }
    }
    public void SpawnRandomItemByType(ItemTypes itemType, int count, bool localItem)
    {
        List<ItemPattern> filteredItems = new();
        List<ItemPattern> items = new();
        if (localItem) items = stagesManager.currentStage.ItemsDataBase;
        else items = GlobalItemsDataBase;

        foreach (ItemPattern item in items)
        {
            if (item.Type == itemType)
            {
                filteredItems.Add(item);
            }
        }

        if (filteredItems.Count == 0)
        {
            if (localItem)
            {
                SpawnRandomItemByType(itemType, count, false);
            }
        }
        else
        {
            int randomIndex = Random.Range(0, filteredItems.Count);
            SpawnItem(filteredItems[randomIndex], count);
        }
    }
    #endregion

    #region Load Items
    public void ItemGetData()
    {
        if (!stagesManager) stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

        int id = 0;
        foreach (ItemPattern itemPattern in GlobalItemsDataBase)
        {
            itemPattern.ID = id;
            id++;
        }
        foreach (Stage stage in stagesManager.StagesDataBase)
        {
            foreach (ItemPattern itemPattern in stage.ItemsDataBase)
            {
                itemPattern.ID = id;
                id++;
            }
        }

        List<ItemPattern> items = new(ItemsDataBase());
        foreach (ItemPattern item in items)
        {
            string keyName = $"Item_{item.ID}Count";
            if (PlayerPrefs.HasKey(keyName))
            {
                int count = PlayerPrefs.GetInt(keyName);

                AddItem(item, count);
            }
        }
    }
    #endregion
}

[Serializable]
public class ItemPattern
{
    public Sprite ico;

    public int ID;
    public string ItemName;
    public string Description;
    public ItemTypes Type;

    public float CurrencyPrice;
    public float XpPrice;

    public string UseMethodName;
}
public enum ItemTypes
{
    Toy, Cloth, Garbage, ItemPack
}
