using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
[CreateAssetMenu (menuName = "PlayerTest/Player")]
public class PlayerInfo : CharacterInfo
{
#region Variables
    [SerializeField] private int experience = 0;
    [SerializeField] private float nextLevelEXP = 0;

    [Header("Stats")]
    [SerializeField] private int _force = 5;
    [SerializeField] private int _dexterity = 5;
    [SerializeField] private int _intelligence = 5;
    [SerializeField] private int _constitution = 5;
    [SerializeField] private int _vigor = 5;
    [SerializeField] private int _statsPoints = 0;
    [SerializeField] private int _activeSkillPoints = 0;
    [SerializeField] private int _passiveSkillPoints = 0;

    [Header("HP MP SP bonus")]
    [SerializeField] private int bonus_hp = 0;
    [SerializeField] private int bonus_mp = 0;
    [SerializeField] private int bonus_sp = 0;

    [Header("Percentage bonus")]
    [SerializeField] private float bonus_percentage_hp = 0;
    [SerializeField] private float bonus_percentage_mp = 0;
    [SerializeField] private float bonus_percentage_sp = 0;
    [SerializeField] private float percentage_hp = 0;
    [SerializeField] private float percentage_mp = 0;
    [SerializeField] private float percentage_sp = 0;

    [Header("Stats bonus")]
    [SerializeField] private int bonus_force = 0;
    [SerializeField] private int bonus_dexterity = 0;
    [SerializeField] private int bonus_intelligence = 0;
    [SerializeField] private int bonus_constitution = 0;
    [SerializeField] private int bonus_vigor = 0;

    [Header("Defense bonus")]
    [SerializeField] private float bonus_armor = 0;
    [SerializeField] private float bonus_magicResistance = 0;

    [Header("Other bonus")]
    [SerializeField] private float bonus_moveSpeed = 0;
    [SerializeField] private float bonus_skillDamage = 0;
    [SerializeField] private float bonus_physicalDamage = 0;
    [SerializeField] private float bonus_magicDamage = 0;

    [Space(10), Header("Regen")]
    [SerializeField] private float manaRegen = 0.5f;
    [SerializeField] private float hpRegen = 0.5f;
    [SerializeField] private float spRegen = 0.5f;

    [Space(10), Header("Critical")]
    [SerializeField] private int critChance = 0;
    [SerializeField] private float  critDmg = 1f;
    [SerializeField] private float attackSpeed = 1f;

    [Space(10), Header("Elemental Resist")]
    [SerializeField] private float fireResist = 0f;
    [SerializeField] private float waterResist = 0f;
    [SerializeField] private float earthResist = 0f;
    [SerializeField] private float windResist = 0f;
    [SerializeField] private float darkResist = 0f;


    [Space(10), Header("Miscellaneous")]
    [SerializeField] private float basicAttackDamagekMultiplier = 1;
    [SerializeField] private float skillDamage = 1;
    [SerializeField] private float damageReduction = 0;
    [SerializeField] private float shield = 0;
    [SerializeField] private float moveSpeed = 100;
    [SerializeField] private float armorPenetration = 0;
    [SerializeField] private float magicPenetration = 0;
    [SerializeField] private float cdr = 0;
    [SerializeField] private float lifesteal = 0;
    [SerializeField] private float manaCostReduction = 0;
    [SerializeField] private float ccDurationReduction = 0;
    [SerializeField] private float potionEffectiveness = 0;
    [SerializeField] private float potionDuration = 0;
    [SerializeField] private float buffDuration = 0;
    [SerializeField] private float healEffectiveness = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float poisonDamage = 0;
    
    [Space(10), Header("Gold")]
    [SerializeField] private float gold = 0;

    [Space(10), Header("Current abilities")]
    [SerializeField] private Active autoAttack = null;
    [SerializeField] private Passive passive = null;
    [SerializeField] private UseSkill skill01 = null;
    [SerializeField] private UseSkill skill02 = null;
    [SerializeField] private UseSkill skill03 = null;
    [SerializeField] private UseSkill skill04 = null;
    [SerializeField] private List<Skill> learnedSkills = null;
    
    [Space(10), Header("Current weapon, gear and jewerly eqquipped")]
    [SerializeField] private Weapon weapon = null;
    [SerializeField] private Helmet helmet = null;
    [SerializeField] private ChestArmor chestArmor = null;
    [SerializeField] private Gloves gloves = null; 
    [SerializeField] private Legging leggins = null;
    [SerializeField] private Boot boots = null;
    [SerializeField] private Jewerly leftRing = null;
    [SerializeField] private Jewerly rightRing = null;
    [SerializeField] private Jewerly amullet = null;
    [SerializeField] private Jewerly ornament = null;

