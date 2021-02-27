using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UI_NotificationManager : MonoBehaviour
{
    private static UI_NotificationManager instance = null;
    public static UI_NotificationManager Instance { get => instance; set => instance = value; }
    private Transform notifications = null;
    public TMP_Text pickUpText = null;

    UI_NotificationManager (){
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IEnumerable<TMP_Text> tMP = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.name == "Pick Up Notification");
        notifications = Resources.FindObjectsOfTypeAll<Transform>().Where(x => x.name == "Announcements").ToList()[0];
        List<TMP_Text> texts = tMP.ToList(); 
        pickUpText = texts[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePickUpText (Item item){
        Color c = ColorTable.ItemNameColor((int) item.GetRarity());
        pickUpText.text = $"Pick <color=#{ColorUtility.ToHtmlStringRGB(c)}>{item.ItemName}</color> up";
    }

    public void NewQuestNotification (Quest quest){
        GameObject newQuestNotification = Resources.Load("Prefabs/New Quest Notification") as GameObject;
        if (newQuestNotification != null){
            newQuestNotification.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{quest.QuestName} started";
            GameObject g = Instantiate(newQuestNotification, transform.position, transform.rotation);
            g.transform.SetParent(notifications);
            RectTransform rect = g.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector3(300, -170, 0f);
        }
    }

    public void ItemEnhancementNotification (string res, Color color){
        GameObject enhanceResult = Resources.Load<GameObject>("Prefabs/Enhance Result") as GameObject;

        GameObject result = Instantiate(enhanceResult, enhanceResult.transform.position, enhanceResult.transform.rotation);
        result.transform.GetChild(0).GetComponent<TMP_Text>().text = res;
        result.transform.GetChild(0).GetComponent<TMP_Text>().color = color;
        AutoDestroy destroy = result.AddComponent<AutoDestroy>();
        destroy.timeType = AutoDestroy.TimeType.Unscaled;
        destroy.destroyTime = 3f;
        result.transform.SetParent(notifications);
        result.transform.localScale = Vector3.one;
        RectTransform rect = result.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, -600, 0);
    }
}
