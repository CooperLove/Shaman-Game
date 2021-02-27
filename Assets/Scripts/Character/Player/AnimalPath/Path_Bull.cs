using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Path_Bull : AnimalPath
{
    [SerializeField] private bool passiveAplied;

    public override void OnLevelUp()
    {
        PlayerInfo playerInfo = Player.Instance.playerInfo;
        playerInfo.Max_HP += 25;
        playerInfo.Max_MP += 15;
        playerInfo.Max_SP += 35;

        playerInfo.HPRegen += 0.45f;
        playerInfo.MPRegen += 0.25f;
        playerInfo.SPRegen += 0.35f;

        playerInfo.StatsPoints += 5;

        playerInfo.MinPhysicalDamage += 10;
        playerInfo.MinMagicDamage    += 2;

        playerInfo.Armor += 8;
        playerInfo.MagicResistance += 5;

        playerInfo.Health = playerInfo.Max_HP;
        playerInfo.Mana = playerInfo.Max_MP;
        playerInfo.Stamina = playerInfo.Max_SP;
    }

    public override void Passive(){
        if (passiveAplied)
            return;
        Player.Instance.playerInfo.HPRegen *= 1.15f;
        Player.Instance.playerInfo.MagicResistance += 25;
        passiveAplied = true;
    }

}
