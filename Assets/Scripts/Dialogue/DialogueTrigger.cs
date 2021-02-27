using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool hitPlayer, speak;
    public GameObject canSpeak;
    /*
    private void Update() {
        if (hitPlayer && Input.GetKeyDown(KeyCode.E)  || (speak && hitPlayer) ){
            DialogueManager dm = FindObjectOfType<DialogueManager>();
            //Debug.Log(dm.animator.GetBool("isOpen"));
            if (!dm.animator.GetBool("isOpen")){
                Debug.Log("Opened");
                dm.AnimateIn();
                dm.StartDialogue(dialogue);
                dm.dialogueTrigger = this;
                canSpeak.SetActive(false);
                speak = false;
                Player.Instance.ignoreCommands = true;
            }
            else{
                dm.AnimateOut();
                speak = false;
                canSpeak.SetActive(true);
                Player.Instance.ignoreCommands = false;
            }
        }
    }
    */
    
    public virtual void TriggerDialogue (){
        
    }
    public abstract void OnDialogueEnd ();
    /*
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(this.name+" "+other.transform.gameObject.name);
        if (other.tag == "Player"){
            hitPlayer = true;
            canSpeak.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            hitPlayer = false;
            canSpeak.SetActive(false);
            FindObjectOfType<DialogueManager>().AnimateOut();
        }
    }
    private void OnBecameVisible() {
        dialogue.nameDisplay.SetActive(true);
        dialogue.nameText.text = dialogue.name;
    }
    private void OnBecameInvisible() {
        dialogue.nameDisplay.SetActive(false);
    }
    */
}
