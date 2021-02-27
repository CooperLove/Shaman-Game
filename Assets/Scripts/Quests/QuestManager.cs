using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public QuestUIManager questUIManager;
    public Transform currentQuests;
    public Transform availableQuests;
    public Transform finishedQuests;
    public GameObject questTemplate;
    public Tab_Quests tab_Quests;
    private static QuestManager instance;

    public static QuestManager Instance { get => instance; set => instance = value; }

    QuestManager (){
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    private void Awake() {
        Tab_Quests[] tabs = Resources.FindObjectsOfTypeAll<Tab_Quests>();
        if (tabs.Length > 0)
            tab_Quests = tabs[1];

        InitializeIngameQuestDescription();

    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    ///<summay>Adiciona uma missão no inventário e no painel de quests</summary>
    public Transform AddQuest (Quest quest){
        int length = availableQuests.childCount;
        for (int i = 0; i < length; i++)
        {   
            transform.GetChild(i).TryGetComponent<QuestHandler>(out QuestHandler handler);
            if (handler != null && handler.Quest.Equals(quest)){
                Debug.Log($"Missão {handler.Quest.QuestName} já existente");
                return transform.GetChild(i);
            }
        }
        AddQuestInProgress(quest);
        return null;
    }

    public void AddQuestInProgress (Quest quest){
        GameObject g = CreateQuestObject(currentQuests);
        AddHandler(quest, g);
        g.SetActive(true);
        tab_Quests.UpdateIndicators();
        QuestUIManager.Instance.AddQuestInUI(quest);
    }

    public void AddAvailableQuest (Quest quest){
        GameObject g = CreateQuestObject(availableQuests);
        AddHandler(quest, g);
        g.SetActive(true);
        tab_Quests.UpdateIndicators();
    }

    private GameObject CreateQuestObject (Transform parent) {
        GameObject g = Instantiate(questTemplate, transform.position, transform.rotation);
        g.transform.SetParent(parent);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        
        return g;
    }

    private void AddHandler (Quest quest, GameObject g){
        Type type = Type.GetType(quest.IsPlayerQuest ? "PlayerQuestHandler" : "CompanionQuestHandler");
        g.AddComponent(type);
        QuestHandler handler = g.GetComponent<QuestHandler>();
        handler.Quest = quest;
        handler.QuestName = g.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    /// <summary>Executa OnAccept() da quest e a move para a aba de quests em progresso.</summary>
    public void OnAcceptQuest (Transform questTransform){
        Debug.Log($"Quest Manager OnAccept");
        Quest q = questTransform.GetComponent<QuestHandler>().Quest;
        q.OnAccept();
        questTransform.SetParent(currentQuests);
        questUIManager.AddQuestInUI(q);
    }

    /// <summary>Executa OnComplete() da quest e a move para a aba de quests finalizadas.</summary>
    public void OnCompleteQuest (Transform questTransform){
        Debug.Log($"Quest Manager OnComplete");
        Quest q = questTransform.GetComponent<QuestHandler>().Quest;
        if (!q.IsCompleted)
            q.OnComplete();
        questTransform.SetParent(finishedQuests);
        questUIManager.RemoveQuestFromUI(q);
    }

    public bool OnCompleteQuest (Quest quest){
        float time = Time.deltaTime;
        int index = QuestUIManager.Instance.quests.IndexOf(quest);
        if (index != -1){
            OnCompleteQuest(currentQuests.GetChild(index));
            return true;
        }
        float elapsed = Time.deltaTime - time;
        Debug.Log($"Time: {elapsed}");
        return false;
    }

    // public bool OnCompleteQuest (Quest quest) {
    //     float time = Time.deltaTime;
    //     int size = currentQuests.childCount;
    //     for (int i = 0; i < size; i++)
    //     {
    //         Transform child = currentQuests.GetChild(i);
    //         child.TryGetComponent<QuestHandler>(out QuestHandler handler);
    //         if (handler != null && handler.Quest.QuestName.Equals(quest.QuestName)){
    //             OnCompleteQuest(child);
    //             return true;
    //         }
    //     }
    //     float elapsed = Time.deltaTime - time;
    //     Debug.Log($"Time: {elapsed}");
    //     return false;
    // }

    public Transform GetQuestTransform (Quest quest) {
        int size = currentQuests.childCount;
        for (int i = 0; i < size; i++)
        {
            Transform child = currentQuests.GetChild(i);
            child.TryGetComponent<QuestHandler>(out QuestHandler handler);
            if (handler != null && handler.Quest.Equals(quest)){
                return child;
            }
        }
        return null;
    }

    /// <summary>Executa OnDecline() da quest e a move para a aba de quests disponíveis.</summary>
    public void OnDeclineQuest (Transform questTransform){
        //Debug.Log($"Quest Manager OnDecline");
        Quest q = questTransform.GetComponent<QuestHandler>().Quest;
        q.OnDecline();
        questTransform.SetParent(availableQuests);
        questUIManager.RemoveQuestFromUI(q);
    }

    public void OnDeclineQuest (Quest q, Transform questTransform){
        //Debug.Log($"Quest Manager OnDecline with quest");
        q.OnDecline();
        questTransform.SetParent(availableQuests);
        questUIManager.RemoveQuestFromUI(q);
    }

    public void OnDeclineQuest (Quest quest){
        int index = QuestUIManager.Instance.quests.IndexOf(quest);
        //Debug.Log($"{quest.QuestName} at index {index}");
        if (index != -1)
            OnDeclineQuest(quest, currentQuests.GetChild(index));
    }

    /// <summary>Depois que uma quest é visualizada, é feita uma nova verificação para 
    /// saber se ainda existem quests não visualizadas.</summary>
    public void UpdateQuestTabState (int index){
        tab_Quests.ExclamationMark.SetActive(Inventory.Instance.CheckForNewQuests(index));
    }

    private void InitializeIngameQuestDescription(){
        ManualQuestMenu.Instance.Initialize();
        IngameQuestDescriptionPanel.Instance.Initialize();
    }
}
