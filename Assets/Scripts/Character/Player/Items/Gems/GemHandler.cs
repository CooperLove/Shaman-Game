using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GemHandler : CollectableHandler
{

    public GemHandler (Gem gem, Item.Rarity rarity){
        this.item = gem;
        this.rarity = rarity;
    }  
    public GemHandler (GemRarity gem){
        this.item = gem.gem;
        this.rarity = gem.rarity;
    }  
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect();
    }

    public override Item CreateNewItem()
    {
        this.Rarity = (Item.Rarity) Random.Range(0, 7);
        var gemLevel = ((Player.Instance.playerInfo.Level/10) * 10).ToString();
        var type = Random.Range(0,5);
        var gemType = ((Item.GemType)type).ToString();
        var s = Random.Range(0,2);
        var isFlat = type > 1 ? (s == 0 ? "Flat" : "Percentage")+"/" : "";
        var item = Random.Range(0, 3).ToString();
        var path = $"ScriptableObjects/Items/Collectable/Gems/{gemType}/{isFlat}{this.rarity} {gemType} Gem";
        Debug.Log($"{this.rarity} Caminho: {path}");
        Gem gem = Resources.Load<Gem>(path);
        Debug.Log(gem);
        return gem;
    }

    public void OnClick (){
        DescriptionPanel.Instance.SetItem(this);
        DescriptionPanel.Instance.SetItem(item);
        DescriptionPanel.Instance.ChangeText();
        EquipmentDescriptionPanel.Instance.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Rarity = item.ItemRarity;
        Sprite = transform.GetChild(0).GetComponent<Image>();
        ItemName = transform.GetChild(1).GetComponent<TMP_Text>();
        QuantityText = transform.GetChild(2).GetComponent<TMP_Text>();
        ChangeText();
    }
}