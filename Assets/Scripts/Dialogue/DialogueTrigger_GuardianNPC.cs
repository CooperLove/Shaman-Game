using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DialogueTrigger_GuardianNPC : DialogueTrigger
{
    public GameObject challengeWindow;
    public Button accept;
    public GameObject dialogueDisplay;
    public Conversation conversation;
    public GameObject boss;
    public GameObject wall;
    public GameObject wall2;
    
    
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(GameStatus.isCutScenePlaying);
        if (!GameStatus.isCutScenePlaying && other.tag == "Player"){
            canSpeak.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            canSpeak.SetActive(false);
        }
    }
    private void OnBecameVisible() {
    }
    private void OnBecameInvisible() {
    }

    public override void OnDialogueEnd()
    {
        Debug.Log("Guardian npc end");
        challengeWindow.SetActive(true);
        accept.onClick.RemoveAllListeners();
        accept.onClick.AddListener(OnAcceptChallenge);
        GetComponent<Guardian>().Open();
    }
    
    public void OnAcceptChallenge (){
        Debug.Log("Challenge begin! "+this.name);
        GameStatus.isOnChallenge = true;
        challengeWindow.SetActive(false);
        Player.Instance.IgnoreCommands = false;
        wall?.SetActive(true);
        wall2?.SetActive(true);
        boss?.SetActive(true);
        BossUIManager.Instance.Boss = boss.GetComponentInChildren<Boss>();
        BossUIManager.Instance.Initialize();
        string typeName = gameObject.name;
        var type = Type.GetType(typeName);
        var attack = (AnimalAttacks) Activator.CreateInstance(type);
        AnimalAttacks a = (AnimalAttacks) Player.Instance.gameObject.GetComponent(attack.GetType());
        a.enabled = true;
    }
}
