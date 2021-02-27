using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bonus : MonoBehaviour
{
    [SerializeField] PlayerInfo playerInfo, eoq;
    [SerializeField] AnimalPath animal;
    float value;
    private void Awake() {
        playerInfo = Player.Instance.playerInfo;
    }
    public enum AttBonus
    {
        Force, Intelligence, Dexterity, Constitution, Armor, MagicResistance, Max_HP, Max_MP, 
        Max_SP, HPRegen, MPRegen, SPRegen, PhysicalDamage, MagicDamage, CriticalChance, CriticalDamage, AttackSpeed
    }
    public void Apply (Weapon weapon){
        /*
        int numBonus = Random.Range(0,3);
        int bonus;
        AttBonus b;
        Debug.Log(numBonus+" bonus");
        weapon.BonusName = new string[numBonus];
        weapon.BonusValue = new float[numBonus];
        for (int i = 0; i < numBonus; i++)
        {
            bonus = Random.Range(0, 17);
            b = (AttBonus) bonus;
            Debug.Log(bonus+" = "+b);
            this.SendMessage(b.ToString());
            weapon.BonusName[i] = b.ToString();
            weapon.BonusValue[i] = value;
        }
        */
    }

    public void OnEquip (string[] bonusName, float[] bonusValue){
        
    }
    public void OnUnequip (string[] bonusName, float[] bonusValue){

    }

    

    public void Force()           {  value = Random.Range(1,6);          playerInfo.Force           += (int) value; }
    public void Dexterity()       {  value = Random.Range(1,6);          playerInfo.Dexterity       += (int) value; }
    public void Intelligence()    {  value = Random.Range(1,6);          playerInfo.Intelligence    += (int) value; }
    public void Constitution()    {  value = Random.Range(1,6);          playerInfo.Constitution    += (int) value; }
    public void PhysicalDamage()  {  value = Random.Range(1,16);         playerInfo.MinPhysicalDamage  += (int) value; }
    public void MagicDamage()     {  value = Random.Range(1,16);         playerInfo.MinMagicDamage     += (int) value; }
    public void Armor()           {  value = Random.Range(1,10);         playerInfo.Armor           += (int) value; }
    public void MagicResistance() {  value = Random.Range(1,10);         playerInfo.MagicResistance += (int) value; }
    public void CriticalDamage()  {  value = Random.Range(1,15);         playerInfo.CriticalDamage  += value; }
    public void CriticalChance()  {  value = Random.Range(1,6);          playerInfo.CriticalChance  += (int) value; }
    public void Max_HP()          {  value = Random.Range(5,31);         playerInfo.Max_HP          += value; }
    public void Max_MP()          {  value = Random.Range(5,31);         playerInfo.Max_MP          += value; }
    public void Max_SP()          {  value = Random.Range(5,31);         playerInfo.Max_SP          += value; }
    public void HPRegen()         {  value = Random.Range(0.1f, 0.6f);   playerInfo.HPRegen         += value; }
    public void MPRegen()         {  value = Random.Range(0.1f, 0.6f);   playerInfo.MPRegen         += value; }
    public void SPRegen()         {  value = Random.Range(0.1f, 0.6f);   playerInfo.SPRegen         += value; }
    public void AttackSpeed()     {  value = Random.Range(0.1f, 0.3f);   playerInfo.AttackSpeed     += value; }
}
