using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Passives/Flat/Intelligence")]
public class PassiveIntelligence : OneTimePassive
{
    public override bool OnLearn(float value, bool isPercentage = false)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Intelligence += (int) value;
        Debug.Log($"Added {value} points in int");
        
        return true;
    }
}
