using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public abstract class QuestHandler : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    [SerializeField] private Quest quest = null;
    [SerializeField] protected Tab_Quests tabQuests;
    
    [SerializeField] private TMP_Text questName  = null;

    public Quest Quest { get => quest; set => quest = value; }
    public TMP_Text QuestName { get => questName; set => questName = value; }

    private void Start() {
        QuestName.text = Quest.QuestName;
        tabQuests = Resources.FindObjectsOfTypeAll<Tab_Quests>()[1];
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            //Debug.Log("Left click "+eventData.clickCount);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle){
            Debug.Log("Middle click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right){
            Debug.Log("Right click");
        }
    }


    public virtual void OnSelect(BaseEventData eventData)
    {
        
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        
    }

    public virtual void UpdateUI (){
        QuestName.text = Quest.QuestName;
    }

    public virtual void Accept () => quest?.OnAccept();
    public virtual void CheckIfComplete () => quest?.OnCheckIfComplete();
    public virtual void OnUpdate () => quest?.OnUpdate();
    public virtual void Complete () => quest?.OnComplete();
    public virtual void Decline () => quest?.OnDecline();
}
