using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using TMPro;

public class CharacterMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text text, value;
    private PlayerInfo playerInfo;

    private void OnEnable() {
        GetVariable(this.name);
    }
    void Awake() {
        playerInfo = Player.Instance.playerInfo;
        text = GetComponent<TMP_Text>();
        value = transform.parent.GetChild(1).GetComponent<TMP_Text>();
        text.text = this.name;
        string stat = this.name;
        stat = stat.Replace(" ", "");
        GetVariable(stat);
        //Debug.Log(stat);
        
        //value.text = (string) mi.Invoke(p,null)+"";
    }

    public void GetVariable (string name){
        this.SendMessage(name);
    }

    public string Force() { return value.text = (playerInfo.Force + playerInfo.Bonus_force).ToString(); }
    public string Dexterity() { return value.text = (playerInfo.Dexterity + playerInfo.Bonus_dexterity).ToString(); }
    public string Intelligence() { return value.text = (playerInfo.Intelligence + playerInfo.Bonus_intelligence).ToString(); }
    public string Constitution() { return value.text = (playerInfo.Constitution + playerInfo.Bonus_constitution).ToString(); }
    public string Vigor() { return value.text = (playerInfo.Vigor + playerInfo.Bonus_vigor).ToString(); }
    public string AvailablePoints() { return value.text = playerInfo.StatsPoints.ToString(); }
    public string Max_HP() { return value.text = (playerInfo.Max_HP + playerInfo.Bonus_hp).ToString(); }
    public string Max_MP() { return value.text = (playerInfo.Max_MP + playerInfo.Bonus_mp).ToString(); }
    public string Max_SP() { return value.text = (playerInfo.Max_SP + playerInfo.Bonus_sp).ToString(); }
    public string HP() { return value.text = playerInfo.Health.ToString(); }
    public string MP() { return value.text = playerInfo.Mana.ToString(); }
    public string SP() { return value.text = playerInfo.Stamina.ToString(); }
    public string HPRegen() { return value.text = Math.Round(playerInfo.HPRegen, 3).ToString(); }
    public string MPRegen() { return value.text = Math.Round(playerInfo.MPRegen, 3).ToString(); }
    public string SPRegen() { return value.text = Math.Round(playerInfo.SPRegen, 3).ToString(); }
    public string CriticalChance() { return value.text = playerInfo.CriticalChance.ToString(); }
    public string CriticalDamage() { return value.text = Math.Round(playerInfo.CriticalDamage, 3).ToString(); }
    public string AttackSpeed() { return value.text = Math.Round(playerInfo.AttackSpeed, 3).ToString(); }
    public string PhysicalDamage() { return value.text = (playerInfo.MinPhysicalDamage + playerInfo.Bonus_physicalDamage).ToString(); }
    public string MagicDamage() { return value.text = (playerInfo.MinMagicDamage + playerInfo.Bonus_magicDamage).ToString(); }
    public string Armor() { return value.text = (playerInfo.Armor + playerInfo.Bonus_armor).ToString(); }
    public string MagicResistance() { return value.text = (playerInfo.MagicResistance + playerInfo.Bonus_magicResistance).ToString(); }
    public string Gold() { return value.text = playerInfo.Gold.ToString(); }
    public string Level() { return value.text = playerInfo.Level.ToString(); }
    public string Experience() { return value.text = playerInfo.Experience.ToString(); }
    public string NextLevelEXP() { return value.text = playerInfo.NextLevelEXP.ToString(); }
    public string Path() { return value.text = playerInfo.AnimalPath.path.ToString(); }
    public string MoveSpeed() { return value.text = playerInfo.MoveSpeed.ToString(); }
    public string ArmorPen() { return value.text = playerInfo.ArmorPenetration.ToString(); }
    public string MagicPen() { return value.text = playerInfo.MagicPenetration.ToString(); }
    public string SkillDMG() { return value.text = playerInfo.SkillDamage.ToString(); }
    public string CDR() { return value.text = playerInfo.Cdr.ToString(); }

}
