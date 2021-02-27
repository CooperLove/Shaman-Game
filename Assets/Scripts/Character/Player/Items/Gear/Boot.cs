using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Gear/Boot")]
public class Boot : Gear
{
    [SerializeField] private float moveSpeed;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public override void OnChangeEquipment(EquipmentHandler handler)
    {
        if (handler == null)
            return;
        //CurrentItemsHandler.CurrentBoots?._UpdateOnEquipText(false);
        CurrentItemsHandler.CurrentBoots?.Unequip();
        
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Boots != null){
            RemoveItemStats((GearHandler) handler);
        }

        CurrentItemsHandler.CurrentBoots = handler;
        pi.Boots = this;
        AddItemStats ((GearHandler) handler);

        CurrentItemsHandler.CurrentBoots._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentBoots == null)
            return false;
        
        
        CurrentItemsHandler.CurrentBoots._UpdateOnEquipText(false);
        RemoveItemStats ((GearHandler) handler);
        CurrentItemsHandler.CurrentBoots = null;
        PlayerInfo pi = Player.Instance.playerInfo;
        pi.Boots = null;

        return true;
    }

    public void AddItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_armor += handler.Armor;
        pi.Bonus_magicResistance += handler.MR;
        //Debug.Log(pi.MoveSpeed+" "+pi.Bonus_moveSpeed+" "+MoveSpeed+" "+((pi.MoveSpeed + pi.Bonus_moveSpeed)/100f) * MoveSpeed);
        pi.Bonus_moveSpeed += ((pi.MoveSpeed + pi.Bonus_moveSpeed)/100f) * MoveSpeed;
        pi.Bonus_hp += handler.HP;
    }

    private void RemoveItemStats (GearHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_moveSpeed -= ((pi.MoveSpeed + pi.Bonus_moveSpeed)/(100f + pi.Boots.MoveSpeed)) * pi.Boots.MoveSpeed;
        pi.Bonus_armor -= handler.Armor;
        pi.Bonus_magicResistance -= handler.MR;
        pi.Bonus_hp -= handler.HP;
    }

    public override string GetAttributesText(EquipmentHandler handler)
    {
        var pi = Player.Instance.playerInfo;
        var isNull = ((GearHandler)CurrentItemsHandler.CurrentBoots) == null;
        var deff = !isNull ? ((GearHandler)CurrentItemsHandler.CurrentBoots).Armor : 0;
        var defm = !isNull ? ((GearHandler)CurrentItemsHandler.CurrentBoots).MR : 0;
        var hp = !isNull ? ((GearHandler)CurrentItemsHandler.CurrentBoots).HP : 0;
        return "";
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
            float oldpMr = MoveSpeed;

            ((GearHandler) handler).Armor *= 1.1f;
            ((GearHandler) handler).MR *= 1.1f;
            ((GearHandler) handler).HP += 10;
            MoveSpeed += 7f/MAX_ENHANCEMENT;
            Debug.Log((Armor - oldArmor)+" ----- "+(int)(Armor - oldArmor)+" ----- "+MoveSpeed+" += "+7/MAX_ENHANCEMENT);
            if (CurrentItemsHandler.CurrentBoots != null && this.Equals(CurrentItemsHandler.CurrentBoots.Item)){
                PlayerInfo pi = Player.Instance.playerInfo;
                pi.Bonus_armor += Armor - oldArmor;
                pi.Bonus_magicResistance += MagicResistance - oldMr;
                pi.Bonus_hp += Health - oldHP;
                pi.Bonus_moveSpeed += ((MoveSpeed - oldpMr) * (pi.MoveSpeed/100f));
            }
        }
        ((ItemHandler)handler).ChangeText();
        return value <= perc;
    }

    
}
