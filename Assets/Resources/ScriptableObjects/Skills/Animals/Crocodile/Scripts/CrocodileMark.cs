using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Marks/Crocodile Mark")]
public class CrocodileMark : Active
{
    private const int MAX_CHARGES = 5;
    public int charges = 1;
    public float damageReduction = 0.9f;  // Representa quanto de dano será reduzido [0.4~1], onde 1 é o dano normal => Dano * DamageReduction
    public override void OnLearn()
    {
        CurrentLevel = 1;
    }

    public override void OnUpgrade()
    {
        if (CanUpgradeSkill()){
            damageReduction -= 0.1f;
            charges += 1;
        }
    }

    public override bool OnUse()
    {
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;
        Mark mark = player.GetComponent<Mark>();
        
        if (!mark.canUseMark || MpCost > pi.Mana)
            return false;

        pi.DamageReduction = damageReduction;
        pi.Mana -= MpCost;
        mark.Use();
        return true;
    }
}
