using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item
{
    public const int MAX_NUMBER_OF_EQUIPMENT_ENCH = 5;
    public const int MAX_ENHANCEMENT = 12;
    public static readonly float[] percEnhancement = {100f, 60f, 40f, 30f, 20f, 15f, 10f, 5f, 2.5f, 1f, 0.5f, 0.1f};
    //public static readonly float[] percEnhancement = {100f, 100f,100f,100f,100f,100f,100f,100f,100f,100f,100f,100f,};

    

    [Header("Requirements")]
    [SerializeField] private int _necessaryLevel;
    [SerializeField] private int _necessaryForce;
    [SerializeField] private int _necessaryInt;
    [SerializeField] private int _necessaryDex;
    [SerializeField] private int _necessaryCon;
    [SerializeField] private int _necessaryVigor;
    

    [Header("Increases the damage or defense of an item")] 
    private int _enhancement;

    [Header("Phrase displayed on the item")]
    [SerializeField, TextArea(3,10)] private string _phrase;

    protected int NecessaryLevel { get => _necessaryLevel; set => _necessaryLevel = value; }
    public int NecessaryForce { get => _necessaryForce; set => _necessaryForce = value; }
    public int NecessaryInt { get => _necessaryInt; set => _necessaryInt = value; }
    public int NecessaryDex { get => _necessaryDex; set => _necessaryDex = value; }
    public int NecessaryCon { get => _necessaryCon; set => _necessaryCon = value; }
    public int NecessaryVigor { get => _necessaryVigor; set => _necessaryVigor = value; }
    public int Enhancement { get => _enhancement; set => _enhancement = Mathf.Clamp(value, 0, MAX_ENHANCEMENT); }
    public string Phrase { get => _phrase; set => _phrase = value; }
    

    public abstract bool OnEnhancement(GemHandler gem, EquipmentHandler handler);
    public abstract string GetAttributesText(EquipmentHandler handler);

    public abstract void OnChangeEquipment (EquipmentHandler handler);
    public abstract bool OnRemoveEquipment (EquipmentHandler handler);
    public virtual string GetRequirementsText(){    
        return "Requires:\n"+
            $"\t<color=#E69913>{_necessaryForce}</color> Force \t\t\t "+
            $"<color=#E69913>{_necessaryInt}</color> Intelligence \t <color=#E69913>{_necessaryDex}</color> Dexterity\n"+
            $"\t<color=#E69913>{_necessaryCon}</color> Constitution \t\t <color=#E69913>{_necessaryVigor}</color> Vigor";
    }
    
    
}
