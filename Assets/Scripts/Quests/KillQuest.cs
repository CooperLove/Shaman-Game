using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu (menuName = "Quests/Kill Quest")]
public class KillQuest : Quest
{
    /*
        Referencia para o(s) inimigo(s)
        Quantidade de inimigos a serem mortos
    */
    public List<Wanted> enemies = new List<Wanted>();
    public override bool OnAccept()
    {
        if (IsCompleted || IsInProgress)
            return false;

        //Adicionar na lista de quests
        Debug.Log($"Starting quest {QuestName}");
        foreach (Wanted enemy in enemies)
            enemy.Initialize(this);  // Adiciona os eventos para atualziar a quantidade de inimigos
        
        IsInProgress = true;
        return true;
    }
    public override bool OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    public override bool OnUpdate(Info goal)
    {
        //UpdateProgress();
        // foreach (Wanted enemy in enemies)
        // {
        //     if (goal.ObjectName.Equals(enemy.goal.ObjectName)){
        //         //Debug.Log($"Killed 1 {enemy.goal.ObjectName}");
        //         enemy.Quantity++;
        //         QuestManager.Instance.questUIManager.UpdateGoalText(this);
        //         CanBeCompleted = OnCheckIfComplete();
        //         if (CanBeCompleted && !NeedsToBeDelivered)
        //             QuestManager.Instance.OnCompleteQuest(this);
        //         RaiseEvent();
        //         return true;
        //     }
        // }s
        return false;
    }
    public bool OnUpdate (EnemyInfo e){
        foreach (Wanted enemy in enemies)
        {
            if (e.name.Equals(enemy.goal.name)){
                enemy.Quantity++;
                CanBeCompleted = OnCheckIfComplete();
                Debug.Log($"Quest {QuestName} {CanBeCompleted} {NeedsToBeDelivered}");
                if (CanBeCompleted && !NeedsToBeDelivered)
                    QuestManager.Instance.OnCompleteQuest(this);
                return true;
            }
        }
        return false;
    }
    public override bool OnCheckIfComplete()
    {
        foreach (Wanted enemy in enemies)
            if (enemy.Quantity < enemy.quantityToGet) 
                return false;

        return true;
    }
    public override bool OnComplete()
    {
        if (IsCompleted || !IsInProgress || !CanBeCompleted)
            return false;

        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Experience += Experience;
        pi.Gold += GoldGiven;

        Reward?.GiveReward();

        IsInProgress = false;
        CanBeCompleted = false;
        IsCompleted = true;

        Debug.Log($"Quest {QuestName} Completed");


        if (NextQuest != null && !NextQuest.NeedsToBeAcceptedByPlayer){
            NextQuest.OnAccept();
            //QuestManager.Instance.questUIManager.AddQuestInUI(NextQuest);
            QuestManager.Instance.AddQuestInProgress(NextQuest);
        }
        
        InvokeOnCompleteEvent();
        return true;
    }

    public override bool OnDecline()
    {
        Quest q = this;
        while (q != null){
            q.IsInProgress = false;
            q.CanBeCompleted = false;
            q.IsCompleted = false;
            q = q.PreviousQuest;
        }
        return true;
    }

    public override string GoalText()
    {
        return enemies.Aggregate ("<size=28><color=#E69913>Goal:\n</color></size>", (unkown, e) => unkown + 
                            $"\t Kill {e.Quantity}/{e.quantityToGet} <color=#E69913>{e.goal.name}</color>\n");
    }

    public override string RewardsText()
    {
        var b = "<color=#E69913>";
        var end = "</color>";
        return $"<size=28>{b}Rewards{end}</size>:\n \tExperience: {b}{Experience}{end} \t Gold: {b}{GoldGiven}{end}"; 
    }

    
}
