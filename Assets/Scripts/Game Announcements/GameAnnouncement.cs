using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class GameAnnouncement
{
    private static Player player = Player.Instance;
    private static readonly Camera MainCamera = Camera.main;
    private static readonly Transform UICanvasTransform = Object.FindObjectsOfType<Transform>().FirstOrDefault(x => x.name == "UI Canvas");
    private static readonly GotNewItemList GotNewItemList = Object.FindObjectsOfType<GotNewItemList>().FirstOrDefault(x => x.name == "Got New Item List");
    private static readonly GotNewItemList GotNewQuestList = Object.FindObjectsOfType<GotNewItemList>().FirstOrDefault(x => x.name == "Got New Quest List");
    private static List<string> lastEnemiesEncountered = new List<string>();
    private static GameObject lastEnemyAnnouncement = null;
    private const float ResetEnemyAnnouncement = 3f;
    private const float ResetEnemyTimer = 25f;
    private static float resetTimer = 0f;
    private static float resetAnnouncementTimer = 0f;
    private static GameObject manaText = Resources.Load("Prefabs/Combat/ManaText") as GameObject;

    public static void OnLevelUp (Vector3 pos)
    {
        var screenPoint = MainCamera.ViewportToScreenPoint(pos);
        var worldPoint = MainCamera.ScreenToWorldPoint(screenPoint);
        var levelUp = Resources.Load("Prefabs/Announcements/Level Up") as GameObject;

        if (!levelUp) 
            return;
        
        var g = Object.Instantiate(levelUp, new Vector3(worldPoint.x, worldPoint.y, 0f), levelUp.transform.rotation);
        g.transform.SetParent(UICanvasTransform);
    }

    public static void OnLearnSkill (Skill s)
    {
        
    }

    public static void OnUpgradeSkill(Skill s)
    {
        
    }

    public static void OnGetItem (Item item, bool rare)
    {
        var p = rare ? new Vector3(0.5f, 0.65f, 0) : new Vector3(0.08f, 0.82f, 0);
        var screenPoint = MainCamera.ViewportToScreenPoint(p);
        var worldPoint = MainCamera.ScreenToWorldPoint(screenPoint);
        var newItem = Resources.Load("Prefabs/Announcements/"+(rare ? "Got Rare Item" : "Got Item")) as GameObject;

        if (!newItem) 
            return;
        
        var g = Object.Instantiate(newItem, new Vector3(worldPoint.x, worldPoint.y, 0f), newItem.transform.rotation);
        
        g.transform.GetChild(0).GetComponent<TMP_Text>().SetText((rare ? "Obtained " : "") +$"{item.ItemName}");
        g.transform.SetParent(rare ? UICanvasTransform : GotNewItemList.transform);
        g.transform.localScale = Vector3.one;
        g.gameObject.SetActive(rare);
        
        if (rare)
            return;
        
        GotNewItemList.Run(g.transform);
    }
    
    public static void OnEnhanceItem (bool result)
    {
        
    }

    public static void OnAcceptQuest (Quest q)
    {
        var p = new Vector3(0.08f, 0.82f, 0);
        var screenPoint = MainCamera.ViewportToScreenPoint(p);
        var worldPoint = MainCamera.ScreenToWorldPoint(screenPoint);
        var newQuest = Resources.Load("Prefabs/Announcements/New Quest") as GameObject;

        if (!newQuest) 
            return;
        
        var g = Object.Instantiate(newQuest, new Vector3(worldPoint.x, worldPoint.y, 0f), newQuest.transform.rotation);
        
        g.transform.GetChild(0).GetComponent<TMP_Text>().SetText($"{q.QuestName} started");
        g.transform.SetParent(GotNewQuestList.transform);
        g.transform.localScale = Vector3.one;
        
        GotNewQuestList.Run(g.transform);
    }
    
    public static void OnCompleteQuest (Quest q)
    {
        
    }

    public static void On_Show_Enemy_Name_When_Enter_Combat (Enemy e)
    {

        var worldPoint = new Vector3(0, 485, 0);
        var enemyName = Resources.Load("Prefabs/Announcements/Enemy Name") as GameObject;
        
        if (!enemyName)
            return;
        

        if (!lastEnemiesEncountered.Contains(e.enemyInfo.ObjectName)) {
            lastEnemiesEncountered.Add(e.enemyInfo.ObjectName);
            Player.Instance.StartCoroutine(ResetLastEnemyEncountered());
            Debug.Log($"{e.enemyInfo.ObjectName} wasn't hit in the last 30s");
        }else
            return;
        
        if (lastEnemyAnnouncement) {
            Player.Instance.StartCoroutine(WaitUntilEnemyAnnouncementResets(enemyName, worldPoint, e));
            return;
        }

        InstantiateEnemyNameAnnouncement(enemyName, worldPoint, e);
    }

    private static void InstantiateEnemyNameAnnouncement(GameObject enemyName, Vector3 worldPoint, Enemy e)
    {
        Debug.DrawLine(Player.Instance.transform.position, worldPoint, Color.yellow, 3f);
        var g = Object.Instantiate(enemyName, new Vector3(worldPoint.x, worldPoint.y, 0f), enemyName.transform.rotation);

        g.transform.GetChild(0).GetComponent<TMP_Text>().SetText(e.enemyInfo.ObjectName);
        g.transform.SetParent(UICanvasTransform);
        g.transform.localScale = Vector3.one;
        g.GetComponent<RectTransform>().anchoredPosition = worldPoint;
        
        lastEnemyAnnouncement = g;
        Player.Instance.StartCoroutine(ResetLastEnemyEncounteredAnnouncement());
    }

    private static IEnumerator ResetLastEnemyEncountered()
    {
        resetTimer = 0f;
        //yield return new WaitForSeconds(ResetEnemyTimer);
        while (resetTimer < ResetEnemyTimer)
        {
            resetTimer += Time.deltaTime;
            yield return null;
        }
        lastEnemiesEncountered.Clear();
    }
    
    private static IEnumerator ResetLastEnemyEncounteredAnnouncement()
    {
        resetAnnouncementTimer = 0f;
        //yield return new WaitForSeconds(ResetEnemyAnnouncement);
        while (resetAnnouncementTimer < ResetEnemyAnnouncement)
        {
            resetAnnouncementTimer += Time.deltaTime;
            yield return null;
        }
        lastEnemyAnnouncement = null;
    }
    
    private static IEnumerator WaitUntilEnemyAnnouncementResets(GameObject enemyName, Vector3 worldPoint, Enemy e)
    {
        yield return new WaitUntil(() => lastEnemyAnnouncement == null);
        
        InstantiateEnemyNameAnnouncement(enemyName, worldPoint, e);
    }

    public static void OnEnterNewArea (string areaName)
    {
        var screenPoint = MainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.8f, 0));
        var worldPoint = MainCamera.ScreenToWorldPoint(screenPoint);
        var area = Resources.Load("Prefabs/Announcements/Enter New Area") as GameObject;

        if (!area) 
            return;
        
        var g = Object.Instantiate(area, new Vector3(worldPoint.x, worldPoint.y, 0f), area.transform.rotation);
        
        g.transform.GetChild(0).GetComponent<TMP_Text>().SetText(areaName);
        g.transform.SetParent(UICanvasTransform);
        g.transform.localScale = Vector3.one;
    }

    public static void NotEnoughManaAnnouncement (){
        GameObject text = Object.Instantiate(manaText, Player.Instance.transform.position, manaText.transform.rotation);
        text.GetComponent<DamageText>().SetText("Not enough mana!", 0, 1);
    }
}
