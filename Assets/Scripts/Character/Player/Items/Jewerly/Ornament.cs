using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Jewel/Ornament")]
public class Ornament : Jewerly
{
    public override string GetAttributesText(EquipmentHandler handler)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        return "<align=center>Level: "+NecessaryLevel+
                "\nPhysical Resistence: "+(int) Armor+( Armor == pi.Ornament?.Armor ? 
                                                    "<color=blue> - </color>" : 
                                                    (Armor > pi.Ornament?.Armor ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    "<color=red><size=30> ↓ </size></color>"))+
               "\nMagic Resistence: "+(int) MagicResistance+( MagicResistance == pi.Ornament?.MagicResistance ? 
                                                    "<color=blue> - </color>" : 
                                                    (MagicResistance > pi.Ornament?.MagicResistance ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    "<color=red><size=30> ↓ </size></color>"))+
                "\nEnhancement: "+Enhancement+
                "\nType: "+Type.ToString()+"</align>";
    }
    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
            
        CurrentItemsHandler.CurrentOrnament?.Unequip();
        CurrentItemsHandler.CurrentOrnament = handler;
        Player.Instance.playerInfo.Ornament = this;

        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Ornament != null){
            RemoveItemStats((JewerlyHandler) handler);
        }

        pi.Ornament = this;
        AddItemStats((JewerlyHandler) handler);

        CurrentItemsHandler.CurrentOrnament._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentOrnament == null)
            return false;

        
        
        CurrentItemsHandler.CurrentOrnament._UpdateOnEquipText(false);
        RemoveItemStats ((JewerlyHandler) handler);
        CurrentItemsHandler.CurrentOrnament = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Ornament = null;

        return true;
    }

    public void AddItemStats (JewerlyHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor           += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
    }

    private void RemoveItemStats (JewerlyHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor           -= handler.Armor;
        pi.Bonus_magicResistance -= handler.MR;
    }

    public override bool OnEnhancement(GemHandler gem, EquipmentHandler handler)
    {
        if (Enhancement >= MAX_ENHANCEMENT)
            return false;

        float value = Random.Range(0f, 100f);
        float perc = percEnhancement[handler.Enhancement] * Gem.percentagePerRarity[(int)gem.Rarity]; 
        perc = Mathf.Clamp(perc, 0, 100);
        if (value <= perc){
            handler.Enhancement += 1;
            float oldArmor = Armor, oldMr = MagicResistance;

            Armor = Armor * 1.1f;
            MagicResistance = MagicResistance * 1.1f;
            
            if (CurrentItemsHandler.CurrentOrnament != null && this.Equals(CurrentItemsHandler.CurrentOrnament.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += Armor - oldArmor;
                pi.Bonus_magicResistance += MagicResistance - oldMr;
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
