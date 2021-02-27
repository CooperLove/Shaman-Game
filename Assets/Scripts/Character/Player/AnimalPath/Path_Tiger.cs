using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Path_Tiger : AnimalPath
{
    [SerializeField] private bool passiveAplied;

    public override void OnLevelUp()
    {
        PlayerInfo playerInfo = Player.Instance.playerInfo;
        playerInfo.Max_HP += 25;
        playerInfo.Max_MP += 15;
        playerInfo.Max_SP += 15;

        playerInfo.HPRegen += 0.45f;
        playerInfo.MPRegen += 0.25f;
        playerInfo.SPRegen += 0.75f;

        playerInfo.StatsPoints += 5;

        playerInfo.MinPhysicalDamage += 10;
        playerInfo.MinMagicDamage    += 2;

        playerInfo.Armor += 5;
        playerInfo.MagicResistance += 3;
        
        playerInfo.Health = playerInfo.Max_HP;
        playerInfo.Mana = playerInfo.Max_MP;
        playerInfo.Stamina = playerInfo.Max_SP;
    }

    public override void Passive(){
        if (passiveAplied)
            return;
        Player.Instance.playerInfo.Max_HP *= 1.10f;
        Player.Instance.playerInfo.Max_SP *= 1.05f;
        passiveAplied = true;
    }
    /*
    public static class MenuEntries {
        [MenuItem("Assets/Create/AnimalPath/Tiger")]
        static void Tiger(){
            var asset = ScriptableObject.CreateInstance<Path_Tiger> ();
            code to preconfigure your asset
            
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);
            path +="/Path_Tiger.asset";
            
            ProjectWindowUtil.CreateAsset (asset, path);
        }
    }
    */
}