    [Space(10), Header("Caminho escolhido")]
    [SerializeField] private AnimalPath animalPath;

    [Space(10), Header("Dreamcatcher charge amount"), Range(0,5)]
    [SerializeField] private float chargeAmount = 0f;
#endregion
    
#region Properties
    public override float Health { get => health; set => health = Mathf.Clamp(value, 0, Max_HP + bonus_hp + bonus_percentage_hp); }
    public override float Mana { get => mana; set => mana = Mathf.Clamp(value, 0, Max_MP + bonus_mp + bonus_percentage_mp); }
    public override float Stamina { get => stamina; set => stamina = Mathf.Clamp(value, 0, Max_SP + bonus_sp + bonus_percentage_sp); }
    
#region Gold e Exp
    public int Experience { get => experience; set => experience = value; }
    public float NextLevelEXP { get => nextLevelEXP; set => nextLevelEXP = (float) System.Math.Round(value,0); }
    public float Gold { get => gold; set => gold = value; }
#endregion
#region Critical
    public int CriticalChance { get => critChance; set => critChance = value; }
    public float CriticalDamage { get => critDmg; set => critDmg = (float) Math.Round(value, 2); }
#endregion
#region Penetration e Attack Speed
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = (float) Math.Round(value, 2); }
    public float ArmorPenetration { get => armorPenetration; set => armorPenetration = value; }
    public float MagicPenetration { get => magicPenetration; set => magicPenetration = value; }
#endregion
#region Regen
    public float HPRegen { get => hpRegen; set => hpRegen = (float) Math.Round(value, 2); }
    public float MPRegen { get => manaRegen; set => manaRegen = (float) Math.Round(value, 2); }
    public float SPRegen { get => spRegen; set => spRegen = (float) Math.Round(value, 2); }
#endregion
#region Statistics
    public int Force { get => _force; set => _force = value; }
    public int Dexterity { get => _dexterity; set => _dexterity = value; }
    public int Intelligence { get => _intelligence; set => _intelligence = value; }
    public int Constitution { get => _constitution; set => _constitution = value; }
    public int Vigor { get => _vigor; set => _vigor = value; }
    public int StatsPoints { get => _statsPoints; set => _statsPoints = value; }
    public int ActiveSkillPoints { get => _activeSkillPoints; set => _activeSkillPoints = value; }
    public int PassiveSkillPoints { get => _passiveSkillPoints; set => _passiveSkillPoints = value; }


#endregion
#region Equipment
    public Weapon Weapon { get => weapon; set => weapon = value; }
    public Helmet Helmet { get => helmet; set => helmet = value; }
    public ChestArmor ChestArmor { get => chestArmor; set => chestArmor = value; }
    public Gloves Gloves { get => gloves; set => gloves = value; }
    public Legging Leggings { get => leggins; set => leggins = value; }
    public Boot Boots { get => boots; set => boots = value; }
    public Jewerly LeftRing { get => leftRing; set => leftRing = value; }
    public Jewerly RightRing { get => rightRing; set => rightRing = value; }
    public Jewerly Amullet { get => amullet; set => amullet = value; }
    public Jewerly Ornament { get => ornament; set => ornament = value; }
#endregion
#region Bonus
    public float Bonus_percentage_hp { get => bonus_percentage_hp; set => bonus_percentage_hp = (float) Math.Round(value, 2); }
    public float Bonus_percentage_mp { get => bonus_percentage_mp; set => bonus_percentage_mp = (float) Math.Round(value, 2); }
    public float Bonus_percentage_sp { get => bonus_percentage_sp; set => bonus_percentage_sp = (float) Math.Round(value, 2); }
    public float Percentage_hp { get => percentage_hp; set => percentage_hp = (float) Math.Round(value, 2); }
    public float Percentage_mp { get => percentage_mp; set => percentage_mp = (float) Math.Round(value, 2); }
    public float Percentage_sp { get => percentage_sp; set => percentage_sp = (float) Math.Round(value, 2); }
    public float Bonus_physicalDamage { get => bonus_physicalDamage; set => bonus_physicalDamage = value; }
    public float Bonus_armor { get => bonus_armor; set { bonus_armor = value; bonus_armor = (float) System.Math.Round(bonus_armor, 2);}}
    public float Bonus_magicResistance { get => bonus_magicResistance; set { bonus_magicResistance = value; bonus_magicResistance = (float) System.Math.Round(bonus_magicResistance, 2);} }
    public float Bonus_moveSpeed { get => bonus_moveSpeed; set => bonus_moveSpeed = value; }
    public float Bonus_skillDamage { get => bonus_skillDamage; set => bonus_skillDamage = (float) Math.Round(value, 2); }
    public float Bonus_magicDamage { get => bonus_magicDamage; set => bonus_magicDamage = value; }
    public int Bonus_force { get => bonus_force; set => bonus_force = value; }
    public int Bonus_dexterity { get => bonus_dexterity; set => bonus_dexterity = value; }
    public int Bonus_intelligence { get => bonus_intelligence; set => bonus_intelligence = value; }
    public int Bonus_constitution { get => bonus_constitution; set => bonus_constitution = value; }
    public int Bonus_vigor { get => bonus_vigor; set => bonus_vigor = value; }
    public int Bonus_hp { get => bonus_hp; set => bonus_hp = value; }
    public int Bonus_mp { get => bonus_mp; set => bonus_mp = value; }
    public int Bonus_sp { get => bonus_sp; set => bonus_sp = value; }
