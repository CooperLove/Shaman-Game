using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger_RegularNPC : DialogueTrigger
{
    public override void OnDialogueEnd()
    {
        Debug.Log("Regular npc end");
    }

    public override void TriggerDialogue()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(this.name+" "+other.transform.gameObject.name);
        if (!GameStatus.isCutScenePlaying && other.tag == "Player"){
            hitPlayer = true;
            canSpeak.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            hitPlayer = false;
            canSpeak.SetActive(false);
            //FindObjectOfType<DialogueManager>().AnimateOut();
        }
    }
    private void OnBecameVisible() {
        dialogue.nameDisplay.SetActive(true);
        dialogue.nameText.text = dialogue.name;
    }
    private void OnBecameInvisible() {
        dialogue.nameDisplay.SetActive(false);
    }
}
