using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private int index = 0;
    [SerializeField] protected string spellName = "";
    [SerializeField, TextArea(3,10)] protected string description = "";
    [SerializeField] protected int currentLevel = 0;
    [SerializeField] protected int maxLevel = 0;
    [SerializeField] protected int requiredPlayerLevel = 0;
    [SerializeField] protected int levelIncrement = 0;
    [SerializeField] protected float cooldown = 0;
    [SerializeField] protected float cost = 0;
    [SerializeField] protected float costIncrement = 0;
    [SerializeField] protected RequiredSpecializations specializations = null;
    [SerializeField] protected bool canBeUpgraded = false;

    public int RequiredPlayerLevel { get => requiredPlayerLevel; set => requiredPlayerLevel = value; }
    public RequiredSpecializations Specializations { get => specializations; set => specializations = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int LevelIncrement { get => levelIncrement; set => levelIncrement = value; }
    public bool CanBeUpgraded { get => canBeUpgraded; set => canBeUpgraded = value; }
    public float Cost { get => cost; set => cost = value; }
    public float CostIncrement { get => costIncrement; set => costIncrement = value; }
    public float Cooldown { get => cooldown; set => cooldown = value; }
    public string Description { get => description; set => description = value; }
    public string SpellName { get => spellName; set => spellName = value; }
    public int Index { get => index; }

    public abstract void OnSkillUpgrade (TMP_Text text);

    public abstract void Use ();
    
    public abstract string GetDescription ();

    public abstract Spell MakeCopy ();
    /*
    
    */
}

[System.Serializable]
public class RequiredSpecializations {
    public int healing;
    public int sheilding;
    public int buffing;
}