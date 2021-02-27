using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Weapon : EquipmentTab
{
    public override void OpenAction()
    {
        Inventory.Instance.InventorySize.ShowWeapons();
        OpenEquipmentTab();
    }
}
