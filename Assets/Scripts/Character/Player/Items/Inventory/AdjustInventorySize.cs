using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustInventorySize : MonoBehaviour
{
    public RectTransform inventoryRect;
    public int inventoryItemSize;
    public GameObject rarityFilterTransform, gearFilterTransform;
    [SerializeField] private int qtdItems;
    [SerializeField] private int qtdWeapons;
    [SerializeField] private int qtdGear;
    [SerializeField] private int qtdComsumables;
    [SerializeField] private int qtdJewerly;
    [SerializeField] private int qtdGems;
    [SerializeField] private int qtdCompanions;

    public int QtdItems { get => qtdItems; set => qtdItems = value; }

    private void Start() {
        CalculateAllQuantities();
        ResizeInventory();
    }

    public void ResizeInventory () => inventoryRect.sizeDelta = new Vector2(inventoryRect.sizeDelta.x, qtdItems * inventoryItemSize);

    public void OpenFilters () => rarityFilterTransform.SetActive(true);
    public void CloseFilters () => rarityFilterTransform.SetActive(false);
    public void OpenGearFilters () => gearFilterTransform.SetActive(true);
    public void CloseGearFilters () => gearFilterTransform.SetActive(false);
    
    public void ShowAllItems (){
        int total = Inventory.Instance.transform.childCount;
        if (total > 0){
            Inventory.Instance.transform.GetChild(0).GetComponent<Button>().Select();
        }
        for (int i = 0; i < total; i++){
            Transform child = Inventory.Instance.transform.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(true);
        }
        inventoryRect.sizeDelta = new Vector2(0, qtdItems * inventoryItemSize);
    }
    public void ShowItem (Type itemType){
        if (!itemType.IsSubclassOf(typeof(Item)) && !itemType.IsSubclassOf(typeof(Companion)))
            return;

        int total = Inventory.Instance.transform.childCount;
        int qtdItems = 0;
        for (int i = 0; i < total; i++){
            Transform child = Inventory.Instance.transform.GetChild(i);

            child.TryGetComponent<ItemHandler>(out ItemHandler handler);

            bool condition = handler != null && handler.Item != null;
            bool isCompanion = handler != null && handler is CompanionHandler && ((CompanionHandler)handler).Companion != null;
            
            if (condition && handler.Item.GetType().IsSubclassOf(itemType) && !child.gameObject.activeInHierarchy){
                child.gameObject.SetActive(true);
            }else if (condition && !handler.Item.GetType().IsSubclassOf(itemType)){
                child.gameObject.SetActive(false);
            }else if (isCompanion && ((CompanionHandler)handler).Companion.GetType().Equals(itemType) && !child.gameObject.activeInHierarchy){
                child.gameObject.SetActive(true);
            }else if (isCompanion && !((CompanionHandler)handler).Companion.GetType().Equals(itemType)){
                child.gameObject.SetActive(false);
            }
        }
        inventoryRect.sizeDelta = new Vector2(0, qtdItems * inventoryItemSize);
    }

    public void ShowWeapons () => ShowItem (typeof(Weapon));
    public void ShowGear () => ShowItem (typeof(Gear));
    public void ShowConsumables () => ShowItem (typeof(Consumable));
    public void ShowJewerly () => ShowItem (typeof(Jewerly));
    public void ShowGems () => ShowItem (typeof(Gem));
    public void ShowCompanions () => ShowItem (typeof(InitialCompanion));

    public void ShowEquipmentsAndGems (){
        int total = Inventory.Instance.transform.childCount;
        int count = 0;
        for (int i = 0; i < total; i++){
            Transform child = Inventory.Instance.transform.GetChild(i);
            bool hasTag = (child.tag == "Gem" || child.tag == "Weapon" || child.tag == "Gear" || child.tag == "Jewerly");
            //Debug.Log($"{child.name} {child.tag} {hasTag}");
            if (hasTag){
                child.gameObject.SetActive(true);
                count++;
            }else if (!hasTag && child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(false);
        }
        Debug.Log($"Achou {count} itens");
        inventoryRect.sizeDelta = new Vector2(0, count * inventoryItemSize);
    }

    public void FilterRarity (Item.Rarity rarity){
        Transform invTransform = Inventory.Instance.transform;
        int length = invTransform.childCount;
        int qtdItems = 0;
        for (int i = 0; i < length; i++)
        {
            Transform child = invTransform.GetChild(i);
            child.TryGetComponent<ItemHandler>(out ItemHandler handler);
            Button currentTab = Inventory.Instance.CurrentTab;
            if (handler != null
                && handler.Item != null
                && handler.Item.ItemRarity == rarity
                && currentTab != null
                && (currentTab.name.Equals("Tab "+child.tag) || currentTab.name.Equals("Tab All Items") || currentTab.name.Equals("Tab Enhance")))
            {
                child.gameObject.SetActive(true);
                qtdItems++;
            }else{
                child.gameObject.SetActive(false);
            }
        }
        inventoryRect.sizeDelta = new Vector2(0, qtdItems * inventoryItemSize);
    }

    public void FilterUncommon () => FilterRarity (Item.Rarity.Uncommon);
    public void FilterCommon () => FilterRarity (Item.Rarity.Common);
    public void FilterGreat () => FilterRarity (Item.Rarity.Great);
    public void FilterRare () => FilterRarity (Item.Rarity.Rare);
    public void FilterEpic () => FilterRarity (Item.Rarity.Epic);
    public void FilterLegendary () => FilterRarity (Item.Rarity.Legendary);
    public void FilterDivine () => FilterRarity (Item.Rarity.Divine);

    public void FilterGear (Type gearType){
        if (!gearType.IsSubclassOf(typeof(Gear)))
            return;

        int total = Inventory.Instance.transform.childCount;
        int qtdItem = 0;

        for (int i = 0; i < total; i++){
            Transform child = Inventory.Instance.transform.GetChild(i);
            child.gameObject.TryGetComponent<ItemHandler>(out ItemHandler handler);

            Button currentTab = Inventory.Instance.CurrentTab;

            bool condition = currentTab != null && handler != null  && handler.Item != null && currentTab.name.Equals("Tab Gear");

            if (condition && handler.Item.GetType().Equals(gearType)){
                child.gameObject.SetActive(true); qtdItem++;
            }else if (condition && !(handler.Item.GetType().Equals(gearType))){
                child.gameObject.SetActive(false);
            }
        }

        inventoryRect.sizeDelta = new Vector2(0, qtdItems * inventoryItemSize);
    }

    public void FilterHelmet () => FilterGear (typeof(Helmet));
    public void FilterChestArmor () => FilterGear (typeof(ChestArmor));
    public void FilterLeggings () => FilterGear (typeof(Legging));
    public void FilterBoots () => FilterGear (typeof(Boot));
    public void FilterGloves () => FilterGear (typeof(Gloves));


    public void CalculateAllQuantities (){
        int total = Inventory.Instance.transform.childCount;
        //Debug.Log($"Total of items {total}");
        int weapon = 0, gear = 0, consumable = 0, companions = 0, jewerly = 0, gems = 0;
        for (int i = 0; i < total; i++)
        {
            Transform child = Inventory.Instance.transform.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
                continue;
            if (child.tag == "Weapon")
                weapon++;
            else if (child.tag == "Gear")
                gear++;
            else if (child.tag == "Consumable")
                consumable++;
            else if (child.tag == "Companion")
                companions++;
            else if (child.tag == "Jewerly")
                jewerly++;
            else if (child.tag == "Gem")
                gems++;
        }
        
        qtdItems       = total;
        qtdWeapons     = weapon;
        qtdGear        = gear;
        qtdComsumables = consumable;
        qtdCompanions  = companions;
        qtdJewerly     = jewerly;
        qtdGems        = gems;
    }
    
}