#endregion
#region Elemental resist
    public float FireResist { get => fireResist; set => fireResist = (float) Math.Round(value, 2); }
    public float WaterResist { get => waterResist; set => waterResist = (float) Math.Round(value, 2); }
    public float EarthResist { get => earthResist; set => earthResist = (float) Math.Round(value, 2); }
    public float WindResist { get => windResist; set => windResist = (float) Math.Round(value, 2); }
    public float DarkResist { get => darkResist; set => darkResist = (float) Math.Round(value, 2); }
#endregion
#region Other
    public float Lifesteal { get => lifesteal; set => lifesteal = (float) Math.Round(value, 2); }
    public float ManaCostReduction { get => manaCostReduction; set => manaCostReduction = (float) Math.Round(value, 2); }
    public float CcDurationReduction { get => ccDurationReduction; set => ccDurationReduction = (float) Math.Round(value, 2); }
    public float PotionEffectiveness { get => potionEffectiveness; set => potionEffectiveness = (float) Math.Round(value, 2); }
    public float PotionDuration { get => potionDuration; set => potionDuration = (float) Math.Round(value, 2); }
    public float HealEffectiveness { get => healEffectiveness; set => healEffectiveness = (float) Math.Round(value, 2); }
    public float BleedingDamage { get => bleedingDamage; set => bleedingDamage = (float) Math.Round(value, 2); }
    public float PoisonDamage { get => poisonDamage; set => poisonDamage = (float) Math.Round(value, 2); }
    public float BuffDuration { get => buffDuration; set => buffDuration = (float) Math.Round(value, 2); }
    public float SkillDamage { get => skillDamage; set => skillDamage = (float) Math.Round(value, 2); }
    public float Cdr { get => cdr; set => cdr = (float) Math.Round(value, 2); }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DamageReduction { 
        get => damageReduction; set => damageReduction = Mathf.Clamp(value, 0.4f, 1);
    }
    public float Shield { 
        get => shield; set => shield = Mathf.Clamp(value, 0, Mathf.Infinity); 
    }
    public AnimalPath AnimalPath { get => animalPath; set => animalPath = value; }
    public float ChargeAmount { get => chargeAmount; set => chargeAmount = (float) Math.Round(value, 2); }
    public Passive Passive { get => passive; set => passive = value; }
    public UseSkill Skill01 { get => skill01; set => skill01 = value; }
    public UseSkill Skill02 { get => skill02; set => skill02 = value; }
    public UseSkill Skill03 { get => skill03; set => skill03 = value; }
    public UseSkill Skill04 { get => skill04; set => skill04 = value; }
    public List<Skill> LearnedSkills { get => learnedSkills; set => learnedSkills = value; }
    public Active AutoAttack { get => autoAttack; set => autoAttack = value; }
    public float BasicAttackDamagekMultiplier { get => basicAttackDamagekMultiplier; set => basicAttackDamagekMultiplier = value; }

    #endregion
    #endregion
    public void OnGainEXP (int xp){
        Experience += xp;
        if (!Player.Instance.CompanionObj.CompName.Equals(""))
            Player.Instance.CompanionObj.OnGainEXP (xp);
        if (Experience >= NextLevelEXP){
            OnLevelUp();
        }
    }
    public void OnLevelUp(){
        LevelManager.Instance?.OnLevelUp();
        AnimalPath.OnLevelUp();
    }

    public override int CalculatePhysicalDamage () =>
        (int)(UnityEngine.Random.Range(minPhysicalDamage, MaxPhysicalDamage) * SkillDamage);
    public override int CalculateMagicalDamage () =>
        (int)(UnityEngine.Random.Range(minMagicDamage, MaxMagicDamage) * SkillDamage);

    public override int CalculateBasicAttackDamage () =>
        (int)(UnityEngine.Random.Range(minPhysicalDamage, MaxPhysicalDamage) * BasicAttackDamagekMultiplier);

    
}
