using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Companion : MonoBehaviour
{
    public enum CompanionType
    {
        Initial, Second, Third, Fourth
    }
    private Player player;
    [SerializeField] private string compName;
    [SerializeField] private Sprite compImage;
    [SerializeField] private CompanionType type;
    
    [Header("General attributes"), Space(10)]
    [SerializeField] private float maxMana;
    [SerializeField] private float mana;
    [SerializeField] private float manaRegen;
    [SerializeField] private float spellEffectMultiplier;
    [SerializeField] private int level;
    [SerializeField] private int xp;
    [SerializeField] private int xpToLevelUp;

    //Attributes
    [Header("Basic attributes"), Space(10)]
    [SerializeField, Tooltip("Increases the companion maximum mana")] private int intelligence;
    [SerializeField, Tooltip("Decreases how much mana a spell uses")] private int intellect;
    [SerializeField, Tooltip("Increases the effect of a spell")] private int wisdom;
    [SerializeField] private int additionalPoints;

    //Bonus stats for the player
    [Header("Additional points for the player")]
    [SerializeField] private int additionalForce;
    [SerializeField] private int additionalInt;
    [SerializeField] private int additionalDex;
    [SerializeField] private int additionalCon;
    [SerializeField] private int additionalVigor;

    //Types of crystals - Used to evolve companions
    [Header("Crystals used to evolve your companion"), Space(10)]
    [SerializeField, Tooltip("T1 crystals are the simplest out of the 3 existents. It's used to evolve the companion but it wont give it much power.")] 
    private int crystalT1;
    [SerializeField, Tooltip("A better version of an evolving crystal. It makes your companion stronger than using T1 crystals.")] 
    private int crystalT2;
    [SerializeField, Tooltip("The best of them, it maxes the power of your companion.")] 
    private int crystalT3;

    //Specialities
    [Header("Points to learn new spells"), Space(10)]
    [SerializeField] private int specializationPoints;
    [SerializeField, Tooltip("Tree path focused on healing spells")] private int healing;
    [SerializeField, Tooltip("Tree path focused on shilding spells")] private int shielding;
    [SerializeField, Tooltip("Tree path focused on buffing spells")] private int buffing;

    //Points to upgrade spells
    [Header("Points to upgrade spell"), Space(10)]
    [SerializeField, Tooltip("Points used to learn and upgrade new spells")] private int spellPoints;
    
    //Unique Spell
    [Header("Unique Spell"), Space(10)]
    [SerializeField, Tooltip("The unique spell that each evolution has")] private GameObject uniqueSpell;
    [SerializeField] private List<GameObject> spells;
    public CompanionHandler companionHandler;
    [SerializeField] private int currentEvoIndex;
    [SerializeField] private List<int> followedPath;
    [SerializeField] private List<int> chosenMissions;
    [SerializeField] private List<int> inProgressMissions;
    [SerializeField] private int maxNumberOfSpells;
    
    public Player Player { get => player; set => player = value; }
    public float MaxMana { get => maxMana; set => maxMana = value;}
    public float Mana { get => mana; set => mana = value; }
    public int Level { get => level; set => level = value; }
    public int Xp { get => xp; set => xp = value; }
    public int XpToLevelUp { get => xpToLevelUp; set => xpToLevelUp = value;}
    public int Wisdom { get => wisdom; set => wisdom = value; }
    public int Intellect { get => intellect; set => intellect = value; }
    public int Intelligence { get => intelligence; set => intelligence = value; }
    public int AdditionalPoints { get => additionalPoints; set => additionalPoints = value; }
    public int AdditionalForce { get => additionalForce; set => additionalForce = value; }
    public int AdditionalInt { get => additionalInt; set => additionalInt = value; }
    public int AdditionalDex { get => additionalDex; set => additionalDex = value; }
    public int AdditionalCon { get => additionalCon; set => additionalCon = value; }
    public int AdditionalVigor { get => additionalVigor; set => additionalVigor = value; }
    public int CrystalT1 { get => crystalT1; set => crystalT1 = value; }
    public int CrystalT2 { get => crystalT2; set => crystalT2 = value; }
    public int CrystalT3 { get => crystalT3; set => crystalT3 = value; }
    public int Healing { get => healing; set => healing = value; }
    public int Shielding { get => shielding; set => shielding = value; }
    public int Buffing { get => buffing; set => buffing = value; }
    public int SpecializationPoints { get => specializationPoints; set => specializationPoints = value; }
    public int SpellPoints { get => spellPoints; set => spellPoints = value; }
    public GameObject UniqueSpell { get => uniqueSpell; set => uniqueSpell = value; }
    public List<GameObject> Spells { get => spells; set => spells = value; }
    public string CompName { get => compName; set => compName = value; }
    public CompanionType Type { get => type; set => type = value; }
    public Sprite CompImage { get => compImage; set => compImage = value; }
    public int CurrentEvoIndex { get => currentEvoIndex; set => currentEvoIndex = value; }
    public List<int> FollowedPath { get => followedPath; set => followedPath = value; }
    public List<int> ChosenMissions { get => chosenMissions; set => chosenMissions = value; }
    public List<int> InProgressMissions { get => inProgressMissions; set => inProgressMissions = value; }
    public float ManaRegen { get => manaRegen; set => manaRegen = value; }
    public float SpellEffectMultiplier { get => spellEffectMultiplier; set => spellEffectMultiplier = value; }
    public int MaxNumberOfSpells { get => maxNumberOfSpells; set { maxNumberOfSpells = value ; maxNumberOfSpells = Mathf.Clamp(maxNumberOfSpells, 0 , 5); } }

    public abstract void FollowPlayer();
    public abstract void OnGainEXP (int xp);
    public abstract void OnLevelUp();
    public abstract void AddUniqueSpell ();
    public abstract void ChangeCompanionStats (Companion c);
    public abstract void UpdateSkillTreeUI ();
    public abstract void ReactivateButtons ();
    public abstract void RefillPaths ();
    public abstract void ApplyBonusStats ();
    public abstract void UnapplyBonusStats ();

    public virtual Type MyHandler (){
        return typeof(CompanionHandler);
    }

    public virtual Type MyType (){
        return typeof(Companion);
    }

    public virtual void NewItemIndicator (bool active){
        InventoryTabManager.Instance.companionTab.ExclamationMark.SetActive(active);
    }
}
