using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CharacterInfo : Info
{
    [SerializeField] private Vector3 position;
    private bool isDead = false;
    
    [Header("Level stats")]
    [SerializeField] protected int level;

    [Header("Max HP/MP/SP")]
    [SerializeField] protected float max_HP;
    [SerializeField] protected float max_MP;
    [SerializeField] protected float max_SP;

    [Header("General")]
    [SerializeField] protected float health;
    [SerializeField] protected float mana;
    [SerializeField] protected float stamina;

    [Header("Damage")]
    [SerializeField] protected int minPhysicalDamage;
    [SerializeField] private int maxPhysicalDamage;
    [SerializeField] protected int minMagicDamage;
    [SerializeField] private int maxMagicDamage;

    [Header("Resistances")]
    [SerializeField] protected float armor;
    [SerializeField] protected float magicResistance;
    [SerializeField] private float bleedingResistance;
    [SerializeField] private float poisonResistance;

    

    public virtual float Health { get => health; 
                    set { health = value; health = Mathf.Clamp(health, 0, Max_HP); } }
    public virtual float Mana { get => mana; set => mana = value; }
    public virtual float Stamina { get => stamina; set => stamina = value; }
    public float Max_HP { get => max_HP; set => max_HP = value; }
    public float Max_MP { get => max_MP; set => max_MP = value; }
    public float Max_SP { get => max_SP; set => max_SP = value; }
    public int MinPhysicalDamage { get => minPhysicalDamage; set => minPhysicalDamage = value; }
    public int MaxPhysicalDamage { get => maxPhysicalDamage; set => maxPhysicalDamage = value; }
    public int MinMagicDamage { get => minMagicDamage; set => minMagicDamage = value; }
    public int MaxMagicDamage { get => maxMagicDamage; set => maxMagicDamage = value; }
    public float Armor { get => armor; set => armor = value; }
    public float MagicResistance { get => magicResistance; set => magicResistance = value; }
    public int Level { get => level; set => level = value; }
    public float BleedingResistance { get => bleedingResistance; set => bleedingResistance = value; }
    public float PoisonResistance { get => poisonResistance; set => poisonResistance = value; }
    public Vector3 Position { get => position; set => position = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    public abstract int CalculatePhysicalDamage ();
    public abstract int CalculateMagicalDamage ();
    public abstract int CalculateBasicAttackDamage ();
}
