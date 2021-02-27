using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class QuestNPC : NPC
{
    public Image indicator = null;
    public Conversation firstConversation  = null;
    public Conversation questsUnfinishedConversation  = null;
    public Conversation questsFinishedConversation  = null;

    // [SerializeField] protected List<Quest> quests = new List<Quest>();
    [SerializeField] protected List<QuestNPCPair> quests = new List<QuestNPCPair>();

    public abstract void HandleUpdate();
    protected abstract bool CheckIfAllQuestsCanBeCompleted ();
    public abstract void MoveQuestsThatCompleteInAnotherNPC ();

    private void OnBecameVisible() {
        UpdateIndicator();
    }

    protected void UpdateIndicator () {
        bool completed = CheckIfAllQuestsAreCompleted();
        if (completed){
            indicator.sprite = Resources.Load<Sprite>("Images/Indicators/Exclamation Mark Icon");
            indicator.color = Color.gray;
            return;
        }

        bool inProgress = CheckIfThereisInProgressQuests();
        bool canBeCompleted = CheckIfSomeQuestIsComplete();
        //Debug.Log($"Status {inProgress} {canBeCompleted}");
        indicator.sprite = inProgress ? Resources.Load<Sprite>("Images/Indicators/Question Mark Icon") :
                                        Resources.Load<Sprite>("Images/Indicators/Exclamation Mark Icon");

        indicator.color = inProgress && !canBeCompleted ? Color.gray : Color.yellow;
    }

    protected bool CheckIfAllQuestsAreCompleted (){
        foreach (QuestNPCPair pair in quests){
            if (pair.quest.IsInProgress && !pair.quest.CompleteOnTheSameNPC)
                continue;
            if (!pair.quest.IsCompleted)
                return false;
        }
        return true;
    }
    
    protected bool CheckIfSomeQuestIsComplete (){
        foreach (QuestNPCPair pair in quests){
            //Debug.Log($"Quest {pair.quest.QuestName} => {pair.quest.CanBeCompleted} {pair.quest.CompleteOnTheSameNPC}");
            if (pair.quest.CanBeCompleted && pair.quest.CompleteOnTheSameNPC)
                return true;
        }
        return false;
    }

    protected bool CheckIfThereisInProgressQuests (){
        foreach (QuestNPCPair pair in quests)
            if (pair.quest.CompleteOnTheSameNPC && pair.quest.IsInProgress)
                return true;
        return false;
    }

    /// <summary>Usada para adicionar uma quest que será completa em outro NPC</summary>
    public void AddQuest(Quest q) {
        q.CompleteOnTheSameNPC = true;
        quests.Add(new QuestNPCPair(q, null));
        q.OnUpdateQuest += HandleUpdate;
        UpdateIndicator();
    }

    public void RemoveQuest (Quest q){
        Debug.Log($"Removendo quest {q.QuestName} de {name}");
        q.OnUpdateQuest -= HandleUpdate;
        quests.Remove(quests.Where(x => x.quest.QuestName.Equals(q.QuestName)).ToList()[0]);
    }



}

[System.Serializable]
public class QuestNPCPair {
    
    public Quest quest = null;
    public QuestNPC npc = null;

    public QuestNPCPair(Quest quest, QuestNPC npc)
    {
        this.quest = quest;
        this.npc = npc;
    }
}
