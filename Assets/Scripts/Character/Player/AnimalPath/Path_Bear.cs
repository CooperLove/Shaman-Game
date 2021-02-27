using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Path_Bear : AnimalPath
{
    [SerializeField] private bool passiveAplied;

    public override void OnLevelUp()
    {
        PlayerInfo playerInfo = Player.Instance.playerInfo;
        playerInfo.Max_HP += 15;
        playerInfo.Max_MP += 30;
        playerInfo.Max_SP += 10;

        playerInfo.HPRegen += 0.45f;
        playerInfo.MPRegen += 0.75f;
        playerInfo.SPRegen += 0.35f;

        playerInfo.StatsPoints += 5;

        playerInfo.MinPhysicalDamage += 8;
        playerInfo.MinMagicDamage    += 15;

        playerInfo.Armor += 8;
        playerInfo.MagicResistance += 12;
        
        playerInfo.Health = playerInfo.Max_HP;
        playerInfo.Mana = playerInfo.Max_MP;
        playerInfo.Stamina = playerInfo.Max_SP;
    }

    public override void Passive(){
        if (passiveAplied)
            return;
        Player.Instance.playerInfo.MPRegen *= 1.10f;
        Player.Instance.playerInfo.Max_MP  *= 1.05f;
        passiveAplied = true;
    }

}
