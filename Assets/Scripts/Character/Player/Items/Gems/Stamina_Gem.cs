using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Gems/Stamina Gem")]
public class Stamina_Gem : Gem
{
    public override Enchantment GetEnchantment() => new Enchantment_Stamina_Points();
    
    public override bool EnhanceItem(GemHandler gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Collectable))  )))
            return false;

        int value = Random.Range(1, Enchantment.MAX_PERCENTAGE_HP_MP_SP);
        Enchantment_Stamina_Points ench = Enchantment.GetEnchantmentSp() as Enchantment_Stamina_Points;
        ench.Value = value;
        return true;
    }

    public override bool EnhanceItem(GemRarity gem, ItemHandler handler)
    {
        return false;
        // if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) )
        //     return false;
        // bool result = false;
        // if (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) && ((Equipment) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
        //     result = ((Equipment) handler.Item).OnEnhancement(new GemHandler(gem), (EquipmentHandler)handler);
        // else if (handler.Item.GetType().IsSubclassOf(typeof(Jewerly)) && ((Jewerly) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
        //     result = ((Jewerly) handler.Item).OnEnhancement(new GemHandler(gem), (EquipmentHandler) handler);
        // return result;
    }

    public override string GetDescriptionText()
    {
        return "This a Stamina Gem. Use this to enchant an item and give it a bonus of Stamina, based on the rarity of this gem.";
    }

    public override string GetEffectsText()
    {
        return "Gives +("+1+"~"+Enchantment.MAX_PERCENTAGE_HP_MP_SP+")";
    }

    
}
