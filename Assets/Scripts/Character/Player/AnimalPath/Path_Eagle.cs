using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Path_Eagle : AnimalPath
{
    [SerializeField] private bool passiveAplied;

    public override void OnLevelUp()
    {
       PlayerInfo playerInfo = Player.Instance.playerInfo;
        playerInfo.Max_HP += 20;
        playerInfo.Max_MP += 05;
        playerInfo.Max_SP += 15;

        playerInfo.HPRegen += 0.45f;
        playerInfo.MPRegen += 0.25f;
        playerInfo.SPRegen += 0.35f;

        playerInfo.StatsPoints += 5;

        playerInfo.MinPhysicalDamage += 15;
        playerInfo.MinMagicDamage    += 4;

        playerInfo.Armor += 5;
        playerInfo.MagicResistance += 3;

        playerInfo.Health = playerInfo.Max_HP;
        playerInfo.Mana = playerInfo.Max_MP;
        playerInfo.Stamina = playerInfo.Max_SP;
    }

    public override void Passive(){
        if (passiveAplied)
            return;
        Player.Instance.playerInfo.CriticalDamage *= 1.20f;
        Player.Instance.playerInfo.AttackSpeed *= 1.15f;
        passiveAplied = true;
    }
}
