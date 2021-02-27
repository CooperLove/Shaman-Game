using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Middle Ice Pillar")]
public class MiddleIcePillar : Active
{
    public float energyPerUse;

    public override void OnLearn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        Player player = Player.Instance;
        PassiveCrocodileSO passive = (PassiveCrocodileSO) player.GetComponent<CrocodileAttacks>().Passive;
        passive.Use(energyPerUse);

        return true;
    }
}
