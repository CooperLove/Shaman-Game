using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    //private GameObject mainMenu = null;
    private RectTransform _rect = null;
    private GameObject itemTemplate = null;
    private Item item = null;
    [SerializeField] private List<Item> items = new List<Item>();
    private List<ItemHandler> handlers = new List<ItemHandler>();
    private Scrollbar inventoryScrollbar = null;
    private static Inventory instance;
    public AdjustInventorySize inventorySize;
    private Hashtable hashtable;
    private static CompanionHandler companionHandler;
    private bool stack;
    private Button currentTab;
    private Transform currentQuestsTransform = null;
    private Transform availableQuestsTransform = null;
    private Transform finishedQuestsTransform = null;
    private GameObject filterCurrentQuests = null;
    private GameObject filterAvailableQuests = null;
    private GameObject filterFinishedQuests = null;
    private List<string> tagsToStack = new List<string>();
    
    [Header("Highlight colors")]
    private Color highlightedColor = Color.white;
    private Color unHighlightedColor = Color.black;


    public Button CurrentTab { get => currentTab; set => currentTab = value; }

    public static Inventory Instance { get => instance; set => instance = value; }
    public AdjustInventorySize InventorySize { get => inventorySize; set => inventorySize = value; }
    public static CompanionHandler CompanionHandler { get => companionHandler; set => companionHandler = value; }
    public List<ItemHandler> Handlers { get => handlers; set => handlers = value; }
    public List<Item> Items { get => items; set => items = value; }

    Inventory (){
        if (instance == null)
            instance = this;
        
        tagsToStack.Add("Consumable");
        tagsToStack.Add("Gem");
        hashtable = new Hashtable();
        
    }
    private void Awake() {
        //Debug.Log($"Awake");
        StackItems();
        
        //Debug.Log($"Inventory awake");
        
    }
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!stack) 
            return;
        //hashtable["Test"] = (int) hashtable["Test"] + 1;
        //Debug.Log(hashtable["Test"]);
        stack = false;
        StackItems();
    }

    public void Initialize (){
        Debug.Log("Inventory initialize");
        inventorySize = GetComponent<AdjustInventorySize>();
        _rect = GetComponent<RectTransform>();
        
        var parent = transform.parent;
        currentQuestsTransform = parent.GetChild(1).GetComponent<Transform>();
        availableQuestsTransform = parent.GetChild(2).GetComponent<Transform>();
        finishedQuestsTransform = parent.GetChild(3).GetComponent<Transform>();
        
        //Debug.Log($"Quests transform {currentQuestsTransform} {availableQuestsTransform} {finishedQuestsTransform}");
        //Transform filterQuest = GameObject.Find("Filters Quests").GetComponent<Transform>();
        var filterQuest = Resources.FindObjectsOfTypeAll<Tab_ActiveQuest>()[0].transform.parent.GetComponent<Transform>();
        filterCurrentQuests = filterQuest.GetChild(0).gameObject;
        filterAvailableQuests = filterQuest.GetChild(1).gameObject;
        filterFinishedQuests = filterQuest.GetChild(2).gameObject;

        
    }
    public void StackItems (){
       
    }

    public void Stack (){
        var length = transform.childCount;
        var pairs = new List<KeyValuePair<ItemHandler, int>>();
        for (var i = length - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            child.TryGetComponent<ItemHandler>(out var handler);

            if (!handler || !handler.Item || !(handler.Item is Collectable)) 
                continue;
            
            var pair = pairs.Find(x => x.Key.Item.Equals(handler.Item));
            //Debug.Log($"Item {transform.GetChild(i).name} Handler {handler} Pair {pair}");

            if (pair.Key != null && pair.Value != 0 && pair.Key.Item.Equals(handler.Item)){
                var index = pairs.FindIndex(x => x.Key.Item.Equals(handler.Item));
                pairs[index] = new KeyValuePair<ItemHandler, int>(pairs[index].Key, pairs[index].Value+(handler.Quantity == 0 ? 1 : handler.Quantity ));
                //Debug.Log($"New pair {items[index]}");
                Items.Remove(handler.Item);
                Handlers.Remove(handler);
                Destroy(child.gameObject);
            }else{
                pairs.Add(new KeyValuePair<ItemHandler, int>(handler, handler.Quantity == 0 ? 1 : handler.Quantity));
            }
        }

        foreach ( var pair in pairs)
        {
            Debug.Log($"Pair: {pair.Key} {pair.Value}");
            pair.Key.Quantity = pair.Value;
            pair.Key.QuantityText.text = $"{pair.Value}";
        }

        
    }

    public void HighlightTab (Button newTab){
        //Debug.Log(newTab.gameObject.name);
        if (!newTab)
            return;
        ColorBlock cb;
        
        if (CurrentTab){
            cb = CurrentTab.colors;
            cb.normalColor = unHighlightedColor;
            CurrentTab.colors = cb;
        }

        cb = newTab.colors;
        cb.normalColor = highlightedColor;
        newTab.colors = cb;
        
        CurrentTab = newTab;
    }

    public void OpenQuests (){
        EnableQuestMenu(true, false, false);
        EnableQuestFilters ();
        gameObject.SetActive(false);
    }

    public void FilterCurrentQuests (){
        EnableQuestMenu(true, false, false);
    }
    public void FilterAvailableQuests (){
        EnableQuestMenu(false, true, false);
    }
    public void FilterFinishedQuests (){
        EnableQuestMenu(false, false, true);
    }

    private void EnableQuestMenu (bool current, bool available, bool finished){
        if (!currentQuestsTransform){
            Debug.Log($"Tavam nulo");
            var parent = transform.parent;
            currentQuestsTransform = parent.GetChild(1).GetComponent<Transform>();
            availableQuestsTransform = parent.GetChild(2).GetComponent<Transform>();
            finishedQuestsTransform = parent.GetChild(3).GetComponent<Transform>();
        }


        currentQuestsTransform.gameObject.SetActive(current);
        availableQuestsTransform.gameObject.SetActive(available);
        finishedQuestsTransform.gameObject.SetActive(finished);
    }

    public void EnableQuestFilters (){
        filterCurrentQuests.SetActive(true);
        filterAvailableQuests.SetActive(true);
        filterFinishedQuests.SetActive(true);
    }

    public void DisableQuestFilters (){
        filterCurrentQuests.SetActive(false);
        filterAvailableQuests.SetActive(false);
        filterFinishedQuests.SetActive(false);
    }
    public void CloseQuests (){
        EnableQuestMenu(false, false, false);
        gameObject.SetActive(true);
    }

    public void CloseQuestsTab () {
        EnableQuestMenu(false, false, false);
    }

    public void Open (){
        EnableQuestMenu(false, false, false);
        if (inventoryScrollbar != null)
            inventoryScrollbar.value = 1;
        gameObject.SetActive(true);
    }

    public void Close () => gameObject.SetActive (false);

    public void AddItem (GameObject g){
        var transform1 = transform;
        var o = Instantiate(g, transform1.position, transform1.rotation);
        o.transform.SetParent(this.transform);
        o.transform.localScale = Vector3.one;
        var handler = o.GetComponent<ItemHandler>();
        handler.Item = handler.CreateNewItem();
        this.InventorySize.CalculateAllQuantities();
    }
    public void AddItem (GameObject g, bool newItem){
        var transform1 = transform;
        var o = Instantiate(g, transform1.position, transform1.rotation);
        o.transform.SetParent(this.transform);
        o.transform.localScale = Vector3.one;
        var handler = o.GetComponent<ItemHandler>();
        if (newItem)
            handler.Item = handler.CreateNewItem();
        this.InventorySize.CalculateAllQuantities();
    }

    public void CreateItem (Item item){
        if (!item)
            return;

        itemTemplate = Resources.Load("Prefabs/Templates/Item Template") as GameObject;

        var transform1 = transform;
        var g = Instantiate(itemTemplate, transform1.position, transform1.rotation);
        //Debug.Log($"Criando handler do tipo {handler.GetType()}");
        var h = (ItemHandler) g.AddComponent(item.MyHandler());
        if (item.MyType().IsSubclassOf(typeof(Equipment)) || item is Gem)
            g.AddComponent<EnhancementHandler>().Handler = h;

        h.Item = item;
        h.Rarity = item.GetRarity();
        //Debug.Log($"Criando {h.Rarity} {item.ItemName}");
        h.Sprite = g.transform.GetChild(0).GetComponent<Image>();
        h.ItemName = g.transform.GetChild(1).GetComponent<TMP_Text>();
        h.QuantityText = g.transform.GetChild(2).GetComponent<TMP_Text>();
        h.Quantity = 1;
        h.QuantityText.text = $"1";
        
        g.name = item.ItemName;
        g.tag = item.MyType().ToString();
        g.transform.SetParent(transform);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        item.NewItemIndicator(true);
        g.gameObject.SetActive(true);

        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y + 50);

        Items.Add(item);
        handlers.Add(h);
        //NewItemIndicator (item.MyHandler());
    }

    public ItemHandler CreateItem (Item item, bool loading){
        if (!item)
            return null;

        itemTemplate = Resources.Load("Prefabs/Templates/Item Template") as GameObject;

        var transform1 = transform;
        var g = Instantiate(itemTemplate, transform1.position, transform1.rotation);
        //Debug.Log($"Criando handler do tipo {handler.GetType()}");
        var h = (ItemHandler) g.AddComponent(item.MyHandler());
        if (item.MyType().IsSubclassOf(typeof(Equipment)) || item is Gem)
            g.AddComponent<EnhancementHandler>().Handler = h;

        h.Item = item;
        h.Rarity = item.GetRarity();
        //Debug.Log($"Criando {h.Rarity} {item.ItemName}");
        h.Sprite = g.transform.GetChild(0).GetComponent<Image>();
        h.ItemName = g.transform.GetChild(1).GetComponent<TMP_Text>();
        h.QuantityText = g.transform.GetChild(2).GetComponent<TMP_Text>();
        h.Quantity = 1;
        h.QuantityText.text = $"{h.Quantity}";

        g.name = item.ItemName;
        g.tag = item.MyType().ToString();
        g.transform.SetParent(transform);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        item.NewItemIndicator(true);
        g.gameObject.SetActive(true);

        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y + 50);

        if (loading) 
            return h;
        
        Items.Add(item);
        handlers.Add(h);

        return h;
        //NewItemIndicator (item.MyHandler());
    }

    public void CreateItem (Item item, Item.Rarity rarity){
        if (item == null)
            return;

        itemTemplate = Resources.Load("Prefabs/Templates/Item Template") as GameObject;

        var g = Instantiate(itemTemplate, transform.position, transform.rotation);
        //Debug.Log($"Criando handler do tipo {handler.GetType()}");
        var h = (ItemHandler) g.AddComponent(item.MyHandler());
        if (item.MyType().IsSubclassOf(typeof(Equipment)) || item is Gem)
            g.AddComponent<EnhancementHandler>().Handler = h;

        h.Item = item;
        h.Rarity = rarity;
        //Debug.Log($"Criando {h.Rarity} {item.ItemName}");
        h.Sprite = g.transform.GetChild(0).GetComponent<Image>();
        h.ItemName = g.transform.GetChild(1).GetComponent<TMP_Text>();
        h.QuantityText = g.transform.GetChild(2).GetComponent<TMP_Text>();
        
        g.name = item.ItemName;
        g.tag = item.MyType().ToString();
        g.transform.SetParent(transform);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        item.NewItemIndicator(true);
        g.gameObject.SetActive(true);

        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y + 50);

        Items.Add(item);
        Handlers.Add(h);
        //NewItemIndicator (item.MyHandler());
    }

    public bool CreateItem (Item item, ItemHandler handler, bool loading){
        itemTemplate = Resources.Load("Prefabs/Templates/Item Template") as GameObject;

        var transform1 = transform;
        var g = Instantiate(itemTemplate, transform1.position, transform1.rotation);
        //Debug.Log($"Criando handler do tipo {handler.GetType()}");
        ItemHandler h = (ItemHandler) g.AddComponent(item.MyHandler());
        // h.Item = item;
        // h.Rarity = item.ItemRarity;
        // h.Sprite = g.transform.GetChild(0).GetComponent<Image>();
        // h.ItemName = g.transform.GetChild(1).GetComponent<TMP_Text>();
        // h.QuantityText = g.transform.GetChild(2).GetComponent<TMP_Text>();
        
        g.transform.SetParent(transform);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        g.gameObject.SetActive(true);

        if (loading) 
            return true;
        
        Items.Add(item);
        Handlers.Add(handler);
        return true;
    }

    public void CreateCompanion (Companion companion){
        itemTemplate = Resources.Load("Prefabs/Templates/Item Template") as GameObject;

        var transform1 = transform;
        var g = Instantiate(itemTemplate, transform1.position, transform1.rotation);
        //Debug.Log($"Criando handler do tipo {handler.GetType()}");
        var h = (CompanionHandler) g.AddComponent(companion.MyHandler());
        h.Companion = companion;
        h.Sprite = g.transform.GetChild(0).GetComponent<Image>();
        h.ItemName = g.transform.GetChild(1).GetComponent<TMP_Text>();
        h.QuantityText = g.transform.GetChild(2).GetComponent<TMP_Text>();
        
        g.name = companion.CompName;
        g.tag = companion.MyType().ToString();
        g.transform.SetParent(transform);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        companion.NewItemIndicator(true);
        g.gameObject.SetActive(true);
    }


    public List<ConsumableHandler> GetConsumables (){
        var consumables = new List<ConsumableHandler>();
        //Debug.Log($"Total items: {transform.childCount}");
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).TryGetComponent<ConsumableHandler>(out var handler);
            if (handler){
                consumables.Add(handler);
            }
        }
        return consumables;
    }

    /// <summary>Verifica se ainda existem quests que ainda não foram visualizadas.</summary>
    public bool CheckForNewQuests (){
        var length = availableQuestsTransform.childCount;
        for (var i = 0; i < length; i++)
        {
            if (availableQuestsTransform.GetChild(i).childCount > 2){
                return true;
            }
        }
        return false;
    }

    /// <summary>Verifica se ainda existem quests diferentes da quest no indice index, que ainda não foram visualizadas.</summary>
    public bool CheckForNewQuests (int index){
        var length = availableQuestsTransform.childCount;
        for (var i = 0; i < length; i++)
        {
            if (i == index)
                continue;
            if (availableQuestsTransform.GetChild(i).childCount > 2){
                return true;
            }
        }
        return false;
    }

    public bool CheckForCompletedQuests (){
        var hasCompletedQuests = false;
        var length = currentQuestsTransform.childCount;
        for (var i = 0; i < length; i++)
        {
            currentQuestsTransform.GetChild(i).TryGetComponent<QuestHandler>(out var handler);
            if (!handler || handler.Quest.IsMainQuest || !handler.Quest.CanBeCompleted) 
                continue;
            
            handler.QuestName.color = Color.green;
            hasCompletedQuests = true;
        }
        return hasCompletedQuests;
    }


    /// <summary>Verifica se existem algum item do tipo itemType no inventário que ainda não foi visualizado.</summary>
    public bool CheckForNewItems (Type itemType, int index){
        //Debug.Log($"Index[{index}] Searching for items of type {itemType} {itemType.IsSubclassOf(typeof(Item))}");
        if (!itemType.IsSubclassOf(typeof(Item)) && !itemType.IsSubclassOf(typeof(Companion)) && !itemType.IsSubclassOf(typeof(Quest)))
            return false;

        var length = transform.childCount;
        for (var i = 0; i < length; i++)
        {
            if (i == index)
                continue;

            transform.GetChild(i).TryGetComponent<ItemHandler>(out var handler);
            if (handler != null && handler.Item != null && handler.transform.childCount > 3 && handler.Item.GetType().IsSubclassOf(itemType)){
                return true;
            }else if (handler != null && handler is CompanionHandler component && component.Companion != null 
                    && component.transform.childCount > 3 && component.Item.GetType().IsSubclassOf(itemType))
                return true;
        }
        return false;
    }

    public void TestGetRarity (){
        int uncommon = 0, common = 0, great = 0, rare = 0, epic = 0, legendary = 0, divine = 0;
        for (var i = 0; i < 100000; i++)
        {
            var r = item.GetRarity();
            switch (r)
            {
                case Item.Rarity.Uncommon:  uncommon++;  break;
                case Item.Rarity.Common:    common++;    break;
                case Item.Rarity.Great:     great++;     break;
                case Item.Rarity.Rare:      rare++;      break;
                case Item.Rarity.Epic:      epic++;      break;
                case Item.Rarity.Legendary: legendary++; break;
                case Item.Rarity.Divine:    divine++;    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Debug.Log($"Un {uncommon} - Co {common} - Great {great} - Rare {rare} - Epic {epic} - Leg {legendary} - Divine {divine}");
    }
}


