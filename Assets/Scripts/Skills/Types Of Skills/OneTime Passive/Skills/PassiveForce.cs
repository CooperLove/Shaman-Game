using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Passives/Flat/Force")]
public class PassiveForce : OneTimePassive
{
    public override void OnLearn()
    {
        
    }

    public override bool OnLearn(float value, bool isFlat = true)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Force += (int) value;
        //Debug.Log($"Added {value} points in for");
        
        return true;
    }
}
