using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Piercing Spikes")]
public class PiercingSpikes : Active
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

        if (pi.Mana <= MpCost) 
            return false;


        var spikes = Resources.Load("Prefabs/Combat/Wolf/Piercing Spikes") as GameObject;

        if (spikes){
            var g = Instantiate(spikes, Vector3.zero, spikes.transform.rotation) as GameObject;
            g.transform.position = player.transform.position;
        }

        return true;
    }
}
