using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class EquipmentHandler : ItemHandler, IPointerClickHandler
{
    [SerializeField] protected int enhancement;

    public int Enhancement { get => enhancement; set => enhancement = value; }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            //Debug.Log("Left click "+eventData.clickCount);
            if (eventData.clickCount >= 2)
                Equip();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right){
            Debug.Log("Right click");
            if (!InventoryTabManager.Instance.tab.Equals(InventoryTabManager.Instance.enhanceTab))
                Unequip();
        }
    }
    protected override void OnSelect(){
        //Debug.Log($"Tab {InventoryTabManager.Instance.tab.Equals(InventoryTabManager.Instance.enhanceTab)}");
        if (InventoryTabManager.Instance.tab.Equals(InventoryTabManager.Instance.enhanceTab))
            return;

        DescriptionPanel.Instance.Close();
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) item);
        EquipmentDescriptionPanel.Instance.SetEquipment(this);
        EquipmentDescriptionPanel.Instance.ChangeText();
        if (transform.childCount > 3)
            Destroy(transform.GetChild(3).gameObject);
        item.NewItemIndicator(Inventory.Instance.CheckForNewItems(item.MyType(), transform.GetSiblingIndex()));
        EquipmentDescriptionPanel.Instance.Open();

        CompanionUI.Instance.CloseCompanionWindows(); 
    }

    protected override void OnDeselect()
    {
        base.OnDeselect();
    }

    public void _UpdateOnEquipText (bool isSelected){ 
        itemName.text = isSelected ? $"+{this.Enhancement} "+this.Item.ItemName+" (Equipped)" : $"+{this.Enhancement} "+this.Item.ItemName;

        //quantityText.text = "1";
    }

    public void Enhance (){
        //((Equipment)item).OnEnhancement(Gem gem);
    }

    public void AddListerners () => GetComponent<Button>().onClick.AddListener(Equip);
    public void RemoveListerners () => GetComponent<Button>().onClick.RemoveListener(Equip);

    public virtual void Equip (){
        ((Equipment) Item).OnChangeEquipment(this);
        CurrentItemsHandler.OnEquipUpdateStats();
    }

    public virtual void Unequip (){
        ((Equipment) Item).OnRemoveEquipment(this);
    }
    
}
