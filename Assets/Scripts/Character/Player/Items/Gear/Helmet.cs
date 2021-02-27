using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Gear/Helmet")]
public class Helmet : Gear
{

    
    public override Type MyHandler(){
        return typeof(HelmetHandler);
    }
    
    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;

        CurrentItemsHandler.CurrentHelmet?.Unequip();
        CurrentItemsHandler.CurrentHelmet = (HelmetHandler) handler;
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Helmet != null){
            RemoveCurrentItemStats ((HelmetHandler) handler);
        }

        pi.Helmet = this;
        AddItemStats ((HelmetHandler) handler);

        CurrentItemsHandler.CurrentHelmet._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentHelmet == null)
            return false;

        

        Helmet helmet = (Helmet) CurrentItemsHandler.CurrentHelmet.Item;
        
        CurrentItemsHandler.CurrentHelmet._UpdateOnEquipText(false);
        RemoveCurrentItemStats ((HelmetHandler) handler);
        CurrentItemsHandler.CurrentHelmet = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Helmet = null;

        return true;
    }

    private void AddItemStats (HelmetHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Bonus_armor += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
        
        pi.Bonus_magicResistance += ((pi.MagicResistance + pi.Bonus_magicResistance)/100f) * ((HelmetHandler)handler).PercentageMagicResistance;
        pi.Bonus_hp += handler.HP;
    }

    private void RemoveCurrentItemStats (HelmetHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Bonus_armor -= handler.Armor;
        pi.Bonus_magicResistance -= ((pi.MagicResistance + pi.Bonus_magicResistance)/(100f + 
                                                        CurrentItemsHandler.CurrentHelmet.PercentageMagicResistance)) * 
                                                        CurrentItemsHandler.CurrentHelmet.PercentageMagicResistance;
        pi.Bonus_magicResistance -= handler.MR;
        pi.Bonus_hp -= handler.HP;
    }

    public override bool OnEnhancement(GemHandler gem, EquipmentHandler handler)
    {
        if (((HelmetHandler) handler).Enhancement >= MAX_ENHANCEMENT)
            return false;

        float value = UnityEngine.Random.Range(0f, 100f);
        float perc = percEnhancement[handler.Enhancement] * Gem.percentagePerRarity[(int)gem.Rarity]; 
        perc = Mathf.Clamp(perc, 0, 100);
        if (value <= perc){
            ((HelmetHandler) handler).Enhancement += 1;
            int oldHP = ((HelmetHandler) handler).HP;
            float oldArmor = ((HelmetHandler) handler).Armor, oldMr = ((HelmetHandler) handler).MR;
            float oldpMr = ((HelmetHandler)handler).PercentageMagicResistance;

            ((HelmetHandler) handler).Armor *= 1.1f;
            ((HelmetHandler) handler).MR *= 1.1f;
            ((HelmetHandler) handler).HP += 10;
            ((HelmetHandler)handler).PercentageMagicResistance += 7f/MAX_ENHANCEMENT;
            //Debug.Log((Armor - oldArmor)+" ----- "+(int)(Armor - oldArmor)+" ----- "+((HelmetHandler)handler).PercentageMagicResistance+" += "+7/MAX_ENHANCEMENT);
            if (CurrentItemsHandler.CurrentHelmet != null && this.Equals(CurrentItemsHandler.CurrentHelmet.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += ((HelmetHandler) handler).Armor - oldArmor;
                pi.Bonus_magicResistance += ((HelmetHandler) handler).MR - oldMr;
                pi.Bonus_hp += ((HelmetHandler) handler).HP - oldHP;
                pi.Bonus_magicResistance += ((((HelmetHandler)handler).PercentageMagicResistance - oldpMr) * ((pi.MagicResistance + pi.Bonus_magicResistance)/100f));
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
