using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class GearHandler : EquipmentHandler
{
    [SerializeField] protected float armor;
    [SerializeField] protected float mr;
    [SerializeField] protected int hp;

    public float Armor { get => armor; set => armor = value; }
    public float MR { get => mr; set => mr = value; }
    public int HP { get => hp; set => hp = value; }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect ();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect ();
        RemoveListerners();
    }

    public void OnClick (){
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment)item);
        EquipmentDescriptionPanel.Instance.SetEquipment(this);
        EquipmentDescriptionPanel.Instance.Open();
        DescriptionPanel.Instance.Close();
        EquipmentDescriptionPanel.Instance.ChangeText();
    }

    private void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        ItemName = transform.GetChild(1).GetComponent<TMP_Text>();
        QuantityText = transform.GetChild(2).GetComponent<TMP_Text>();
        base.ApllyGemsStats();
        base.ChangeText();
        if (item is Gear){
            armor = ((Gear) item).Armor;
            mr = ((Gear) item).MagicResistance;
        }
    }

    public override Item CreateNewItem()
    {
        this.Rarity = (Item.Rarity) Random.Range(0, 7);
        string gearLevel = ((Player.Instance.playerInfo.Level/10) * 10).ToString();
        string gearType = ((Item.GearType)Random.Range(0,5)).ToString();
        string item = Random.Range(0, 3).ToString();
        string path = $"ScriptableObjects/Items/Gear/{gearType}/Level {gearLevel}/{gearType} {item}";
        Debug.Log($"{this.rarity} Caminho: {path}");
        Gear gear = Resources.Load<Gear>(path);
        Debug.Log(gear);
        this.armor = gear.Armor;
        this.mr = gear.MagicResistance;
        return gear;
    }
}


