using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Jewel/RightRing")]
public class RightRing : Jewerly
{
    public override string GetAttributesText(EquipmentHandler handler)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        return "<align=center>Level: "+NecessaryLevel+
                "\nPhysical Resistence: "+(int) Armor+( Armor == pi.RightRing?.Armor ? 
                                                    "<color=blue> - </color>" : 
                                                    (Armor > pi.RightRing?.Armor ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    (pi.RightRing == null ? 
                                                     "<color=green><size=30> ↑ </size></color>" : 
                                                     "<color=red><size=30> ↓ </size></color>")))+
               "\nMagic Resistence: "+(int) MagicResistance+( MagicResistance == pi.RightRing?.MagicResistance ? 
                                                    "<color=blue> - </color>" : 
                                                    (MagicResistance > pi.RightRing?.MagicResistance ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    (pi.RightRing == null ? 
                                                     "<color=green><size=30> ↑ </size></color>" : 
                                                     "<color=red><size=30> ↓ </size></color>")))+
                "\nEnhancement: "+Enhancement+
                "\nType: "+Type.ToString()+"</align>";
    }
    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
            
        CurrentItemsHandler.CurrentRightRing?.Unequip();
        CurrentItemsHandler.CurrentRightRing = handler;
        Player.Instance.playerInfo.RightRing = this;


        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.RightRing != null){
            RemoveItemStats((JewerlyHandler) handler);
        }

        pi.RightRing = this;
        AddItemStats((JewerlyHandler) handler);

        CurrentItemsHandler.CurrentRightRing._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentRightRing == null)
            return false;

        
        
        CurrentItemsHandler.CurrentRightRing._UpdateOnEquipText(false);
        RemoveItemStats ((JewerlyHandler) handler);
        CurrentItemsHandler.CurrentRightRing = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.RightRing = null;

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
            
            if (CurrentItemsHandler.CurrentRightRing != null && this.Equals(CurrentItemsHandler.CurrentRightRing.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += Armor - oldArmor;
                pi.Bonus_magicResistance += MagicResistance - oldMr;
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
