using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CollectableHandler : ItemHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            Debug.Log("Left click "+eventData.clickCount);
            
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right){
            //Debug.Log("Right click");
            if (Item != null && Item is Consumable){
                ((ConsumableHandler) this).Use();
                Debug.Log("Click on "+gameObject.name+" Using "+item.ItemName);
            }
        }
    }
    protected override void OnSelect()
    {
        if (InventoryTabManager.Instance.tab.Equals(InventoryTabManager.Instance.enhanceTab))
            return;

        //Debug.Log(item.Name);
        EquipmentDescriptionPanel.Instance.Close();
        var descPanel = DescriptionPanel.Instance; 
        descPanel.SetItem(item);
        descPanel.SetItem(this);
        descPanel.ChangeText();
        
        if (transform.childCount > 3)
            Destroy(transform.GetChild(3).gameObject);
        item.NewItemIndicator(Inventory.Instance.CheckForNewItems(item.MyType(), transform.GetSiblingIndex()));
        
        descPanel.Open();

        CompanionUI.Instance.CloseCompanionWindows();
        
    }

    protected override void OnDeselect()
    {
        base.OnDeselect();
    }
}
