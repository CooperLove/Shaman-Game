using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Consumable : EquipmentTab
{
    public override void OpenAction()
    {
        Inventory.Instance.InventorySize.ShowConsumables();
        OpenEquipmentTab();
    }
}
