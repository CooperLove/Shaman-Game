using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Crocodile/Auto Attack Crocodile")]
public class CrocodileBasicAttack : Active
{
    public override void OnLearn()
    {

        Player.Instance.gameObject.AddComponent<BasicAttackPhysics>();

        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;

        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Stamina <= SpCost) 
            return false;
        
        Player player = Player.Instance;
        Mark mark = player.GetComponent<Mark>();
        mark.Gain(GainOfEnergyPerUse);
        
        player.TryGetComponent(out BasicAttackPhysics autoAttack);
        if (autoAttack == null){
            Player.Instance.gameObject.AddComponent(typeof(BasicAttackPhysics));
            autoAttack = player.GetComponent<BasicAttackPhysics>();
        }
        autoAttack.Use();

        pi?.Passive.OnUse();

        pi.Stamina -= SpCost;
        return true;
    }
}
