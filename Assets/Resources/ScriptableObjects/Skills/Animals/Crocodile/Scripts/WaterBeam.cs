using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Water Beam")]
public class WaterBeam : Active
{
    public override void OnLearn()
    {
        CurrentLevel = 1;
        Player player = Player.Instance;
        player.TryGetComponent(out WaterBeamPhysics beam);
        if (beam == null){
            Player.Instance.gameObject.AddComponent(typeof(WaterBeamPhysics));
        }
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        Player player = Player.Instance;
        Mark mark = player.GetComponent<Mark>();
        
        if (pi.Mana <= MpCost) 
            return false;

        WaterBeamPhysics beam = player.GetComponent<WaterBeamPhysics>();
        beam.Use();

        PassiveCrocodile passive = player.GetComponent<PassiveCrocodile>();
        passive.Use(GainOfEnergyPerUse);

        mark.Gain(GainOfEnergyPerUse);

        pi.Mana -= MpCost;

        return true;
    }
}
