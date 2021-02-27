using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuUI : MonoBehaviour
{
    static CharacterMenuUI instance;
    [SerializeField] private Sprite defaultImage = null;
    [SerializeField] private Image helm = null;
    [SerializeField] private Image chest = null;
    [SerializeField] private Image legging = null;
    [SerializeField] private Image boot = null;
    [SerializeField] private Image gloves = null;
    [SerializeField] private Image weapon = null;
    [SerializeField] private Image ring01 = null;
    [SerializeField] private Image ring02 = null;
    [SerializeField] private Image amullet = null;
    [SerializeField] private Image ornament = null;

    [SerializeField] private EquipmentHandler helmHandler;
    [SerializeField] private EquipmentHandler chestHandler;
    [SerializeField] private EquipmentHandler leggingHandler;
    [SerializeField] private EquipmentHandler bootHandler;
    [SerializeField] private EquipmentHandler glovesHandler;
    [SerializeField] private EquipmentHandler weaponHandler;
    [SerializeField] private EquipmentHandler ring01Handler;
    [SerializeField] private EquipmentHandler ring02Handler;
    [SerializeField] private EquipmentHandler amulletHandler;
    [SerializeField] private EquipmentHandler ornamentHandler;

    public GameObject cMenu;

    public Image Helm { get => helm; set => helm = value; }
    public Image Chest { get => chest; set => chest = value; }
    public Image Legging { get => legging; set => legging = value; }
    public Image Boot { get => boot; set => boot = value; }
    public Image Gloves { get => gloves; set => gloves = value; }
    public Image Ring01 { get => ring01; set => ring01 = value; }
    public Image Ring02 { get => ring02; set => ring02 = value; }
    public Image Amullet { get => amullet; set => amullet = value; }
    public Image Ornament { get => ornament; set => ornament = value; }
    public EquipmentHandler HelmHandler { get => helmHandler; set => helmHandler = value; }
    public EquipmentHandler ChestHandler { get => chestHandler; set => chestHandler = value; }
    public EquipmentHandler LeggingHandler { get => leggingHandler; set => leggingHandler = value; }
    public EquipmentHandler BootHandler { get => bootHandler; set => bootHandler = value; }
    public EquipmentHandler GlovesHandler { get => glovesHandler; set => glovesHandler = value; }
    public EquipmentHandler WeaponHandler { get => weaponHandler; set => weaponHandler = value; }
    public EquipmentHandler Ring01Handler { get => ring01Handler; set => ring01Handler = value; }
    public EquipmentHandler Ring02Handler { get => ring02Handler; set => ring02Handler = value; }
    public EquipmentHandler AmulletHandler { get => amulletHandler; set => amulletHandler = value; }
    public EquipmentHandler OrnamentHandler { get => ornamentHandler; set => ornamentHandler = value; }
    public static CharacterMenuUI Instance { get => instance;}

    CharacterMenuUI () {
        if (instance == null)
            instance = this;
    }
    public void OpenCharacterMenu () => cMenu.SetActive(true);
    public void CloseCharacterMenu () => cMenu.SetActive(false);
    public void ChangeUI_Images (){
        PlayerInfo pi = Player.Instance.playerInfo;
        helm.sprite     =  pi.Helmet != null ? pi.Helmet.Sprite : defaultImage;
        chest.sprite    =  pi.ChestArmor != null ? pi.ChestArmor.Sprite : defaultImage;
        legging.sprite  =  pi.Leggings != null ? pi.Leggings.Sprite : defaultImage;
        boot.sprite     =  pi.Boots != null ? pi.Boots.Sprite : defaultImage;
        gloves.sprite   =  pi.Gloves != null ? pi.Gloves.Sprite : defaultImage;
        weapon.sprite   =  pi.Weapon != null ? pi.Weapon.Sprite : defaultImage;
        ring01.sprite   =  pi.LeftRing != null ? pi.LeftRing.Sprite : defaultImage;
        ring02.sprite   =  pi.RightRing != null ? pi.RightRing.Sprite : defaultImage;
        amullet.sprite  =  pi.Amullet != null ? pi.Amullet.Sprite : defaultImage;
        ornament.sprite =  pi.Ornament != null ? pi.Ornament.Sprite : defaultImage;
    }

    public void ChangeHandlers (){
        HelmHandler = CurrentItemsHandler.CurrentHelmet;
        ChestHandler = CurrentItemsHandler.CurrentChestArmor;
        LeggingHandler = CurrentItemsHandler.CurrentLeggings;
        BootHandler = CurrentItemsHandler.CurrentBoots;
        GlovesHandler = CurrentItemsHandler.CurrentGloves;
        WeaponHandler = CurrentItemsHandler.CurrentWeapon;
        Ring01Handler = CurrentItemsHandler.CurrentLeftRing;
        Ring02Handler = CurrentItemsHandler.CurrentRightRing;
        AmulletHandler = CurrentItemsHandler.CurrentAmullet;
        OrnamentHandler = CurrentItemsHandler.CurrentOrnament;
    }

    public void ActionHelmHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = helmHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) helmHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionChestHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = chestHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) chestHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionBootsHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = bootHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) bootHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionGlovesHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = glovesHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) glovesHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionLeggingHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = leggingHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) leggingHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionRing01Handler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = ring01Handler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) ring01Handler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionRing02Handler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = ring02Handler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) ring02Handler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionAmulletHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = amulletHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) amulletHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionOrnamentHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = ornamentHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) ornamentHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void ActionWeaponHandler (){
        ChangeHandlers ();
        EquipmentDescriptionPanel.Instance.Handler = weaponHandler;
        EquipmentDescriptionPanel.Instance.SetEquipment((Equipment) weaponHandler?.Item);
        CharacterMenuOpenEquipmentDesc();
    }
    public void CharacterMenuOpenEquipmentDesc (){
        //CloseCharacterMenu();
        DescriptionPanel.Instance.Close();
        EquipmentDescriptionPanel.Instance.ChangeText();
        EquipmentDescriptionPanel.Instance.Open();
    }

}
