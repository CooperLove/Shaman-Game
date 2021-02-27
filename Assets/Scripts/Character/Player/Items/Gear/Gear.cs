using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gear : Armature
{
    
    [SerializeField] private int health = 0;

    protected int Health { get => health; set => health = value; }

    public override string GetDescriptionText()
    {
        return "";
    }
    
    public override string GetAttributesText(EquipmentHandler handler)
    {
        return $"ARMOR: <color=#E69913>{Armor}</color> \t\t\t\t\tLVL: <color=#E69913>{NecessaryLevel}</color> \n"+
               $"MAGIC RESIST: <color=#E69913>{MagicResistance}</color>\t\t\t\tENHANC: <color=#E69913>+{Enhancement}</color> \n"+
               $"HEALTH: <color=#E69913>+{health}</color>";
    }
    
    public abstract override void OnChangeEquipment(EquipmentHandler handler);

    

    public override Type MyType (){
        return typeof(Gear);
    }

    public override void NewItemIndicator (bool active){
        InventoryTabManager.Instance.gearTab.ExclamationMark.SetActive(active);
    }
}
