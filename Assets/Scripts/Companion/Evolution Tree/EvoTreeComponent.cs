using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvoTreeComponent : MonoBehaviour
{
    public TranscendenceInfo info;
    private Sprite companionImage;
    [SerializeField] private Image[] ways = null;
    [SerializeField] private GameObject[] childs = null;
    [SerializeField] private List<Quest> quests = new List<Quest>();
    public int chosenPath;
    public float p;
    public int numWays { get => ways.Length; }
    public List<Quest> Quests { get => quests; set => quests = value; }
    public GameObject[] Childs { get => childs; set => childs = value; }

    // Start is called before the first frame update
    void Start()
    {
        chosenPath = -1;
        companionImage = info.Sprite;
        ChangeImage();
    }

    public void ChangeImage () => GetComponent<Image>().sprite = companionImage;
    public void FillPath (int index) => ShowPathImage(index);
    public void UnfillPath (int index) => ResetPathImageColor(index);
    public void FillAllPaths () {
        StopAllCoroutines();
        for (int i = 0; i < ways.Length; i++)
            StartCoroutine (FillPathImage(i, p));
    }

    public void ShowPathImage (int index){
        if (index < 0 || index > ways.Length)
            return;

        ways[index].gameObject.SetActive(true);
        ways[index].gameObject.GetComponent<Image>().enabled = true;
        ways[index].fillAmount = 1;
        ways[index].color = Color.red;
        Childs[index].SetActive(true);
    }

    public void ResetPathImageColor (int index){
        if (index > ways.Length)
            return;

        ways[index].color = Color.cyan;
    }
    public IEnumerator FillPathImage (int index, float percentage){
        
        if (index > ways.Length)
           StopCoroutine("FillPathImage");

        if (percentage > 1)
            percentage = 1;
        ways[index].fillAmount = 0;
        while (ways[index].fillAmount < percentage){
            ways[index].fillAmount += Time.unscaledDeltaTime;
            yield return null;
        }

        if (ways[index].fillAmount == 1){
            Childs[index].SetActive(true);
            if (Childs[index].transform.childCount > 0){
                Childs[index].transform.GetChild(Childs[index].transform.childCount - 1).GetComponent<EvoTreeComponent>().companionImage = companionImage;
            }
        }
        else
            Childs[index].SetActive(false);
    }

    public void OnClick (){
        CompanionDescriptionPanel.Instance.UpdateUI(info);
        CompanionDescriptionPanel.Instance.SetEvoComponent(this);
        CompanionDescriptionPanel.Instance.ActivateButtons();
        EvolutionTree.Instance.Close();
        CompanionDescriptionPanel.Instance.Open();
    }
}
