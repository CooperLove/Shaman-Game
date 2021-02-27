using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Stats/Percentage HP")]
public class StatsSkill_PercentageHP : Active
{
    public override void OnLearn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpgrade()
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Bonus_percentage_hp = 0;
        pi.Percentage_hp += GainOfEnergyPerUse;
        pi.Bonus_percentage_hp += (pi.Max_HP + pi.Bonus_hp) * (pi.Percentage_hp/100);
        pi.ActiveSkillPoints -= PointsPerLevel;
    }

    public override bool OnUse()
    {
        return false;
    }
}
