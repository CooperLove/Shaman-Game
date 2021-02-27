using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ConsumableHandler : CollectableHandler, IPointerUpHandler
{
    [SerializeField] private int currentSlot = -1;

    public int CurrentSlot { get => currentSlot; set => currentSlot = value; }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect ();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect ();
        RemoveListerners();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("The mouse click was released");
        
    }

    public void OnClick (){
        DescriptionPanel.Instance.SetItem((Consumable) item);
        DescriptionPanel.Instance.ChangeText();
        EquipmentDescriptionPanel.Instance.Close();
    }
    private void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        ItemName = transform.GetChild(1).GetComponent<TMP_Text>();
        QuantityText = transform.GetChild(2).GetComponent<TMP_Text>();
        base.ApllyGemsStats();
        base.ChangeText();
    }

    public void AddListerners () => GetComponent<Button>().onClick.AddListener(Use);
    public void RemoveListerners () => GetComponent<Button>().onClick.RemoveListener(Use);
    public void Use () {
        if (Quantity > 0){
            Consumable consumable = (Consumable) item;
            consumable.Use();
            Quantity--;
            QuantityText.text = ""+Quantity;
            if (Quantity == 0)
                Destroy(gameObject);
        }
    }

    public override Item CreateNewItem()
    {
        this.Rarity = (Item.Rarity) Random.Range(0, 7);
        string consumableLevel = ((Player.Instance.playerInfo.Level/10) * 10).ToString();
        string potionType = ((Item.PotionType) Random.Range(0,3)).ToString();
        string item = Random.Range(0, 3).ToString();
        string path = $"ScriptableObjects/Items/Collectable/Potions/{potionType}/{potionType} Potion";
        Debug.Log($"{this.rarity} Caminho: {path}");
        Collectable consumable = Resources.Load<Collectable>(path);
        Debug.Log(consumable);
        return consumable;
    }
}
