using UnityEngine;
using UnityEngine.UI;

public class Tab_ActiveQuest : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark = null;

    public GameObject ExclamationMark { get => exclamationMark; set => exclamationMark = value; }
    public void OpenAction()
    {
        Inventory.Instance.EnableQuestFilters();
        Inventory.Instance.FilterCurrentQuests();
        ExclamationMark.SetActive(Inventory.Instance.CheckForCompletedQuests());
        Inventory.Instance.HighlightTab(GetComponent<Button>());
    }

}
