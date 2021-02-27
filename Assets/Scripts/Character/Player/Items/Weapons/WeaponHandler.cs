using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponHandler : EquipmentHandler
{
    [Header("Damage")]
    [SerializeField] private int _fDamage;
    [SerializeField] private int _mDamage;
    [SerializeField] private int _armorPenetration;
    [SerializeField] private int _magicPenetration;
    
    public int FDamage { get => _fDamage; set => _fDamage = value; }
    public int MDamage { get => _mDamage; set => _mDamage = value; }
    public int ArmorPenetration { get => _armorPenetration; set => _armorPenetration = value; }
    public int MagicPenetration { get => _magicPenetration; set => _magicPenetration = value; }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect ();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect ();
        RemoveListerners();
    }    

    private void Start() {
        base.ApllyGemsStats();
        //foreach (Enchantment ench in Enchantments)
            //ench.Apply();
        this.FDamage = ((Weapon)item).FDamage;
        this.MDamage = ((Weapon)item).MDamage;
        base.ChangeText();
    }

    public void OnClick (){
        EquipmentDescriptionPanel.Instance.SetEquipment((Weapon)item);
        EquipmentDescriptionPanel.Instance.SetEquipment(this);
        EquipmentDescriptionPanel.Instance.Open();
        DescriptionPanel.Instance.Close();
        EquipmentDescriptionPanel.Instance.ChangeText();
    }
    public override Item CreateNewItem()
    {
        this.Rarity = (Item.Rarity) Random.Range(0, 7);
        string weaponLevel = ((Player.Instance.playerInfo.Level/10) * 10).ToString();
        string[] split = Player.Instance.playerInfo.AnimalPath.ToString().Split('_', ' ');
        string weaponType = $"{split[1]}";
        string item = Random.Range(0, 3).ToString();
        string path = $"ScriptableObjects/Items/Weapons/{weaponType}/Level {weaponLevel}/{FindWeaponType(weaponType)} {item}";
        Debug.Log($"{this.rarity} Caminho: {path}");
        Weapon weapon = Resources.Load<Weapon>(path);
        Debug.Log(weapon);
        return weapon;
    }

    private string FindWeaponType (string animal){
        if (animal.Equals("Bull"))
            return "Mace";
        if (animal.Equals("Crow") || animal.Equals("Crocodile"))
            return "Staff";
        if (animal.Equals("Eagle"))
            return "Bow";
        if (animal.Equals("Tiger"))
            return "Claw";

        return "";
    }

}
