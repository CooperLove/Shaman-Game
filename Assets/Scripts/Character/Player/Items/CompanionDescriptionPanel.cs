using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CompanionDescriptionPanel : MonoBehaviour
{
    
    private static CompanionDescriptionPanel instance;
    [SerializeField] private TMP_Text compName = null;
    [SerializeField] private Image image  = null;
    
    [SerializeField] private TMP_Text desc  = null;
    [SerializeField] private TMP_Text intelligenceValue = null;
    [SerializeField] private TMP_Text intellectValue = null;
    [SerializeField] private TMP_Text wisdomValue = null;
    [SerializeField] private TMP_Text forValue = null;
    [SerializeField] private TMP_Text intValue = null;
    [SerializeField] private TMP_Text dexValue = null;
    [SerializeField] private TMP_Text conValue = null;
    [SerializeField] private TMP_Text vigorValue = null;
    [SerializeField] private Button[] buttons  = null;
    [SerializeField] private EvoTreeComponent evoComponent  = null;
    [SerializeField] private Scrollbar scrollbar  = null;

    public static CompanionDescriptionPanel Instance { get => instance; }
    public EvoTreeComponent EvoComponent { get => evoComponent; set => evoComponent = value; }

    public void Open () {
        gameObject.SetActive(true); 
        scrollbar.value = 1;
    }
    public void Close () => gameObject.SetActive(false);
    private void OnEnable() {
        
    }

    
    public void SetEvoComponent (EvoTreeComponent component) => EvoComponent = component;
    public void FillEvoPath (int index) => EvoComponent?.FillPath(index);
    public Quest GetQuest (int index){
        return EvoComponent?.Quests.Count > 0 ? EvoComponent?.Quests[index] : null;
    } 
    public void ShowQuest (int index) {
        if (index >= EvoComponent.Quests.Count)
            return;

        
    }
    public void ActivateButtons (){
        if (EvoComponent == null)
            return;

        ResetButtons ();

        if (!CheckIfCanDoQuests ())
            return;

        int index = evoComponent.transform.parent.GetSiblingIndex();

        ResetQuests (index);

        //Debug.Log(Player.Instance.Companion.FollowedPath.Contains(evoComponent.transform.parent.GetSiblingIndex())+" "+Player.Instance.Companion.CurrentEvoIndex);
        if ((evoComponent.transform.parent.GetSiblingIndex() != 0 &&
            !Player.Instance.Companion.FollowedPath.Contains(evoComponent.transform.parent.GetSiblingIndex())) || 
            Player.Instance.Companion.CurrentEvoIndex == 0)
            return;

        QuestsNotChosenCantBeSelected (index);
    }

    /// <summary> Turns X buttons active and interactable. 
    ///X is the number of quest a evolution has. </summary> 
    private void ResetButtons(){
        for (int i =  0; i < EvoComponent.numWays; i++){
            buttons[i].gameObject.SetActive(true);
            buttons[i].interactable = true;
        }
        for (int i = EvoComponent.numWays ; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);
    }

    /// <summary> Makes the buttons not interactable if the companion didn't go through some path or already did a quest in a previous companion. </summary> 
    private bool CheckIfCanDoQuests (){
        if (EvoComponent.Quests.Count > 0 && 
            Player.Instance.playerInfo.Level < EvoComponent.Quests[0].NecessaryLevel || 
            evoComponent.transform.parent.GetSiblingIndex() > Player.Instance.Companion.CurrentEvoIndex ||
            (!Player.Instance.Companion.FollowedPath.Contains(evoComponent.transform.parent.GetSiblingIndex()) && evoComponent.transform.parent.GetSiblingIndex() != 0) )
        {
            for (int i = 0 ; i < buttons.Length; i++)
                buttons[i].interactable = false;

            return false;
        }
        return true;
    }

    /// <summary> As the quests are Scriptable Objects, they are shared. This method resets the completed quests in a new companion </summary> 
    /// <param name="index">Index of the evolution.</param> 
    private void ResetQuests (int index){
        if (Player.Instance.Companion.FollowedPath.Contains(index)){
            int a = Player.Instance.Companion.FollowedPath.IndexOf(index);
            if (a+1 >= Player.Instance.Companion.ChosenMissions.Count){
                foreach (Quest quest in evoComponent.Quests)
                    if (!quest.IsInProgress)
                        quest.IsCompleted = false;
            }else{
                foreach (Quest quest in evoComponent.Quests)
                    quest.IsCompleted = true;
            }
        }
        if (index == 0){
            Debug.Log("Index == 0");
            if (Player.Instance.Companion.ChosenMissions.Count == 0){
                Debug.Log("0 Missions");
                foreach (Quest quest in evoComponent.Quests)
                    if (!quest.IsInProgress)
                        quest.IsCompleted = false;
            }else{
                foreach (Quest quest in evoComponent.Quests)
                    quest.IsCompleted = true;
            }
        }
    }

    /// <summary> Makes the buttons with index different of the index passed as parameter not interactable </summary>
    /// <param name="index">Index of the button to stay active.</param> 
    public void QuestsNotChosenCantBeSelected (int index){
        if (Player.Instance.Companion.FollowedPath.Contains(index)){          // Checa se o pet possui a evolução
            int a = Player.Instance.Companion.FollowedPath.IndexOf(index);    // Indice da evolução
            if (a+1 >= Player.Instance.Companion.ChosenMissions.Count)        // Se ainda não escolheu alguma quest, retorne.
                return;
            
            int k = Player.Instance.Companion.ChosenMissions[a+1];            // Indice da missão escolhida
            //Debug.Log("==> "+k+" "+a);
            for (int i = 0; i < EvoComponent.numWays; i++)                    // Percorre todos os botões e marca os que não tiveram a quest escolhida com não interagivel
                if (i != k)
                    buttons[i].interactable = false;
        }
        if (index == 0){
            
            int k = Player.Instance.Companion.ChosenMissions[0];
            for (int i = 0; i < EvoComponent.numWays; i++)
                if (i != k)
                    buttons[i].interactable = false;
        }
    }

    CompanionDescriptionPanel (){
        if (instance == null)
            instance = this;
    }

    public void UpdateUI (TranscendenceInfo transcendence){
        compName.text = transcendence.CompName;
        image.sprite = transcendence.CompImage;
        desc.text = transcendence.Description;
        intelligenceValue.text = "+"+transcendence.Intelligence;
        intellectValue.text = "+"+transcendence.Intellect;
        wisdomValue.text = "+"+transcendence.Wisdom;
        forValue.text = "+"+transcendence.AdditionalForce;
        intValue.text = "+"+transcendence.AdditionalInt;
        dexValue.text = "+"+transcendence.AdditionalDex;
        conValue.text = "+"+transcendence.AdditionalCon;
        vigorValue.text = "+"+transcendence.AdditionalVigor;
    }
}
