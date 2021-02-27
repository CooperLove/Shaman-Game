using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Gear/Legging")]
public class Legging : Gear
{
    [SerializeField] private float percentageArmor;

    public float PercentageArmor { get => percentageArmor; set => percentageArmor = value; }

    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
        CurrentItemsHandler.CurrentLeggings?.Unequip();
        CurrentItemsHandler.CurrentLeggings = handler;
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Leggings != null){
            RemoveItemStats((GearHandler) handler);
        }

        pi.Leggings = this;
        AddItemStats ((GearHandler) handler);

        CurrentItemsHandler.CurrentLeggings._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentLeggings == null)
            return false;

        
        CurrentItemsHandler.CurrentLeggings._UpdateOnEquipText(false);
        RemoveItemStats ((GearHandler) handler);
        CurrentItemsHandler.CurrentLeggings = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Leggings = null;

        return true;
    }

    public void AddItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;
        
        pi.Bonus_armor += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
        pi.Bonus_armor += ((pi.Armor + pi.Bonus_armor)/100f) * PercentageArmor;
        pi.Bonus_hp += handler.HP;
    }

    private void RemoveItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor -= ((pi.Armor + pi.Bonus_armor)/(100f + pi.Leggings.PercentageArmor)) * pi.Leggings.PercentageArmor;
        pi.Bonus_armor -= handler.Armor;
        pi.Bonus_magicResistance -= handler.MR;
        pi.Bonus_hp -= handler.HP;
    }

    public override bool OnEnhancement(GemHandler gem, EquipmentHandler handler)
    {
        if (((GearHandler) handler).Enhancement >= MAX_ENHANCEMENT)
            return false;

        float value = Random.Range(0f, 100f);
        float perc = percEnhancement[handler.Enhancement] * Gem.percentagePerRarity[(int)gem.Rarity]; 
        perc = Mathf.Clamp(perc, 0, 100);
        if (value <= perc){
            ((GearHandler) handler).Enhancement += 1;
            int oldHP = ((GearHandler) handler).HP;
            float oldArmor = ((GearHandler) handler).Armor, oldMr = ((GearHandler) handler).MR;
            float oldpMr = percentageArmor;

            ((GearHandler) handler).Armor *= 1.1f;
            ((GearHandler) handler).MR *= 1.1f;
            ((GearHandler) handler).HP += 10;
            percentageArmor += 7f/MAX_ENHANCEMENT;
            if (CurrentItemsHandler.CurrentLeggings != null && this.Equals(CurrentItemsHandler.CurrentLeggings.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += ((GearHandler) handler).Armor - oldArmor;
                pi.Bonus_magicResistance += ((GearHandler) handler).MR - oldMr;
                pi.Bonus_hp += ((GearHandler) handler).HP - oldHP;
                pi.Bonus_armor += ((percentageArmor - oldpMr) * ((pi.Bonus_armor + pi.Bonus_armor)/100f));
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
