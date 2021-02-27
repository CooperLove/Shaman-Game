
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gem : Collectable
{
    
                                                        //{100f, 60f, 40f, 30f, 20f, 15f, 10f, 5f, 2.5f, 1f, 0.5f, 0.1f};
    public static readonly float[] percentagePerRarity = {0.5f , 1.0f, 1.5f, 2.5f, 5.0f, 7.0f, 10.0f};
    
    public abstract bool EnhanceItem (GemHandler gem, ItemHandler handler);
    
    public abstract bool EnhanceItem (GemRarity gem, ItemHandler handler);

    public virtual Enchantment GetEnchantment()
    {
        return null;
    }
    
    public override Type MyHandler() {
        return typeof(GemHandler);
    }

    public override Type MyType (){
        return typeof(Gem);
    }
    
    public override void NewItemIndicator (bool active){
        InventoryTabManager.Instance.gemTab.ExclamationMark.SetActive(active);
    }
    
    public void DestroyGem (GemHandler handler){
        Inventory.Instance.Items.Remove(handler.Item);
        Inventory.Instance.Handlers.Remove(handler);

        Destroy(handler.gameObject);
    }
    
    public override string GetEnchantmentsText(List<GemRarity> gems)
    {
        return "Gems cannot be enchanted.";
    }
}

