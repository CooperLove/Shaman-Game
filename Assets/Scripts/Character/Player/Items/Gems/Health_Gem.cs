using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Gems/Health Gem")]
public class Health_Gem : Gem
{
    public override Enchantment GetEnchantment() => new Enchantment_Health_Points();
    
    public override bool EnhanceItem(GemHandler gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Collectable))  )))
            return false;
        
        
        var value = Random.Range(1, Enchantment.MAX_PERCENTAGE_HP_MP_SP);

        if (!(Enchantment.GetEnchantmentHp() is Enchantment_Health_Points ench)) 
            return false;
        
        ench.Value = value;

        return true;
    }

    public override bool EnhanceItem(GemRarity gem, ItemHandler handler)
    {
        return false;
    }
    public override string GetDescriptionText()
    {
        return "This a Health Gem. Use this to enchant an item and give it a flat bonus of Health, based on the rarity of this gem.";
    }

    public override string GetEffectsText()
    {
        return "Gives +("+1+"~"+Enchantment.MAX_PERCENTAGE_HP_MP_SP+")";
    }

    
}

