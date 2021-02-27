using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Linq;

public class Conversation : MonoBehaviour
{
    [SerializeField] private NPC npc = null;
    public bool shown = false;
    public bool canStartDialogue = false;
    private bool canDisplayDialogue = false;
    [SerializeField] private TMP_Text npcName = null;
    [SerializeField] private TMP_Text dialogueText = null;
    [SerializeField] private List<NPC_Sentences> npcs = new List<NPC_Sentences>();
    [SerializeField] private Queue<string> sentences = new Queue<string>();
    private int currentDialogue = 0;
    private List<int> npcStartIndex = new List<int>();
    private float timeBetweenLetters = 0;

    private void Awake() {
        canStartDialogue = true;
        npcName = DialogueManager.Instance.npcName;
        dialogueText = DialogueManager.Instance.dialogueText;
    }
    private void Update() {
        if (GameStatus.isCutScenePlaying || GameStatus.isGamePaused)
            return;
        // if (Input.GetKeyDown(KeyCode.E) && canStartDialogue )
        //     StartDialogue();
        if (Input.GetKeyDown(KeyCode.Space) && canDisplayDialogue )
            DisplayNextSentence();
    }

    public void StartDialogue (){
        if (!canStartDialogue)
            return;

        canStartDialogue = false;
        canDisplayDialogue = true;
        Player.Instance.IgnoreCommands = true;
        sentences.Clear();
        currentDialogue = 0;
        npcName.text = npcs[0].name;
        int i = 0;
        foreach (NPC_Sentences npc in npcs)
        {
            npcStartIndex.Add(i);
            foreach (string sentence in npc.sentences)
            {
                sentences.Enqueue(sentence);
                //Debug.Log(sentence);
                i++;
            }
        }
        //foreach (int index in npcStartIndex)
            //Debug.Log(index);
        //DialogueManager.Instance.dialogueDisplay.SetActive(true);
        DisplayNextSentence ();
        
    }

    public void DisplayNextSentence (){
        //Debug.Log("Next sentence: "+sentences.Count);
        if (sentences.Count == 0){
            DialogueEnd();
            return;
        }
        DialogueManager.Instance.AnimateIn();
        string sentence = sentences.Dequeue();
        if (npcStartIndex.Contains(currentDialogue)){
            int index = npcStartIndex.IndexOf(currentDialogue);
            npcName.text = npcs[index].name;
        }
        currentDialogue++;
        //Debug.Log($"{npcName.text} {currentDialogue} => {sentence}");
        StopAllCoroutines();
        StartCoroutine (TypeSentence(sentence));
    }

    public void DialogueEnd (){
        //Debug.Log("Dialogue ended! "+sentences.Count);
        sentences = new Queue<string>();
        npcStartIndex = new List<int>();
        DialogueManager.Instance.AnimateOut();
        Player.Instance.IgnoreCommands = false;

        canStartDialogue = true;
        canDisplayDialogue = false;
        
        //if (this.name.Equals("Conv Boss Final Dialogue"))
        //    SceneLevelManager.Instance.LoadScene(2);

        npc?.OnEndConversation();
    }

    private IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if (other.gameObject.tag.Equals("Player")){
        //     if (this.tag == "NPC"){
        //         StartDialogue();
        //         return;
        //     }                
        //     canStartDialogue = true;
        //     Debug.Log("Tag: "+other.gameObject.tag);
        // }
    }
    private void OnTriggerExit2D(Collider2D other) {
        // if (other.gameObject.tag.Equals("Player")){
        //     canStartDialogue = false;
        //     Debug.Log("Tag: "+other.gameObject.tag);
        // }
    }
}
