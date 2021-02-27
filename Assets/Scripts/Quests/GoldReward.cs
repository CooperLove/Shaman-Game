using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Quests/Rewards/Gold Reward")]
public class StatsReward : Reward
{

    public int Force { get; set; }
    public int Intelligence { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Vigor { get; set; }

    public override bool GiveReward()
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        
        return true;
    }
}
