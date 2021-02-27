using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Quests/Rewards/Item Reward")]
public class ItemReward : Reward
{
    public List<Item> items;
    public override bool GiveReward()
    {
        Player player = Player.Instance;
        foreach (Item item in items)
        {
            Type type = Type.GetType($"{item.GetType()}Handler");
            if (item != null)
                player.OnCreateItem(item);
            //Debug.Log($"Criando item do tipo {item.GetType()} e handler do tipo {item.MyHandler().GetType()}");
        }
        //ItemHandler s = (ItemHandler) Activator.CreateInstance(type);
        return true;
    }
}
