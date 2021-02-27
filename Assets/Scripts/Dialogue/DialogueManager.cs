using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text npcName;
    public TMP_Text dialogueText;
    public Scrollbar scrollBar;
    public GameObject dialogueDisplay;
    private Animator _animator;

    private static DialogueManager instance = null;

    public static DialogueManager Instance { get => instance; set => instance = value; }

    DialogueManager (){
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        IEnumerable<GameObject> d = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Equals("Dialogue Display"));
        IEnumerable<TMP_Text> dialogue = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.name.Equals("Dialogue Display Text"));

        dialogueDisplay = d.ToList()[0];
        npcName = dialogueDisplay.transform.GetChild(0).GetComponent<TMP_Text>();
        dialogueText = dialogue.ToList()[0];

        _animator = dialogueDisplay.GetComponent<Animator>();
        Debug.Log($"{name}");
    }

    public void AnimateIn (){
        dialogueDisplay.SetActive(true);
        scrollBar.value = 1;
        _animator.SetBool("isOpen", true);
    }

    public void AnimateOut (){
        _animator.SetBool("isOpen", false);
        StartCoroutine(TurnDisplayOff());
    }

    private IEnumerator TurnDisplayOff (){
        yield return new WaitForSeconds(0.5f);
        dialogueDisplay.SetActive(false);
    }
        
}
