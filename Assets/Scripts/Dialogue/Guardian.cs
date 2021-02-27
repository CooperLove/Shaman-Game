using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Guardian : MonoBehaviour
{
    public GuardianInfo guardianInfo;
    public TMP_Text nameText, descriptionText;
    public Image iconUI, tribeUI;
    public Animator menuAnimator;
    public GameObject tribeWindow, challengeWindow;
    

    public void Open (){
        //Debug.Log("OnEnable "+this.name);
        nameText.text = guardianInfo.name;
        descriptionText.text = guardianInfo.description;
        iconUI.sprite = guardianInfo.icon;
        tribeUI.sprite = guardianInfo.tribe;
        Player.Instance.IgnoreCommands = true;
    }
    private void OnEnable() {
        //Debug.Log("OnEnable "+this.name);
        nameText.text = guardianInfo.name;
        descriptionText.text = guardianInfo.description;
        iconUI.sprite = guardianInfo.icon;
        tribeUI.sprite = guardianInfo.tribe;
        //menuAnimator.SetBool("isOpen", true);
    }

    public void OpenWindow (){
        tribeWindow.SetActive (!tribeWindow.activeInHierarchy);
        challengeWindow.SetActive(!challengeWindow.activeInHierarchy);
        Player.Instance.IgnoreCommands = true;
    }
    public void CloseWindow (){
        tribeWindow.SetActive(false);
        challengeWindow.SetActive(false);
        Player.Instance.IgnoreCommands = false;
    }
    public void OnAcceptChallenge (){
        Debug.Log("Challenge begin!!");
        GetComponent<DialogueTrigger_GuardianNPC>().OnAcceptChallenge();
    }
}

[System.Serializable]
public class GuardianInfo {
    public string name;
    [TextArea(3,25)]
    public string description;
    public Sprite icon, tribe;
}