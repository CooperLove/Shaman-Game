using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Gear/Chest")]
public class ChestArmor : Gear
{
    [SerializeField] private float skillDamage;

    public float SkillDamage { get => skillDamage; set => skillDamage = value; }

    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
        CurrentItemsHandler.CurrentChestArmor?.Unequip();
        CurrentItemsHandler.CurrentChestArmor = handler;
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.ChestArmor != null){
            RemoveItemStats((GearHandler) handler);
        }

        pi.ChestArmor = this;
        AddItemStats ((GearHandler) handler);

        CurrentItemsHandler.CurrentChestArmor._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentChestArmor == null)
            return false;

        
        CurrentItemsHandler.CurrentChestArmor._UpdateOnEquipText(false);
        RemoveItemStats ((GearHandler) handler);
        CurrentItemsHandler.CurrentChestArmor = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.ChestArmor = null;

        return true;
    }

    public void AddItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
        pi.Bonus_skillDamage += skillDamage;
        pi.Bonus_hp += handler.HP;
    }

    private void RemoveItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor -= handler.Armor;
        pi.Bonus_magicResistance -= handler.MR;
        pi.Bonus_skillDamage -= pi.ChestArmor.SkillDamage;
        pi.Bonus_hp -= handler.HP;
    }

    public override bool OnEnhancement(GemHandler gem, EquipmentHandler handler)
    {
        if (((GearHandler) handler).Enhancement >= MAX_ENHANCEMENT || !(gem.Item is EnhancementGem))
            return false;

        float value = Random.Range(0f, 100f);
        float perc = percEnhancement[handler.Enhancement] * Gem.percentagePerRarity[(int)gem.Rarity]; 
        perc = Mathf.Clamp(perc, 0, 100);
        if (value <= perc){
            ((GearHandler) handler).Enhancement += 1;
            int oldHP = ((GearHandler) handler).HP;
            float oldArmor = ((GearHandler) handler).Armor, oldMr = ((GearHandler) handler).MR;
            float oldpMr = skillDamage;

            ((GearHandler) handler).Armor *= 1.1f;
            ((GearHandler) handler).MR *= 1.1f;
            ((GearHandler) handler).HP += 10;
            skillDamage += 7f/MAX_ENHANCEMENT;
            if (CurrentItemsHandler.CurrentChestArmor != null && this.Equals(CurrentItemsHandler.CurrentChestArmor.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += ((GearHandler) handler).Armor - oldArmor;
                pi.Bonus_magicResistance += ((GearHandler) handler).MR - oldMr;
                pi.Bonus_hp += ((GearHandler) handler).HP - oldHP;
                pi.Bonus_skillDamage += ((skillDamage - oldpMr) * (pi.SkillDamage/100f));
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
