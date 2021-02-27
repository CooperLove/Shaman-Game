using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Quests : Tab
{
    [SerializeField] Tab_ActiveQuest activeQuests;
    [SerializeField] Tab_AvailableQuest availabelQuests;
    private void Start() {
        
    }

    public void Initialize (){
        activeQuests = Resources.FindObjectsOfTypeAll<Tab_ActiveQuest>()[0];
        availabelQuests = Resources.FindObjectsOfTypeAll<Tab_AvailableQuest>()[0];
    }
    public override void OpenAction()
    {
        Inventory.Instance.OpenQuests();
        Inventory.Instance.EnableQuestFilters();
        UpdateIndicators();
        Inventory.Instance.HighlightTab(GetComponent<Button>());
    }
    public override void CloseAction()
    {
        QuestDescriptionPanel.Instance.Close();
        Inventory.Instance.DisableQuestFilters();
        Inventory.Instance.CloseQuestsTab();
    }

    public void UpdateIndicators (){
        bool isThereNewQuests = Inventory.Instance.CheckForNewQuests();
        bool isThereCompletedQuests = Inventory.Instance.CheckForCompletedQuests();
        ExclamationMark?.SetActive(isThereNewQuests || isThereCompletedQuests);
        activeQuests.ExclamationMark.SetActive(isThereCompletedQuests);
        availabelQuests.ExclamationMark.SetActive(isThereNewQuests);
        //Debug.Log($"{isThereNewQuests} {activeQuests.ExclamationMark.activeInHierarchy} {availabelQuests.ExclamationMark.activeInHierarchy}");
    }

    public void UpdateIndicators (int index){
        bool isThereNewQuests = Inventory.Instance.CheckForNewQuests(index);
        bool isThereCompletedQuests = Inventory.Instance.CheckForCompletedQuests();
        ExclamationMark?.SetActive(isThereNewQuests || isThereCompletedQuests);
        activeQuests.ExclamationMark.SetActive(isThereCompletedQuests);
        availabelQuests.ExclamationMark.SetActive(isThereNewQuests);
        //Debug.Log($"-- {isThereNewQuests} {activeQuests.ExclamationMark.activeInHierarchy} {availabelQuests.ExclamationMark.activeInHierarchy}");
    }
}
