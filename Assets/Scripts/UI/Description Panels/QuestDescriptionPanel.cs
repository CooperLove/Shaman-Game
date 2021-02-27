using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDescriptionPanel : MonoBehaviour
{
    private static QuestDescriptionPanel instance;
    public Quest quest;
    [SerializeField] private GameObject itemRewardTemplate = null;
    [SerializeField] private RectTransform contentRect = null;

    [Header("Texts")]
    [SerializeField] private TMP_Text questName = null;
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private TMP_Text objective = null;
    [SerializeField] private TMP_Text reward = null;
    // [SerializeField] private TMP_Text goldReward = null;
    [SerializeField] private Transform rewardItems = null;
    [SerializeField] private RectTransform status = null;

    [Header("Buttons")]
    [SerializeField] private Button accept = null;
    [SerializeField] private Button decline = null;
    [SerializeField] private Button complete = null;
    [SerializeField] private Scrollbar scrollbar = null;
    public Transform inProgressQuests;
    public Transform finishedQuests;
    public GameObject questTemplate;
    public Transform questTransform;
    public int index;

    [Header("Panel Preffered Heights")]
    public float descriptionPrefferedHeight;
    public float goalPrefferedHeight;

    public static QuestDescriptionPanel Instance { get => instance; }

    private void Awake() {
        
    }
    QuestDescriptionPanel (){
        if (instance == null)
            instance = this;
    }
    public void Open () {
        //Debug.Log($"{quest.PreviousQuest} {quest?.PreviousQuest?.IsCompleted} {(quest.PreviousQuest != null && !quest.PreviousQuest.IsCompleted)}");
        if (Player.Instance.playerInfo.Level < quest?.NecessaryLevel || quest.IsCompleted || quest.IsInProgress || 
            (quest.PreviousQuest != null && !quest.PreviousQuest.IsCompleted) )
            accept.interactable = false;
        else
            accept.interactable = true;
        
        if (quest.IsInProgress && !quest.IsMainQuest/*&& Player.Instance.Companion.InProgressMissions.Contains(quest.ID)*/){
            //Debug.Log("Has the quest "+quest.ID);
            //complete.interactable = true;
            decline.interactable = true;
        }
        else{
            //complete.interactable = false;
            decline.interactable = false;
        }
        //if (quest.OnCheckIfComplete())
            //complete.interactable = true;
        if (quest.IsCompleted || !quest.IsInProgress)
            complete.interactable = false;
        else
            complete.interactable = true;

        scrollbar.value = 1;
        gameObject.SetActive(true);
    }

    public void OpenPanel () => gameObject.SetActive(true);
    public void Close () => gameObject.SetActive(false);
    public void SetQuest (int index) {
        this.index = index;
        quest = CompanionDescriptionPanel.Instance.GetQuest(index);
    }
    public void SetQuest (Quest quest) {
        this.quest = quest;
    }

    public void OnAcceptQuest (){
        if (questTransform != null){
            QuestManager.Instance.OnAcceptQuest(questTransform);
            questTransform = null;
        }
    }
    public void OnCompleteQuest (){
        if (questTransform != null){
            QuestManager.Instance.OnCompleteQuest(questTransform);
            questTransform = null;
            QuestManager.Instance.tab_Quests.UpdateIndicators();
        }
    }
    public void OnDeclineQuest (){
        if (questTransform != null){
            QuestManager.Instance.OnDeclineQuest(questTransform);
            questTransform = null;
        }
    }
    public void AcceptQuest () {
        if (quest.IsPlayerQuest)
            return;
        if (quest.Accept()){
            QuestManager.Instance.questUIManager.AddQuestInUI(quest);
            QuestManager.Instance.AddQuestInProgress(quest);
            complete.interactable = true;
            Player.Instance.Companion.InProgressMissions.Add(quest.ID);
            CompanionQuestHandler handler = (CompanionQuestHandler) GetInProgressQuestHandler(quest);
            if (handler != null)
                handler.EvoTreeComponent = CompanionDescriptionPanel.Instance.EvoComponent;
            //if ()
            //AddQuest();
        }else {
            accept.interactable = false;
        }
    }

    private QuestHandler GetInProgressQuestHandler (Quest quest){
        int size = inProgressQuests.childCount;
        for (int i = 0; i < size; i++)
        {
            inProgressQuests.GetChild(i).TryGetComponent<QuestHandler>(out QuestHandler handler);
            if (handler != null && handler.Quest.Equals(quest)){
                return handler;
            }
        }
        return null;      
    }

    private QuestHandler GetFinishedQuestHandler (Quest quest){
        int size = finishedQuests.childCount;
        for (int i = 0; i < size; i++)
        {
            finishedQuests.GetChild(i).TryGetComponent<QuestHandler>(out QuestHandler handler);
            if (handler != null && handler.Quest.Equals(quest)){
                return handler;
            }
        }
        return null;      
    }
    public void CompleteQuest () {
        if (quest.IsPlayerQuest)
            return;

        CompanionQuestHandler handler = (CompanionQuestHandler) GetFinishedQuestHandler(quest);
        if (quest != null && quest.IsCompleted){
            if (handler.EvoTreeComponent != null){
                EvoTreeComponent evoComp = handler.EvoTreeComponent;
                //Inicia a animação para preencher o caminho e add qual a missão foi escolhida na lista de missões feitas
                int id = -1;

                if (evoComp.Quests.Contains(quest))
                    id = quest.ID;
                Debug.Log($"Found quest id {id}");
                // for (int i = 0; i < evoComp.Quests.Count; i++)
                // {
                //     if (evoComp.Quests[i].ID == quest.ID){
                //         id = i;
                //         break;
                //     }
                // }
                if (id != -1) {
                    // Exibe o caminho até a próxima evo
                    CompanionDescriptionPanel.Instance.FillEvoPath(id);
                    // Atualiza o caminho escolhido
                    evoComp.chosenPath = id;
                    // Adiciona a missão escolhida
                    Player.Instance.Companion.ChosenMissions.Add(id);

                    //Adiociona o indice da evolução escolhida e atualiza o número da evo atual
                    int compIndex = evoComp.Childs[id].transform.GetSiblingIndex();
                    Player.Instance.Companion.FollowedPath.Add(compIndex);
                    Player.Instance.Companion.CurrentEvoIndex = compIndex;
                    Player.Instance.CompanionObj.CurrentEvoIndex = compIndex;
                }

                //Desmarca todas as missões que estavam em progresso
                Debug.Log($"Resetando quests");
                foreach (Quest q in evoComp.Quests)
                {
                    Debug.Log("Checking quest "+q.ID);
                    if (Player.Instance.Companion.InProgressMissions.Contains(q.ID)){
                        q.IsInProgress = false;
                    }
                }
            }

            Debug.Log("Removendo quest da lista de quests finalizadas");
            // Remove a quest completa da lista de quests
            int size = finishedQuests.childCount;
            for (int i = 0; i < size; i++)
            {
                Quest q = finishedQuests.GetChild(i).GetComponent<QuestHandler>().Quest;
                if (q.Equals(quest)){
                    Debug.Log("q Removed mission "+q.ID+" "+finishedQuests.GetChild(i).gameObject.name);
                    //Destroy (availableQuests.GetChild(i).gameObject);
                    Player.Instance.Companion.InProgressMissions.Remove(q.ID);
                }
                if (Player.Instance.Companion.InProgressMissions.Contains(q.ID)){
                    Debug.Log("q Removed mission "+q.ID+" "+finishedQuests.GetChild(i).gameObject.name);
                    //Destroy (availableQuests.GetChild(i).gameObject);
                    Player.Instance.Companion.InProgressMissions.Remove(q.ID);
                }
                if (Player.Instance.Companion.InProgressMissions.Count == 0)
                    break;
            }
            //Player.Instance.Companion.InProgressMissions.Remove(quest.ID);
        }
        return;
    }

    public void DeclineQuest (){
        if (quest.IsPlayerQuest)
            return;
        if (quest.Decline()){
            Player.Instance.Companion.InProgressMissions.Remove(index);
        }
    }

    public void AddQuest (){
        return;
        // GameObject g = Instantiate(questTemplate, transform.position, transform.rotation) as GameObject;
        // g.transform.SetParent(finishedQuests);
        // g.transform.localPosition = Vector3.zero;
        // g.transform.localScale = Vector3.one;
        // g.GetComponent<QuestHandler>().Quest = quest;
        // g.GetComponent<QuestHandler>().UpdateUI();
        //g.GetComponent<QuestHandler>().EvoTreeComponent = CompanionDescriptionPanel.Instance.EvoComponent;
    }

    public void SetEvoComponent (){
        return;
        //Transform t = EvolutionTree.Instance.transform.GetChild(quest.ID).transform;
        //EvoTreeComponent evo = t.GetChild(t.childCount - 1).GetComponent<EvoTreeComponent>();
        //CompanionDescriptionPanel.Instance.EvoComponent = evo;
    }

    public void UpdateUI (){
        if (quest == null)
            return;

        var contentHeight = 225f;  // 75 do titulo - 150 dos botões

        questName.text = quest.QuestName;

        description.text = quest.Description;
        contentHeight += ResizePanelPortion(description, descriptionPrefferedHeight);

        objective.text = quest.GoalText();
        contentHeight += ResizePanelPortion(objective, goalPrefferedHeight);

        reward.text = quest.RewardsText();
        contentHeight += reward.preferredHeight;

        int t = 0;
        if (quest.Reward is ItemReward qReward) {
            t = qReward.items.Count;
            List<Item> items = qReward.items;
            int size = rewardItems.childCount;
            for (int i = size - 1; i >= 0 ; i--)
                Destroy(rewardItems.GetChild(i).gameObject);

            for (int i = 0; i < t; i++)
                AddItemRewardOnDescriptionPanel(items[i]);
        }

        var rewardsHeight = 45f * t;
        contentHeight += rewardsHeight;
        Debug.Log($"Content {contentHeight}");

        status.anchoredPosition = new Vector2 (0, -rewardsHeight);
        Debug.Log($"{contentHeight} => {t * 45} - {t} itens na lista {status.localPosition} {status.position}");
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }

    private float ResizePanelPortion (TMP_Text textPortion, float prefferedHeight){
        RectTransform rect = textPortion.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2 (rect.sizeDelta.x, textPortion.preferredHeight);

        return textPortion.preferredHeight;
    }

    private void AddItemRewardOnDescriptionPanel (Item item){
        GameObject g = Instantiate(itemRewardTemplate, transform.position, transform.rotation);
        g.transform.SetParent(rewardItems); 
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;

        TMP_Text itemTemplateText = g.transform.GetChild(0).GetComponent<TMP_Text>();
        itemTemplateText.text = item.ItemName;
        itemTemplateText.enabled = true;

        Image itemTemplateImage = g.transform.GetComponent<Image>();
        itemTemplateImage.sprite = item.Sprite;
        itemTemplateImage.enabled = true;

        g.gameObject.SetActive(true);
    }
}
