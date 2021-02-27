using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Gems/Mana Gem")]
public class Mana_Gem : Gem
{
    
    public override Enchantment GetEnchantment() => new Enchantment_Mana_Points();
    public override bool EnhanceItem(GemHandler gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Collectable))  )))
            return false;

        int value = Random.Range(1, Enchantment.MAX_PERCENTAGE_HP_MP_SP);
        Enchantment_Mana_Points ench = Enchantment.GetEnchantmentMp() as Enchantment_Mana_Points;
        ench.Value = value;
        
        return true;
    }

    public override bool EnhanceItem(GemRarity gem, ItemHandler handler)
    {
        return false;
    }

    public override string GetDescriptionText()
    {
        return "This a Mana Gem. Use this to enchant an item and give it a bonus of Mana, based on the rarity of this gem.";
    }

    public override string GetEffectsText()
    {
        return "Gives +("+1+"~"+Enchantment.MAX_PERCENTAGE_HP_MP_SP+")";
    }

    
}




