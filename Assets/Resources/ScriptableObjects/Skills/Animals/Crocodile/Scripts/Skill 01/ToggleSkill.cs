using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Toggle Skill")]
public class ToggleSkill : Toggle
{
    [SerializeField] private float heal = 0;
    [SerializeField] private float damage = 1.25f;

    private float onUpgradeValue = 0.25f;

    public override void OnLearn()
    {
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        damage += onUpgradeValue;
    }

    public override void TurnOn()
    {
        Debug.Log("Turning on");
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;
        PassiveCrocodileSO passive = ((PassiveCrocodileSO)pi.Passive);

        heal = passive.heal;
        passive.heal = 0;
        passive.damageTaken *= damage;
        pi.SkillDamage *= damage;
    }

    public override void TurnOff()
    {
        Debug.Log("Turning off");
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;
        PassiveCrocodileSO passive = ((PassiveCrocodileSO)pi.Passive);

        passive.heal = heal;
        passive.damageTaken /= damage;
        pi.SkillDamage /= damage;
    }

    public override bool OnUse()
    {        
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;
        PassiveCrocodileSO passive = ((PassiveCrocodileSO)pi.Passive);

        if (!Active && pi.Stamina < SpCost || pi.Mana < MpCost)
            return false;

        Debug.Log($"Toggle => {Active} = {!Active}");
        Active = !Active;
     
        if (Active){
            TurnOn();
            return true;
        }

        TurnOff();

        pi.Stamina -= SpCost;
        pi.Mana -= MpCost;
        return true;
    }
}
