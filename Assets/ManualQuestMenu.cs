using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class ManualQuestMenu : MonoBehaviour
{
    private static ManualQuestMenu instance = null;
    [SerializeField] private ManualQuestNPC npc = null;
    [SerializeField] private TMP_Text npcName = null;
    [SerializeField] private TMP_Text npcDescription = null;
    [SerializeField] private RectTransform content = null;
    [SerializeField] private GameObject questTemplate = null;

    public static ManualQuestMenu Instance { get => instance;}
    public ManualQuestNPC Npc { get => npc; }

    ManualQuestMenu (){
        if (instance == null)
            instance = this;
    }

    private void OnEnable() {
        GameStatus.isInteractingWithNpc = true;
    }

    private void OnDisable() {
        GameStatus.isInteractingWithNpc = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && IngameQuestDescriptionPanel.Instance.gameObject.activeInHierarchy){
            IngameQuestDescriptionPanel.Instance.Close();
        }else if (Input.GetKeyDown(KeyCode.Escape)){
            Close();
        }
    }

    public void Open (ManualQuestNPC npc) {
        this.npc = npc;
        UpdateText();
        gameObject.SetActive(true);
    }
    public void Close () => gameObject.SetActive(false);
    public void Initialize (){
        npcName = transform.GetChild(0).GetComponent<TMP_Text>();
        npcDescription = transform.GetChild(1).GetComponent<TMP_Text>();
        content = transform.GetComponentsInChildren<RectTransform>().Where(x => x.name.Equals("Content")).ToList()[0];
    }

    public void AddQuest (Quest q){
        GameObject g = Instantiate(questTemplate, transform.position, transform.rotation);

        g.transform.SetParent(content);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        PlayerQuestHandler handler = g.GetComponent<PlayerQuestHandler>();
        handler.Quest = q;

        g.SetActive(true);
    }

    public bool ClearContent (ManualQuestNPC npc){
        if (npc.Equals(this.Npc))
            return false;

        int length = content.childCount;
        for (int i = length - 1; i >= 0 ; i--)
            Destroy(content.GetChild(i).gameObject);
        
        return true;
    }

    public void UpdateText (){
        if (Npc == null)
            return;

        npcName.text = Npc.Name;
        npcDescription.text = Npc.description;
    }

    public void ResizeContent (){
        content.sizeDelta = new Vector2(content.sizeDelta.x, content.childCount * content.GetComponent<GridLayoutGroup>().cellSize.y);
    }
}
