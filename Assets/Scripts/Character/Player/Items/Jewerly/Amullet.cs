using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Jewel/Amullet")]
public class Amullet : Jewerly
{
    public override string GetAttributesText(EquipmentHandler handler)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        return "<align=center>Level: "+NecessaryLevel+
                "\nPhysical Resistence: "+(int) Armor+( Armor == pi.Amullet?.Armor ? 
                                                    "<color=blue> - </color>" : 
                                                    (Armor > pi.Amullet?.Armor ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    (pi.Amullet == null ? 
                                                     "<color=green><size=30> ↑ </size></color>" : 
                                                     "<color=red><size=30> ↓ </size></color>")))+
               "\nMagic Resistence: "+(int) MagicResistance+( MagicResistance == pi.Amullet?.MagicResistance ? 
                                                    "<color=blue> - </color>" : 
                                                    (MagicResistance > pi.Amullet?.MagicResistance ? 
                                                    "<color=green><size=30> ↑ </size></color>" : 
                                                    (pi.Amullet == null ? 
                                                     "<color=green><size=30> ↑ </size></color>" : 
                                                     "<color=red><size=30> ↓ </size></color>")))+
                "\nEnhancement: "+Enhancement+
                "\nType: "+Type.ToString()+"</align>";
    }
    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;

        CurrentItemsHandler.CurrentAmullet?.Unequip();
        CurrentItemsHandler.CurrentAmullet = handler;
        Player.Instance.playerInfo.Amullet = this;

        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Amullet != null){
            RemoveItemStats ((JewerlyHandler) handler);
        }

        pi.Amullet = this;
        AddItemStats((JewerlyHandler) handler);

        CurrentItemsHandler.CurrentAmullet._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentAmullet == null)
            return false;

        
        CurrentItemsHandler.CurrentAmullet._UpdateOnEquipText(false);
        RemoveItemStats ((JewerlyHandler) handler);
        CurrentItemsHandler.CurrentAmullet = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Amullet = null;

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

            if (CurrentItemsHandler.CurrentAmullet != null && this.Equals(CurrentItemsHandler.CurrentAmullet.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += Armor - oldArmor;
                pi.Bonus_magicResistance += MagicResistance - oldMr;
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
