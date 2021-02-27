using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Gear : EquipmentTab
{
    public override void OpenAction()
    {
        Debug.Log("Open Gear Menu");

        Inventory.Instance.InventorySize.ShowGear();
        Inventory.Instance.InventorySize.OpenGearFilters();
        OpenEquipmentTab();
    }
}
