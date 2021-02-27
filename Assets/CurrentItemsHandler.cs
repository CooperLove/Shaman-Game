using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentItemsHandler
{
    [SerializeField] private static HelmetHandler currentHelmet;
    [SerializeField] private static EquipmentHandler currentChestArmor;
    [SerializeField] private static EquipmentHandler currentGloves;
    [SerializeField] private static EquipmentHandler currentLeggins;
    [SerializeField] private static EquipmentHandler currentBoots;
    [SerializeField] private static EquipmentHandler currentWeapon;
    [SerializeField] private static EquipmentHandler currentLeftRing;
    [SerializeField] private static EquipmentHandler currentRightRing;
    [SerializeField] private static EquipmentHandler currentAmullet;
    [SerializeField] private static EquipmentHandler currentOrnament;

    public static HelmetHandler CurrentHelmet { get => currentHelmet; set => currentHelmet = value; }
    public static EquipmentHandler CurrentChestArmor { get => currentChestArmor; set => currentChestArmor = value; }
    public static EquipmentHandler CurrentGloves { get => currentGloves; set => currentGloves = value; }
    public static EquipmentHandler CurrentLeggings { get => currentLeggins; set => currentLeggins = value; }
    public static EquipmentHandler CurrentBoots { get => currentBoots; set => currentBoots = value; }
    public static EquipmentHandler CurrentLeftRing { get => currentLeftRing; set => currentLeftRing = value; }
    public static EquipmentHandler CurrentRightRing { get => currentRightRing; set => currentRightRing = value; }
    public static EquipmentHandler CurrentAmullet { get => currentAmullet; set => currentAmullet = value; }
    public static EquipmentHandler CurrentOrnament { get => currentOrnament; set => currentOrnament = value; }
    public static EquipmentHandler CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }

    public static void OnEquipUpdateStats (){
        if (currentHelmet != null) ((Equipment)currentHelmet?.Item).OnChangeEquipment(currentHelmet);
        if (currentChestArmor != null) ((Equipment)currentChestArmor?.Item).OnChangeEquipment(currentChestArmor);
        if (currentGloves != null) ((Equipment)currentGloves?.Item).OnChangeEquipment(currentGloves);
        if (currentLeggins != null) ((Equipment)currentLeggins?.Item).OnChangeEquipment(currentLeggins);
        if (currentBoots != null) ((Equipment)currentBoots?.Item).OnChangeEquipment(currentBoots);
        if (currentWeapon != null) ((Equipment)currentWeapon?.Item).OnChangeEquipment(currentWeapon);
        if (currentLeftRing != null) ((Equipment)currentLeftRing?.Item).OnChangeEquipment(currentLeftRing);
        if (currentRightRing != null) ((Equipment)currentRightRing?.Item).OnChangeEquipment(currentRightRing);
        if (currentOrnament != null) ((Equipment)currentOrnament?.Item).OnChangeEquipment(currentOrnament);
        if (currentAmullet != null) ((Equipment)currentAmullet?.Item).OnChangeEquipment(currentAmullet);
    }
}
