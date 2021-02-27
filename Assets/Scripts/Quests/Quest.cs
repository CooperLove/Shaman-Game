using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public abstract class Quest : ScriptableObject {
    [SerializeField] public event Action OnUpdateQuest;
    [SerializeField] public event Action<Quest> OnCompleteEvent;
    [SerializeField] private int id;
    [SerializeField] private string questName;
    [SerializeField, TextArea(3, 15)] private string description;
    
    [Header("Indicadores para diferenciar entre quests.")]
    
    [SerializeField, Tooltip("Indica se é uma quest para o player ou para o companion.")] 
    private bool isPlayerQuest;
    
    [SerializeField, Tooltip("Indica se é uma quest principal ou secundária.")] 
    private bool isMainQuest;

    [SerializeField, Tooltip("Indica se o player precisará aceitar a quest ou ela irá iniciar automaticamente.")] 
    private bool needsToBeAcceptedByPlayer;

    [SerializeField, Tooltip("Indica se o player precisará completar a quest manualmente.")] 
    private bool needsToBeDelivered;

    [SerializeField, Tooltip("Indica se a quest será completa no mesmo NPC em que foi aceita.")] 
    private bool completeOnTheSameNPC;

    [Header("Nível necessário e recompensas dadas pela quest.")]
    [SerializeField] private int necessaryLevel;
    [SerializeField] private int goldGiven;
    [SerializeField] private int experience;
    [SerializeField] private Reward reward;
    
    [Header("Estado atual da quest.")]
    [SerializeField] private bool isInProgress;
    [SerializeField] private bool canBeCompleted;
    [SerializeField] private bool isCompleted;
    
    [Header("Quests que são interligadas")]
    [SerializeField] private Quest previousQuest;
    [SerializeField] private Quest nextQuest;

    public string QuestName    { get => questName; set => questName = value; }
    public string Description  { get => description; set => description = value; }
    public bool IsInProgress   { get => isInProgress; set => isInProgress = value; }
    public bool IsCompleted    { get => isCompleted; set => isCompleted = value; }
    public int NecessaryLevel  { get => necessaryLevel; set => necessaryLevel = value; }
    public int ID              { get => id; set => id = value; }
    public int GoldGiven       { get => goldGiven; set => goldGiven = value; }
    public int Experience      { get => experience; set => experience = value; }
    public Reward Reward       { get => reward; set => reward = value; }
    public Quest NextQuest     { get => nextQuest; set => nextQuest = value; }
    public Quest PreviousQuest { get => previousQuest; set => previousQuest = value; }
    public bool IsPlayerQuest { get => isPlayerQuest; set => isPlayerQuest = value; }
    public bool IsMainQuest { get => isMainQuest; set => isMainQuest = value; }
    public bool NeedsToBeAcceptedByPlayer { get => needsToBeAcceptedByPlayer; set => needsToBeAcceptedByPlayer = value; }
    public bool CanBeCompleted { get => canBeCompleted; set => canBeCompleted = value; }
    public bool NeedsToBeDelivered { get => needsToBeDelivered; set => needsToBeDelivered = value; }
    public bool CompleteOnTheSameNPC { get => completeOnTheSameNPC; set => completeOnTheSameNPC = value; }

    public abstract bool OnAccept ();
    public abstract bool OnUpdate ();
    public abstract bool OnUpdate (Info goal);
    public abstract bool OnCheckIfComplete ();
    public abstract bool OnComplete ();
    public abstract bool OnDecline ();
    public abstract string GoalText ();
    public abstract string RewardsText ();

    public virtual bool Accept () {
        if (isInProgress)
            return false;

        isInProgress = true;
        return true;
    }
    public virtual bool Complete () {
        // Check the requirements
        isCompleted = true;
        isInProgress = false;
        return true;
    }

    /// <summary></summary>
    public virtual bool Decline (){
        if (isCompleted || !isInProgress)
            return false;
        
        isInProgress = false;
        return true;
    }

    public virtual void UpdateProgress () {
        Debug.Log($"Invoking {questName} Event");
        QuestManager.Instance.questUIManager.UpdateGoalText(this);
        CanBeCompleted = OnCheckIfComplete();
        OnUpdateQuest?.Invoke();
        if (CanBeCompleted && !NeedsToBeDelivered)
            QuestManager.Instance.OnCompleteQuest(this);
    }

    public virtual void InvokeOnCompleteEvent (){
        OnCompleteEvent?.Invoke(this);
    }
}
