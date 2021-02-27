using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public static class SavingSystem
{
    public static void Save (){
        //SavePlayer();
        SavePlayerInfo();
        SaveSkills();
        SaveInventory();
    }

    public static void Load (){
        //LoadPlayer();
        LoadPlayerInfo();
        LoadSkills();
        LoadInventory();
    }

    private static void SavePlayer () {
        string player = JsonUtility.ToJson(Player.Instance);

        System.IO.File.WriteAllText("Player.json", JToken.Parse(player).ToString());
        PlayerPrefs.SetString("Player", JToken.Parse(player).ToString());
    }

    private static void LoadPlayer (){
        var playerData = PlayerPrefs.GetString("Player");
        JsonUtility.FromJsonOverwrite(playerData, Player.Instance); 
    }
    private static void LoadPlayerInfo (){
        var piData = PlayerPrefs.GetString("PlayerInfo");
        JsonUtility.FromJsonOverwrite(piData, Player.Instance.playerInfo);
    }
    private static void SavePlayerInfo () {
        string pi = JsonUtility.ToJson(Player.Instance.playerInfo);

        System.IO.File.WriteAllText("PlayerInfo.json", JToken.Parse(pi).ToString());
        PlayerPrefs.SetString("PlayerInfo", JToken.Parse(pi).ToString());
    }

    private static void SaveSkills (){
        string skills = "";
        int i = 01;
        int size = Player.Instance.playerInfo.LearnedSkills.Count;
        foreach (Active s in Player.Instance.playerInfo.LearnedSkills)
        {
            string sk = JToken.Parse( JsonUtility.ToJson(s)).ToString();
            skills += $"\"skill0{i}\":"+sk+(i >= 1 && i < size ? "," : "");
            i++;
        }
        skills = "{"+skills+"}";
        System.IO.File.WriteAllText("Skills.json", JToken.Parse(skills).ToString());
        PlayerPrefs.SetString("Skills", JToken.Parse(skills).ToString());
    }

    private static void LoadSkills (){
        var skills = PlayerPrefs.GetString("Skills");
        JObject s = JObject.Parse(skills);
        int i = 1;
        string index = "skill0";
        foreach (Active skill in Player.Instance.playerInfo.LearnedSkills)
        {
            JsonUtility.FromJsonOverwrite(s.SelectToken((index+i++)).ToString(), skill);
        }
    }

    private static void SaveInventory (){
        string inventory = JsonUtility.ToJson(Inventory.Instance);

        System.IO.File.WriteAllText("Inventory.json", JToken.Parse(inventory).ToString());
        PlayerPrefs.SetString("Inventory", JToken.Parse(inventory).ToString());

        SaveInventoryItems();
    }
    private static void LoadInventory (){
        var inventory = PlayerPrefs.GetString("Inventory");
        JsonUtility.FromJsonOverwrite(inventory, Inventory.Instance);

        LoadInventoryItems();

        int size = Inventory.Instance.Items.Count;
        for (int i = 0; i < size; i++)
        {
           ItemHandler handler = Inventory.Instance.CreateItem(Inventory.Instance.Items[i], true);
           Inventory.Instance.Handlers.Add(handler);
        }

        LoadInventoryHandlers();

        ConsumableBar.Instance.Open();
        Player.Instance.StartCoroutine(ConsumableBar.Instance.OnLoad());
        ConsumableBar.Instance.Close();
    }

    private static void SaveInventoryItems (){
        string items = "";
        int i = 00;
        int size = Inventory.Instance.Items.Count;
        foreach (Item item in Inventory.Instance.Items)
        {
            string sk = JToken.Parse( JsonUtility.ToJson(item)).ToString();
            items += $"\"item{i}\":"+sk+",";
            i++;
        }
        i = 0;
        foreach (ItemHandler rarity in Inventory.Instance.Handlers)
        {
            string sk = JToken.Parse( JsonUtility.ToJson(rarity)).ToString();
            items += $"\"handler{i}\":"+sk+(i >= 0 && i < size ? "," : "");
            i++;
        }
        items = "{"+items+"}";
        System.IO.File.WriteAllText("InventoryItems.json", JToken.Parse(items).ToString());
        PlayerPrefs.SetString("InventoryItems", JToken.Parse(items).ToString());
    }
    private static void LoadInventoryItems (){
        var skills = PlayerPrefs.GetString("InventoryItems");
        JObject s = JObject.Parse(skills);
        int i = 0;
        string index = "item";

        foreach (Item item in Inventory.Instance.Items)
            JsonUtility.FromJsonOverwrite(s.SelectToken((index+i++)).ToString(), item);
    }
    private static void LoadInventoryHandlers (){
        var skills = PlayerPrefs.GetString("InventoryItems");
        JObject s = JObject.Parse(skills);
        int i = 0;
        string handlerIndex = "handler";

        foreach (ItemHandler handler in Inventory.Instance.Handlers)
            JsonUtility.FromJsonOverwrite(s.SelectToken((handlerIndex+i++)).ToString(), handler);
        
    }
}
