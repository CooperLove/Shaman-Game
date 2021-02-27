using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Passives/Flat/Constitution")]
public class PassiveConstitution : OneTimePassive
{
    public override bool OnLearn(float value, bool isPercentage = false)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Constitution += (int) value;
        Debug.Log($"Added {value} points in con");
        
        return true;
    }
}
