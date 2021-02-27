using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Gems/EnchantmentGem")]
public class EnchantmentGem : Gem
{
    private Enchantment GenerateAnEquipEnchantment (){
        var index = UnityEngine.Random.Range(0, 5);
        var typeName = ((Enchantment.EquipmentEnchantments) index).ToString();
        var type = System.Type.GetType(typeName);
        Debug.Log($"index {index} {type}");
        var enchantment = (Enchantment) Activator.CreateInstance(type);

        return enchantment;        
    }
    private Enchantment GenerateACollectableEnchantment (){
        var index = UnityEngine.Random.Range(0, 3);
        var typeName = ((Enchantment.CollectableEnchantments) index).ToString();
        var type = System.Type.GetType(typeName);
        var enchantment = (Enchantment) Activator.CreateInstance(type);
        Debug.Log(type+" "+enchantment);

        return enchantment;
    }
    public override bool EnhanceItem(GemHandler gem, ItemHandler handler)
    {
        if (handler.Item.GetType().IsSubclassOf(typeof(Gem)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Equipment)) || 
           (handler.Item.GetType().IsSubclassOf(typeof(Collectable))  )))
            return false;
        
        Debug.Log($"Adicionando gema em {handler.Item.ItemName} {handler.Item.GetType()}");
        
        Enchantment ench = null;
        if (handler.Item.GetType().IsSubclassOf(typeof(Equipment)))
            ench = GenerateAnEquipEnchantment();
        if (handler.Item.GetType().IsSubclassOf(typeof(Collectable)))
            ench = GenerateACollectableEnchantment();
        
        if (ench != null){
            //handler.Gems.Add(new GemRarity((Gem)gem.Item, handler.Rarity));
            
        }

        return ench != null;
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
        return "This is a Enchantment Gem. Use this gem to give a random enchantment to your equipment.";
    }

    public override string GetEffectsText()
    {
        return "A random enchantment. The more rare this gems is, the more strong is the enchantment.";
    }

    

}
