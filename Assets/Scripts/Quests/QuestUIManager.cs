using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questTemplateUI;
    public Transform questsUI;
    public Transform currentQuests;
    public List<Transform> questsTransforms = new List<Transform>();
    public List<Quest> quests = new List<Quest>();
    
    private static QuestUIManager instance;

    public static QuestUIManager Instance { get => instance; set => instance = value; }

    QuestUIManager (){
        if (Instance == null)
            Instance = this;
    }

    public void AddQuestInUI (Quest quest) {
        GameObject g = CreateQuestUI(quest);

        //Adiciona a quest na lista de quests atuais
        quests.Add(quest);
        questsTransforms.Add(g.transform);

        SetupQuestNameAndQuestGoal(g, quest);

        ResizeUIPanel(g.GetComponent<RectTransform>(), true, quest);

        g.gameObject.SetActive(true);
    }

    private GameObject CreateQuestUI (Quest quest){
        GameObject g = Instantiate(questTemplateUI, transform.position, transform.rotation);
        g.transform.SetParent(questsUI);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        g.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        g.name = quest.QuestName;
        return g;
    }

    private void SetupQuestNameAndQuestGoal (GameObject g, Quest quest){
        g.GetComponent<TMP_Text>().text = $"{quests.IndexOf(quest)+1} "+quest.QuestName;
        TMP_Text goalText = g.transform.GetChild(0).GetComponent<TMP_Text>();
        goalText.text = quest.GoalText();
        goalText.rectTransform.sizeDelta = new Vector2(goalText.rectTransform.sizeDelta.x, goalText.preferredHeight);
    }

    public void RemoveQuestFromUI (Quest quest){
        GameObject questTransform = questsTransforms?[quests.IndexOf(quest)].gameObject;
        Debug.Log($"Removendo {questTransform.name} => Quest[{quests.IndexOf(quest)}] {quest.QuestName}");
        ResizeUIPanel(questTransform.GetComponent<RectTransform>(), false, quest);
        quests.Remove(quest);
        questsTransforms.Remove(questTransform.transform);
        Destroy(questTransform);
    }

    private void ResizeUIPanel (RectTransform rect, bool addQuest, Quest quest) {
        int index = quests.IndexOf(quest);

        //Altura do objeto (nome + objetivo) dessa quest
        float height = rect.GetChild(0).GetComponent<TMP_Text>().preferredHeight + 25;
        //Atualiza a altura das demais quests caso esteja removendo uma
        if (!addQuest){
            UpdateEntirePanel(index, height);
        }
        UpdateCurrentQuestHeight(rect, index);
        //Debug.Log($"After Rect {rect.name} {rect.anchoredPosition} {rect.localPosition} {rect.position}");
        ResizePanel(addQuest, height);
    }

    private void UpdateCurrentQuestHeight (RectTransform rect, int index){
        if (index == 0){
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);

            return;
        }
        //Referencia para saber em que altura a quest acima dessa está, para medir a distancia que a quest atual irá ficar
        RectTransform previous = questsUI.GetChild(index -1).GetComponent<RectTransform>();
        //Altura do texto do objetivo da quest acima dessa
        float height = previous.GetChild(0).GetComponent<TMP_Text>().preferredHeight;
        //Debug.Log($"[{index-1}] {previous.name} Altura {previous.localPosition.y} {height} - 25 = {previous.localPosition.y - height - 25}");
        //Atualiza a altura do rect da quest atual Obs: 25 é a altura do nome da quest
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, previous.localPosition.y - height - 25);
    }

    private void UpdateEntirePanel (int index, float height) {
        int size = questsUI.childCount;
        //Debug.Log($"Entire panel {questsUI.childCount} {size} {quests.Count}");
        foreach (Quest q in quests)
        {
            //Debug.Log($"Quests[{quests.IndexOf(q)}] {q.QuestName}");
        }
        for (int i = index + 1; i < size; i++)
        {
            if (i >= quests.Count)
                break;
            RectTransform child = questsUI.GetChild(i).GetComponent<RectTransform>();
            int childIndex = child.GetSiblingIndex();
            //Debug.Log($"Indexes {i} {childIndex}");
            child.anchoredPosition = new Vector2(child.anchoredPosition.x, child.anchoredPosition.y + height);
            //Atualiza o numero da quest
            child.GetComponent<TMP_Text>().text = $"{i} "+quests[i].QuestName;
        }
    }

    private void ResizePanel (bool addQuest, float height) {
        //Debug.Log($"Resize panel {addQuest}");
        //Referencia do tamanho do painel das quests
        Vector2 sizeDelta = questsUI.GetComponent<RectTransform>().sizeDelta;
        //Aumenta ou diminui a altura do painel, depende se está adicionando ou removendo uma quest
        sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + (addQuest ? height : -height));
        questsUI.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    public void UpdateGoalText (Quest quest) {
        int index = quests.IndexOf(quest);
        if (index < 0)
            return;
        Transform t = questsTransforms.Find(x => x.name.Equals(quest.QuestName));
        RectTransform questTransform = t.GetComponent<RectTransform>();
        //Debug.Log($"Goal Text {index} {quest.QuestName} transform {t}");
        questTransform.GetChild(0).GetComponent<TMP_Text>().text = quest.GoalText();
    }
}
