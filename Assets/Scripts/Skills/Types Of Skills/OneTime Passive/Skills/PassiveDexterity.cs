using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Passives/Flat/Dexterity")]
public class PassiveDexterity : OneTimePassive
{
    public override bool OnLearn(float value, bool isPercentage = false)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Dexterity += (int) value;
        Debug.Log($"Added {value} points in dex");
        
        return true;
    }
}
