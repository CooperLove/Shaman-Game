using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JewerlyHandler : EquipmentHandler
{
    [SerializeField] protected float armor;
    [SerializeField] protected float mr;

    public float Armor { get => armor; set => armor = value; }
    public float MR { get => mr; set => mr = value; }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect();
    }

    public void OnClick (){
        EquipmentDescriptionPanel.Instance.SetEquipment((Jewerly) item);
        DescriptionPanel.Instance.SetItem(this);
        EquipmentDescriptionPanel.Instance.Open();
        DescriptionPanel.Instance.Close();
        DescriptionPanel.Instance.ChangeText();
    }

    // Start is called before the first frame update
    void Start()
    {
        base.ApllyGemsStats();
        
        base.ChangeText();
    }

    public override Item CreateNewItem()
    {
        this.Rarity = (Item.Rarity) Random.Range(0, 7);
        string jewelLevel = ((Player.Instance.playerInfo.Level/10) * 10).ToString();
        string jewelType = ((Item.JewerlyType)Random.Range(0,3)).ToString();
        string item = Random.Range(0, 3).ToString();
        string path = $"ScriptableObjects/Items/Jewerly/{jewelType}/Level {jewelLevel}/{jewelType} {item}";
        Debug.Log($"{this.rarity} Caminho: {path}");
        Jewerly jewel = Resources.Load<Jewerly>(path);
        Debug.Log(jewel);
        this.armor = jewel.Armor;
        this.mr = jewel.MagicResistance;
        return jewel;
    }
}
