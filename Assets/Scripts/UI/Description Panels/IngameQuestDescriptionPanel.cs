using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Serialization;

public class IngameQuestDescriptionPanel : MonoBehaviour
{
    public Quest quest;
    [SerializeField] private GameObject itemRewardTemplate = null;

    [Header("Texts")]
    [SerializeField] private TMP_Text questName = null;
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private TMP_Text goal = null;
    // [SerializeField] private TMP_Text reward = null;
    [SerializeField] private TMP_Text expReward = null;
    [SerializeField] private TMP_Text goldReward = null;
    [SerializeField] private Transform rewardItems = null;
    // [SerializeField] private RectTransform status = null;

    [Header("Buttons")]
    [SerializeField] private RectTransform buttons = null;
    [SerializeField] private Button accept = null;
    [SerializeField] private Button decline = null;
    [SerializeField] private Button complete = null;
    
    [Header("")]
    [SerializeField] private RectTransform content = null;
    //[SerializeField] private Scrollbar scrollbar = null;
    public Transform inProgressQuests;
    public Transform finishedQuests;
    public GameObject questTemplate;
    public Transform questTransform;
    public int index;

    [FormerlySerializedAs("descriptionPreferredHeight")] [Header("Panel Preffered Heights")]
    public float descriptionPreferredHeight;
    [FormerlySerializedAs("goalPrefferedHeight")] 
    public float goalPreferredHeight;

    public Vector3 center;
    public float radius;

    public static IngameQuestDescriptionPanel Instance { get; private set; }

    private IngameQuestDescriptionPanel (){
        if (Instance == null)
            Instance = this;

    }

    private void Awake() {
        
    }

    private void OnEnable() {
        EnableButtons();
    }

    public void Open () => gameObject.SetActive(true);
    public void Close () => gameObject.SetActive(false);

    public void Initialize (){
        questName = transform.GetComponentsInChildren<TMP_Text>().Where(x => x.name.Equals("Quest Name")).ToList()[0];
        description = transform.GetComponentsInChildren<TMP_Text>().Where(x => x.name.Equals("Description")).ToList()[0];
        goal = transform.GetComponentsInChildren<TMP_Text>().Where(x => x.name.Equals("Goal Description")).ToList()[0];
        expReward = transform.GetComponentsInChildren<TMP_Text>().Where(x => x.name.Equals("Experience Reward")).ToList()[0];
        goldReward = transform.GetComponentsInChildren<TMP_Text>().Where(x => x.name.Equals("Gold Reward")).ToList()[0];
        rewardItems = transform.GetComponentsInChildren<Transform>().Where(x => x.name.Equals("Reward Items")).ToList()[0];
        // itemRewardTemplate = transform.GetComponentsInChildren<GameObject>().Where(x => x.name.Equals("Reward Item Template")).ToList()[0];

        buttons = transform.GetComponentsInChildren<RectTransform>().Where(x => x.name.Equals("Buttons")).ToList()[0];
        accept = transform.GetComponentsInChildren<Button>().Where(x => x.name.Equals("Accept")).ToList()[0];
        complete = transform.GetComponentsInChildren<Button>().Where(x => x.name.Equals("Complete")).ToList()[0];
        decline = transform.GetComponentsInChildren<Button>().Where(x => x.name.Equals("Decline")).ToList()[0];

        content = transform.GetComponentsInChildren<RectTransform>().Where(x => x.name.Equals("Content")).ToList()[0];
    }

    public void UpdateText (){
        if (!quest)
            return;

        questName.text = $"{quest.QuestName}";
        description.text = quest.Description;
        goal.text = quest.GoalText();
        expReward.text = quest.Experience.ToString();
        goldReward.text = quest.GoldGiven.ToString();

        ResizePanel();
        EnableButtons();
    }

    private void EnableButtons (){
        accept.interactable = !quest.IsInProgress && !quest.IsCompleted;
        complete.interactable = quest.CanBeCompleted && !quest.IsCompleted? true : false;
        decline.interactable = quest.IsInProgress && !quest.IsMainQuest && !quest.IsCompleted ? true : false;
    }

    public void SetQuest (Quest q) => quest = q;


    private void ResizePanel (){
        var height = 0.0f;
        var rect = description.GetComponent<RectTransform>();
        var sizeDelta = rect.sizeDelta;
        sizeDelta = new Vector2(sizeDelta.x, description.preferredHeight);
        rect.sizeDelta = sizeDelta;
        description.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        rect = goal.GetComponent<RectTransform>();
        var delta = rect.sizeDelta;
        delta = new Vector2(delta.x, goal.preferredHeight);
        rect.sizeDelta = delta;
        goal.GetComponent<RectTransform>().sizeDelta = delta;

        var childCount = UpdateRewardDescription();
        var rewards = (childCount + 1) * rewardItems.GetComponent<GridLayoutGroup>().cellSize.y;

        height+= description.preferredHeight;
        height+= goal.preferredHeight;
        height+= expReward.preferredHeight;
        height+= rewards;
        height+= 175;
        //Debug.Log($"Size: {(childCount + 1)} * {rewardItems.GetComponent<GridLayoutGroup>().cellSize.y} = {rewards}");
        buttons.anchoredPosition = new Vector3(buttons.anchoredPosition.x, -rewards-50, 0);

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }

    private int UpdateRewardDescription (){
        if (!quest.Reward)
            return -1;
        var size = rewardItems.childCount;
        for (var i = size - 1; i >= 0 ; i--)
            Destroy(rewardItems.GetChild(i).gameObject);

        foreach (var item in ((ItemReward)quest.Reward).items)
            AddItemRewardOnDescriptionPanel(item);
        
        return ((ItemReward)quest.Reward).items.Count;
    }

    private void AddItemRewardOnDescriptionPanel (Item item){
        if (item == null)
            return;

        var transform1 = transform;
        var g = Instantiate(itemRewardTemplate, transform1.position, transform1.rotation);
        g.transform.SetParent(rewardItems); 
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;

        var itemTemplateText = g.transform.GetChild(0).GetComponent<TMP_Text>();
        itemTemplateText.text = item.ItemName;
        itemTemplateText.enabled = true;

        var itemTemplateImage = g.transform.GetComponent<Image>();
        itemTemplateImage.sprite = item.Sprite;
        itemTemplateImage.enabled = true;

        g.gameObject.SetActive(true);
    }

    public void Accept ()
    {
        if (!quest.OnAccept()) 
            return;
        
        UI_NotificationManager.Instance.NewQuestNotification(quest);
        var questTransform = QuestManager.Instance.AddQuest(quest);
        if (questTransform != null){
            QuestManager.Instance.OnAcceptQuest(questTransform);
        }
        ManualQuestMenu.Instance?.Npc.MoveQuestsThatCompleteInAnotherNPC();
        UpdateUI ();

    }
    public void Complete (){
        if (quest.CanBeCompleted){
            QuestManager.Instance.OnCompleteQuest(quest);
            UpdateUI ();
        }
    }
    public void Decline ()
    {
        if (!quest.OnDecline()) 
            return;
        
        QuestManager.Instance.OnDeclineQuest(quest);
        UpdateUI ();
    }

    public void UpdateUI (){
        EnableButtons();
        ManualQuestMenu.Instance?.Npc.HandleUpdate();
    }

}
