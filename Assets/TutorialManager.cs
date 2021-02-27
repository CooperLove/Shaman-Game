using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image bg = null;
    [SerializeField] private TMP_Text text_bubble = null;

    [SerializeField] private Queue<string> sentences = new Queue<string>();
    [SerializeField] private List<Transform> highlightsTransforms = new List<Transform>();
    [SerializeField] private Queue<Transform> highlights = new Queue<Transform>();
    private bool isOnTutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        sentences.Enqueue("Essa é a barra de habilidades <color=green>primária</color>. Aqui ficam as skills que utilizam estamina.");
        sentences.Enqueue("Esse é o ataque básico. Pressione a tecla <color=green>J</color> para utilizar esta habilidade.");
        sentences.Enqueue("Esse é seu ataque fraco. Pressione a tecla <color=green>L</color> para utilizar esta habilidade.");
        sentences.Enqueue("Esse é seu ataque médio. Pressione a tecla <color=green>K</color> para utilizar esta habilidade.");
        sentences.Enqueue("Esse é seu ataque alto. Pressione a tecla <color=green>I</color> para utilizar esta habilidade.");
        sentences.Enqueue("Essa é a barra de habilidades <color=green>secundaria</color>. Essas habilidades podem ser aprendidas no decorrer do jogo.");

        foreach (Transform t in highlightsTransforms)
        {
            highlights.Enqueue(t);
        }
        highlightsTransforms = null;
        //GameStatus.isOnTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnTutorial){
            NextHighlight ();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")){
            isOnTutorial = true;
            Player.Instance.IgnoreCommands = true;
            Player.Instance.GetComponent<AnimationsScript>().OnRunExit();
            bg.gameObject.SetActive(true);
            text_bubble.transform.parent.gameObject.SetActive(true);
            NextHighlight ();
        }
    }

    private void NextHighlight (){
        if (highlights.Count <= 0){
            GameStatus.isOnTutorial = false;
            isOnTutorial = false;
            Player.Instance.IgnoreCommands = false;
            bg.gameObject.SetActive(false);
            text_bubble.transform.parent.gameObject.SetActive(false);
            SceneClipManager.Instance.play = true;
            return;
        }
        Transform t = highlights.Dequeue();
        if (t.tag.Equals("CharacterMenu")){
            Color c = t.transform.GetChild(1).GetComponent<Image>().color;
            t.transform.GetChild(1).GetComponent<Image>().color = new Color (c.r, c.g, c.b, 1);
        }else{
            Color c = t.transform.GetComponent<Image>().color;
            t.transform.GetComponent<Image>().color = new Color (c.r, c.g, c.b, 1);
            Color c2 = t.transform.GetChild(0).GetComponent<Image>().color;
            t.transform.GetChild(0).GetComponent<Image>().color = new Color (c2.r, c2.g, c2.b, 1);
        }
        text_bubble.text = sentences.Dequeue();
        text_bubble.transform.parent.SetParent(t);
        text_bubble.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3 (35, 0, 0);
    }

    
}
