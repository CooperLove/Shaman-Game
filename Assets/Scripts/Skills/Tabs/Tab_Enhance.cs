using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Enhance : Tab
{
    public override void OpenAction() {
        Inventory.Instance.Open();
        Inventory.Instance.InventorySize.OpenFilters();
        Inventory.Instance.InventorySize.ShowEquipmentsAndGems();
        Inventory.Instance.HighlightTab(GetComponent<Button>());
        Enhancement.Instance.Open();
    }
    public override void CloseAction() {
        Enhancement.Instance.Close();
        Inventory.Instance.InventorySize.CloseFilters();
        Inventory.Instance.Close();
    }
}
