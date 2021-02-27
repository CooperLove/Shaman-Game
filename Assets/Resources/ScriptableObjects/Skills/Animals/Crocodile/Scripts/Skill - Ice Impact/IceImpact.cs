using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Ice Impact")]
public class IceImpact : Active
{
    public override void OnLearn()
    {
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        Player player = Player.Instance;

        if (pi.Mana <= MpCost || player.OnGround) 
            return false;


        var impact = Resources.Load("Prefabs/Combat/Wolf/Ice Impact") as GameObject;

        if (impact){
            var g = Instantiate(impact, Vector3.zero, impact.transform.rotation) as GameObject;
        }

        return true;
    }
}
