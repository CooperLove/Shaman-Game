using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticQuestNPC : QuestNPC
{
    private void Awake() {
        foreach (QuestNPCPair pair in quests)
            pair.quest.OnUpdateQuest += HandleUpdate;
        
    }

    public override void HandleUpdate (){
        Debug.Log("Invoking NPC Event");
        UpdateIndicator();
    }

    public override void OnBeginInteraction()
    {
        Debug.Log($"Iniciando interação com {name}");
        if (!firstConversation.shown)
            firstConversation.StartDialogue();
        else if (CheckIfAllQuestsAreCompleted() || CheckIfSomeQuestIsComplete())
            questsFinishedConversation.StartDialogue();
        else if (!CheckIfAllQuestsCanBeCompleted())
            questsUnfinishedConversation.StartDialogue();
        else
            conversation.StartDialogue();
    }

    public override void OnEndConversation()
    {
        Debug.Log($"OnEndConversantion {name}");
        
        if (!firstConversation.shown){
            MoveQuestsThatCompleteInAnotherNPC();
            
            firstConversation.shown = true;
        }
        canInteract = true;
        UpdateIndicator();

        //Se não há nenhuma quest a ser completa retorna
        if (!CheckIfSomeQuestIsComplete())
            return;
        //Completar as missões que podem ser completas
        foreach (QuestNPCPair pair in quests)
            if (pair.quest.CanBeCompleted)
                QuestManager.Instance.OnCompleteQuest(pair.quest);

        UpdateIndicator();
    }

    protected override bool CheckIfAllQuestsCanBeCompleted()
    {
        foreach (QuestNPCPair pair in quests)
            if (!pair.quest.OnCheckIfComplete())
                return false;
        Debug.Log($"Todas as missões estão completas");
        return true;
    }

    public override void MoveQuestsThatCompleteInAnotherNPC(){
        List<Quest> questToBeRemoved = new List<Quest>();
        foreach (QuestNPCPair pair in quests){
            if (pair.quest.PreviousQuest == null && pair.quest.OnAccept())
                QuestManager.Instance.AddQuestInProgress(pair.quest);
            if (pair.quest.IsInProgress && pair.npc != null && !pair.quest.CompleteOnTheSameNPC){
                pair.npc.AddQuest(pair.quest);
                questToBeRemoved.Add(pair.quest);
            }
        }
        foreach (Quest q in questToBeRemoved)
            RemoveQuest(q);
    }

    public override void OnEndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
