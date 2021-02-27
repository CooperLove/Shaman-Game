using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapon")]
public abstract class Weapon : Equipment
{
    public new enum WeaponType
    {
        Claw, ShieldAndMace, ScepterAndBook, BowAndArrow, Staff
    }

    public enum Animal
    {
        Crocodile, Eagle, Bull, Tiger, Crow
    }

    [Header("Level to use")] 
    [SerializeField] private int necessaryLevel = 0;
    
    [Header("Damage")]
    [SerializeField] private int minPhysicalDamage = 0;
    [SerializeField] private int maxPhysicalDamage = 0;
    [SerializeField] private int minMagicDamage = 0;
    [SerializeField] private int maxMagicDamage = 0;
    [SerializeField] private int armorPenetration = 0;
    [SerializeField] private int magicPenetration = 0;

    [Header("Weapon type")]
    [SerializeField] private WeaponType _type;
    
    public int FDamage { get => minPhysicalDamage; set => minPhysicalDamage = value; }
    public int MDamage { get => minMagicDamage; set => minMagicDamage = value; }
    public WeaponType EWeaponType { get => _type; set => _type = value; }
    public int ArmorPenetration { get => armorPenetration; set => armorPenetration = value; }
    public int MagicPenetration { get => magicPenetration; set => magicPenetration = value; }

    public override bool OnEnhancement(GemHandler gem, EquipmentHandler handler) {
        return false;
    }
    public override string GetDescriptionText()
    {
        return "Weapon Desc";
    }
    public override string GetAttributesText(EquipmentHandler handler)
    {
        PlayerInfo pi = Player.Instance.playerInfo;
        return $"PHY DMG: <color=#E69913>{minPhysicalDamage} - {maxPhysicalDamage}</color>" +
               $"\t\t\t\t LVL: <color=#E69913>{necessaryLevel}</color>" +
               $"MAG DMG: <color=#E69913>{minMagicDamage} - {maxMagicDamage}</color>" +
               $"\t\t\t\t ENHANC: +<color=#E69913>+{Enhancement}</color>";
    }

    public override void OnChangeEquipment(EquipmentHandler handler){
        Debug.Log("Changed weapon");
        if (handler == null)
            return;
        CurrentItemsHandler.CurrentWeapon?.Unequip();
        CurrentItemsHandler.CurrentWeapon = handler;
        Player.Instance.playerInfo.Weapon = this;

        PlayerInfo pi = Player.Instance.playerInfo;
        if (CurrentItemsHandler.CurrentWeapon != null){
            RemoveItemStats((WeaponHandler) handler);
        }

        CurrentItemsHandler.CurrentWeapon = handler;
        AddItemStats((WeaponHandler) handler);

        CurrentItemsHandler.CurrentWeapon._UpdateOnEquipText(true);
        
    }

    public override bool OnRemoveEquipment(EquipmentHandler handler)
    {
        if (CurrentItemsHandler.CurrentWeapon == null)
            return false;

        
        
        CurrentItemsHandler.CurrentWeapon._UpdateOnEquipText(false);
        RemoveItemStats ((WeaponHandler) handler);
        CurrentItemsHandler.CurrentWeapon = null;

        return true;
    }

    public void AddItemStats (WeaponHandler handler){ 
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_physicalDamage += handler.FDamage;
        pi.Bonus_magicDamage    += handler.MDamage;
        pi.ArmorPenetration     += handler.ArmorPenetration;
        pi.MagicPenetration     += handler.MagicPenetration;
    }

    private void RemoveItemStats (WeaponHandler handler){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Bonus_physicalDamage -= handler.FDamage;
        pi.Bonus_magicDamage    -= handler.MDamage;
        pi.ArmorPenetration     -= handler.ArmorPenetration;
        pi.MagicPenetration     -= handler.MagicPenetration;
    }
    public override Type MyHandler() {
        return typeof(WeaponHandler);
    }

    public override Type MyType (){
        return typeof(Weapon);
    }

    public override void NewItemIndicator (bool active){
        InventoryTabManager.Instance.weaponTab.ExclamationMark.SetActive(active);
    }
}
