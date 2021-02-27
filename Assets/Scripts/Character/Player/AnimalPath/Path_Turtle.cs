using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Path_Turtle : AnimalPath
{
    [SerializeField] private bool passiveAplied;

    public override void OnLevelUp()
    {
        PlayerInfo playerInfo = Player.Instance.playerInfo;
        playerInfo.Max_HP += 35;
        playerInfo.Max_MP += 15;
        playerInfo.Max_SP += 20;

        playerInfo.HPRegen += 0.75f;
        playerInfo.MPRegen += 0.5f;
        playerInfo.SPRegen += 0.75f;

        playerInfo.StatsPoints += 5;

        playerInfo.MinPhysicalDamage += 6;
        playerInfo.MinMagicDamage    += 12;

        playerInfo.Armor += 12;
        playerInfo.MagicResistance += 12;
        
        playerInfo.Health = playerInfo.Max_HP;
        playerInfo.Mana = playerInfo.Max_MP;
        playerInfo.Stamina = playerInfo.Max_SP;
    }

    public override void Passive(){
        if (passiveAplied)
            return;
        Player.Instance.playerInfo.Max_HP *= 1.25f;
        Player.Instance.playerInfo.Armor += 10;
        passiveAplied = true;
    }
    
}
