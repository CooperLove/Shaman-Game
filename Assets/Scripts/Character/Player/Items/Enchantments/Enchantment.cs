using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enchantment
{
    public enum EquipmentEnchantments
    {
        Enchantment_Health_Points, Enchantment_Mana_Points, Enchantment_Stamina_Points,
        Enchantment_Maximum_Health, Enchantment_Maximum_Mana, Enchantment_Maximum_Stamina,
        //Enchantment_Health_Points_Regeneration, EnchantmentMana_Regeneration, EnchantmentStamina_Regeneration, 
        //EnchantmentMove_Speed, EnchantmentPhysical_Damage, EnchantmentMagic_Damage,
        //Enchantment_Critical_Chance, Enchantment_Critical_Damage
    }

    public enum CollectableEnchantments
    {
        Enchantment_More_Chance_To_Enhance, Enchantment_Consumable_Effect
    }

    public const int MAX_FLAT_HP_MP_SP = 250;
    public const int MAX_PERCENTAGE_HP_MP_SP = 5;
    public const int MAX_FLAT_PHYS_MAG_DAMAGE = 125;
    public const int MAX_PERCENTAGE_PHYS_MAG_DAMAGE = 3;
    public const int MAX_PERCENTAGE_HP_MP_SP_Regen = 5;
    public const int MAX_CRIT_CHANCE = 3;
    public const int MAX_CRIT_DAMAGE = 5;
    public const int MAX_ENHANCEMENT = 12;

    private float value = 0;

    public string Name { get; protected set; } = "";
    public float Value { get => value; set { if(this.value != 0) return; this.value = value; }}
    public bool IsFlat { get; protected set; } = true;
    protected float AppliedValue { get; set; } = 0;

    public abstract bool Apply ();
    public abstract bool Unapply ();
    public static Enchantment GetEnchantmentHp() { return new Enchantment_Health_Points();  }
    public static Enchantment GetEnchantmentPercentageHp() { return new Enchantment_Maximum_Health();  }
    public static Enchantment GetEnchantmentMp() { return new Enchantment_Mana_Points();    }
    public static Enchantment GetEnchantmentPercentageMp() { return new Enchantment_Maximum_Mana();  }
    public static Enchantment GetEnchantmentSp() { return new Enchantment_Stamina_Points(); }
    public static Enchantment GetEnchantmentPercentageSp() { return new Enchantment_Maximum_Stamina();  }
    
}

[System.Serializable]
public class Enchantment_Health_Points : Enchantment
{
    public Enchantment_Health_Points()
    {
        Name = "Health Points";
        Value = Random.Range(1, MAX_FLAT_HP_MP_SP);
    }
    
    public override bool Apply ()
    {
        AppliedValue = Value;
        Player.Instance.playerInfo.Bonus_hp += (int) AppliedValue;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_hp -= (int) AppliedValue;
        AppliedValue = 0;
        return true;
    }
}

public class Enchantment_Maximum_Health : Enchantment
{
    public Enchantment_Maximum_Health(){
        IsFlat = false;
    }
    public override bool Apply ()
    {
        AppliedValue = (Player.Instance.playerInfo.Max_HP/100f) * Value;
        Player.Instance.playerInfo.Bonus_hp += (int) AppliedValue;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_hp -= (int) AppliedValue;
        AppliedValue = 0;
        return true;
    }
}

public class Enchantment_Mana_Points : Enchantment
{
    public Enchantment_Mana_Points()
    {
        Name = "Mana Points";
        Value = Random.Range(1, MAX_FLAT_HP_MP_SP);
    }
    
    public override bool Apply()
    {
        Player.Instance.playerInfo.Bonus_mp += (int) Value;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_mp -= (int) Value;
        return true;
    }
}

public class Enchantment_Maximum_Mana : Enchantment
{
    public Enchantment_Maximum_Mana(){
        IsFlat = false;
    }
    public override bool Apply()
    {
        AppliedValue = (Player.Instance.playerInfo.Max_MP/100f) * Value;
        Player.Instance.playerInfo.Bonus_mp += (int) AppliedValue;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_mp -= (int) AppliedValue;
        AppliedValue = 0;
        return true;
    }
}

public class Enchantment_Stamina_Points : Enchantment
{
    public Enchantment_Stamina_Points()
    {
        Name = "Stamina Points";
        Value = Random.Range(1, MAX_FLAT_HP_MP_SP);
    }
    
    public override bool Apply()
    {
        Player.Instance.playerInfo.Bonus_sp += (int) Value;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_sp -= (int) Value;
        return true;
    }
}

public class Enchantment_Maximum_Stamina : Enchantment
{
    public Enchantment_Maximum_Stamina(){
        IsFlat = false;
    }
    public override bool Apply()
    {
        AppliedValue = (Player.Instance.playerInfo.Max_SP/100f) * Value;
        Player.Instance.playerInfo.Bonus_sp += (int) AppliedValue;
        return true;
    }

    public override bool Unapply()
    {
        Player.Instance.playerInfo.Bonus_sp -= (int) AppliedValue;
        AppliedValue = 0;
        return true;
    }
}



