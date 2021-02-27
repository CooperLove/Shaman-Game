using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Gems/EnhancementGem")]
public class EnhancementGem : Gem
{
    public override bool EnhanceItem(GemHandler gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) )
            return false;
        bool result = false;
        if (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) && ((Equipment) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
            result = ((Equipment) handler.Item).OnEnhancement(gem, (EquipmentHandler)handler);
        else if (handler.Item.GetType().IsSubclassOf(typeof(Jewerly)) && ((Jewerly) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
            result = ((Jewerly) handler.Item).OnEnhancement(gem, (EquipmentHandler) handler);
        return result;
    }

    public override bool EnhanceItem(GemRarity gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) )
            return false;
        bool result = false;
        if (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) && ((Equipment) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
            result = ((Equipment) handler.Item).OnEnhancement(new GemHandler(gem), (EquipmentHandler)handler);
        else if (handler.Item.GetType().IsSubclassOf(typeof(Jewerly)) && ((Jewerly) handler.Item).Enhancement < Enchantment.MAX_ENHANCEMENT)
            result = ((Jewerly) handler.Item).OnEnhancement(new GemHandler(gem), (EquipmentHandler) handler);
        return result;
    }

    public override string GetDescriptionText()
    {
        return "";
    }

    public override string GetEffectsText()
    {
        return "";
    }

    

    
}
