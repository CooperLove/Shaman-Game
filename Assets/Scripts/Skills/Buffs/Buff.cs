using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buff
{
    public static IEnumerator AntiStun (Character character, float duration){
        yield return new WaitForSeconds(duration);
    }
    /*Aplica redução de velocidade de movimento em percentage% por duration segundos*/
    public static IEnumerator MsBuff (Character character, float percentage, float duration){
        float vel = character.Velocity;
        character.Velocity *= percentage;
        yield return new WaitForSeconds(duration);
        character.Velocity = vel;
    }
    /*Enraiza o personagem por duration segundos*/
    public static IEnumerator ArmorBuffPercentage (CharacterInfo character, float duration, float percentage){
        float armor = character.Armor;
        character.Armor = (int)(character.Armor * percentage);
        yield return new WaitForSeconds(duration);
        character.Armor = armor;
    }
    public static IEnumerator ArmorBuffFlat (CharacterInfo character, float duration, int value){
        float armor = character.Armor;
        character.Armor += value;
        yield return new WaitForSeconds(duration);
        character.Armor = armor;
        
    }
    public static IEnumerator MrBuffPercentage (CharacterInfo character, float duration, float percentage){
        float mr = character.MagicResistance;
        character.MagicResistance = (int)(character.MagicResistance * percentage);
        yield return new WaitForSeconds(duration);
        character.MagicResistance = mr;  
    }
    public static IEnumerator MrBuffFlat (CharacterInfo character, float duration, int value){
        float mr = character.MagicResistance;
        character.MagicResistance += value;
        yield return new WaitForSeconds(duration);
        character.MagicResistance = mr;  
        
    }
    public static IEnumerator AdBuffPercentage (CharacterInfo character, float duration, float percentage){
        int p = character.MinPhysicalDamage;
        character.MinPhysicalDamage = (int)(character.MinPhysicalDamage * percentage);
        yield return new WaitForSeconds(duration);
        character.MinPhysicalDamage = p;
    }
    public static IEnumerator AdBuffFlat (CharacterInfo character, float duration, int value){
        int p = character.MinPhysicalDamage;
        character.MinPhysicalDamage += value;
        yield return new WaitForSeconds(duration);
        character.MinPhysicalDamage = p;
    }
    public static IEnumerator ApBuffPercentage (CharacterInfo character, float duration, float percentage){
        int p = character.MinMagicDamage;
        character.MinMagicDamage = (int)(character.MinMagicDamage * percentage);
        yield return new WaitForSeconds(duration);
        character.MinMagicDamage = p;
        
    }
    public static IEnumerator ApBuffFlat (CharacterInfo character, float duration, int value){
        int p = character.MinMagicDamage;
        character.MinMagicDamage += value;
        yield return new WaitForSeconds(duration);
        character.MinMagicDamage = p;
        
    }
    public static IEnumerator HpBuffPercentage (CharacterInfo character, float duration, float percentage){
        float p = character.Health;
        character.Health *= percentage;
        yield return new WaitForSeconds(duration);
        character.Health = p;
    }
    public static IEnumerator HpBuffFlat (CharacterInfo character, float duration, int value){
        float p = character.Health;
        character.Health += value;
        yield return new WaitForSeconds(duration);
        character.Health = p;
    }
    public static IEnumerator MpBuffPercentage (CharacterInfo character, float duration, float percentage){
        float p = character.Mana;
        character.Mana *= percentage;
        yield return new WaitForSeconds(duration);
        character.Mana = p;
    }
    public static IEnumerator MpBuffFlat (CharacterInfo character, float duration, int value){
        float p = character.Mana;
        character.Mana += value;
        yield return new WaitForSeconds(duration);
        character.Mana = p;
    }
    public static IEnumerator SpBuffPercentage (CharacterInfo character, float duration, float percentage){
        float p = character.Stamina;
        character.Stamina *= percentage;
        yield return new WaitForSeconds(duration);
        character.Stamina = p;
    }
    public static IEnumerator SpBuffFlat (CharacterInfo character, float duration, int value){
        float p = character.Stamina;
        character.Stamina += value;
        yield return new WaitForSeconds(duration);
        character.Stamina = p;
    }
    public static IEnumerator HOT (CharacterInfo character, float duration, int amount){
        float tick = amount/duration;
        float dur = 0.0f;
        while (dur < duration)
        {
            dur += Time.deltaTime;
            character.Health += tick;
            yield return null;
        }
    }
    public static IEnumerator CritChanceBuff (CharacterInfo character, float duration, float value){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static void CritChanceBuffPermanent (CharacterInfo character, float duration, float value){
        
    }
    public static IEnumerator CritDamageBuff (CharacterInfo character, float duration, float value){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static void CritDamageBuffPermanent (CharacterInfo character, float duration, float value){
        
    }
}
