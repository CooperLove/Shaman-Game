using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualQuestNPC : QuestNPC
{
    [TextArea(1,5)] public string description = null;
    private void Awake() {
        foreach (QuestNPCPair pair in quests){
            pair.quest.OnUpdateQuest += HandleUpdate;
            pair.quest.OnCompleteEvent += AddNextQuest;
        }
    }

    /// <summary>Atualiza o indicador do NPC, durante o progresso da missão;
    ///</summary>
    public override void HandleUpdate (){
        Debug.Log("Invoking Manual NPC Event");
        UpdateIndicator();
    }


    /// <summary>Adiciona a missão sequêncial a missão <paramref name="quest"/>, no NPC.
    /// <para>Obs: Só será adicionado caso a próxima missão exista.</para>
    ///</summary>
    /// <param name="quest">Quest que foi completa</param>
    public void AddNextQuest (Quest quest){
        if (quest.NextQuest != null){
            AddQuest(quest.NextQuest);
            quest.NextQuest.OnCompleteEvent += AddNextQuest;
        }
        
        quest.OnCompleteEvent -= AddNextQuest;
    }

    
    public override void OnBeginInteraction()
    {
        if (!firstConversation.shown)
            firstConversation.StartDialogue();
        else
            conversation.StartDialogue();
    }

    public override void OnEndConversation()
    {
        if (!firstConversation.shown)
            firstConversation.shown = true;
            
        if (ManualQuestMenu.Instance.ClearContent(this)){
            foreach (QuestNPCPair pair in quests)
                ManualQuestMenu.Instance.AddQuest(pair.quest);
        }
        
        MoveQuestsThatCompleteInAnotherNPC();

        UpdateIndicator();
        ManualQuestMenu.Instance.Open(this);
    }

    protected override bool CheckIfAllQuestsCanBeCompleted()
    {
        throw new System.NotImplementedException();
    }

    public override void MoveQuestsThatCompleteInAnotherNPC(){
        List<Quest> questToBeRemoved = new List<Quest>();
        foreach (QuestNPCPair pair in quests){
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
