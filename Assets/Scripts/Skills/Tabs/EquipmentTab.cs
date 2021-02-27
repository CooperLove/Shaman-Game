using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EquipmentTab : Tab
{
    protected virtual void OpenEquipmentTab () {
        CompanionUI.Instance.OpenFilters();
        Inventory.Instance.InventorySize.OpenFilters();
        Inventory.Instance.HighlightTab(GetComponent<Button>());
        Inventory.Instance.Open();
    }

    public override void CloseAction()
    {
        //Debug.Log("Closing inventory");
        //CompanionUI.Instance.CloseFilters();
        DescriptionPanel.Instance.Close();
        EquipmentDescriptionPanel.Instance.Close();
        Inventory.Instance.InventorySize.CloseFilters();
        Inventory.Instance.InventorySize.CloseGearFilters();
        Inventory.Instance.Close();
    }
}
