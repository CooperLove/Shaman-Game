using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_AllItems : EquipmentTab
{
    public override void OpenAction()
    {
        //Debug.Log("Open All items");

        Inventory.Instance.InventorySize.ShowAllItems();
        OpenEquipmentTab();
    }
}
