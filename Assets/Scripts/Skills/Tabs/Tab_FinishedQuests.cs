using UnityEngine;
using UnityEngine.UI;

public class Tab_FinishedQuests : MonoBehaviour
{
    public void OpenAction()
    {
        Inventory.Instance.EnableQuestFilters();
        Inventory.Instance.FilterFinishedQuests();
        Inventory.Instance.CheckForCompletedQuests();
        Inventory.Instance.HighlightTab(GetComponent<Button>());
    }
}
