using UnityEngine;
using UnityEngine.UI;

public class Tab_AvailableQuest : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark = null;

    public GameObject ExclamationMark { get => exclamationMark; set => exclamationMark = value; }
    public void OpenAction()
    {
        Inventory.Instance.EnableQuestFilters();
        Inventory.Instance.FilterAvailableQuests();
        ExclamationMark.SetActive(Inventory.Instance.CheckForNewQuests());
        Inventory.Instance.HighlightTab(GetComponent<Button>());
    }
}
