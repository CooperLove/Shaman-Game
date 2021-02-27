using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ItemHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Item")]
    [SerializeField] protected Item item;
    [SerializeField] protected Item.Rarity rarity;

    [Header("UI Elements")]
    protected Image sprite;
    protected TMP_Text itemName;
    protected TMP_Text quantityText;
    [SerializeField] private int quantity;

    [Header("Gems and Enchantments of the current item on this script")]
    [SerializeField] private List<GemRarity> gems = new List<GemRarity>();
    

    public Item Item { get => item; set => item = value; }
    public TMP_Text ItemName { get => itemName; set => itemName = value; }
    public TMP_Text QuantityText { get => quantityText; set => quantityText = value; }
    public Image Sprite { get => sprite; set => sprite = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public List<GemRarity> Gems { get => gems; set => gems = value; }
    public Item.Rarity Rarity { get => rarity; set => rarity = value; }

    public abstract void OnSelect(BaseEventData eventData);
    public abstract void OnDeselect(BaseEventData eventData);

    protected abstract void OnSelect ();
    protected virtual void OnDeselect (){

    }

    public virtual ItemHandler FillHandler (Item item){
        return null;
    }
    private void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        ItemName = transform.GetChild(1).GetComponent<TMP_Text>();
        QuantityText = transform.GetChild(2).GetComponent<TMP_Text>();
        gems = new List<GemRarity>();
    }

    public virtual void ApllyGemsStats (){
        foreach (GemRarity gem in Gems)
        {
            gem.gem.EnhanceItem(gem, this);
        }
    }

    public void ChangeText (){
        //Debug.Log(itemName.color);
        //Rarity = item.ItemRarity;
        if (item == null)
            return;
        
        itemName.text = this is EquipmentHandler ? 
                        $"<color=#E69913>+{((EquipmentHandler) this).Enhancement}</color> {item.ItemName}" : 
                        $"{item.ItemName}";
        sprite.sprite = item.Sprite;
        itemName.color = ColorTable.ItemNameColor((int) Rarity);
        QuantityText.text = ""+Quantity;
    }

    public abstract Item CreateNewItem ();

}
