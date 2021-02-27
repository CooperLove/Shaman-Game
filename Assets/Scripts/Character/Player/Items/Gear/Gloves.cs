using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Gear/Gloves")]
public class Gloves : Gear
{
    [SerializeField] private float cooldownReduction;

    public float CooldownReduction { get => cooldownReduction; set => cooldownReduction = value; }

    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
        CurrentItemsHandler.CurrentGloves?.Unequip();
        CurrentItemsHandler.CurrentGloves = handler;
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Gloves != null){
            RemoveItemStats((GearHandler) handler);
        }

        pi.Gloves = this;
        AddItemStats ((GearHandler) handler);

        CurrentItemsHandler.CurrentGloves._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentGloves == null)
            return false;

        
        CurrentItemsHandler.CurrentGloves._UpdateOnEquipText(false);
        RemoveItemStats ((GearHandler) handler);
        CurrentItemsHandler.CurrentGloves = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Gloves = null;

        return true;
    }

    public void AddItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
        //pi.Bonus_armor += (pi.cooldownReduction/100) * cooldownReduction;
        pi.Bonus_hp += handler.HP;
    }

    private void RemoveItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        //pi.Bonus_armor -= (pi.Armor/(100 + pi.Gloves.cooldownReduction)) * pi.Gloves.cooldownReduction;
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
            float oldpMr = CooldownReduction;

            ((GearHandler) handler).Armor *= 1.1f;
            ((GearHandler) handler).MR *= 1.1f;
            ((GearHandler) handler).HP += 10;
            CooldownReduction += 7f/MAX_ENHANCEMENT;
            
            if (CurrentItemsHandler.CurrentGloves != null && this.Equals(CurrentItemsHandler.CurrentGloves.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += Armor - oldArmor;
                pi.Bonus_magicResistance += MagicResistance - oldMr;
                pi.Bonus_hp += Health - oldHP;
            }
            //pi.Bonus_armor += ((CooldownReduction - oldpMr) * pi.Armor);
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
