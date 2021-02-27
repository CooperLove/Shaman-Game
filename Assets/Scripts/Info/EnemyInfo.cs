using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyInfo : CharacterInfo
{
    [Header("Static is an enemy that doesn't move")]
    [SerializeField] private bool isStatic = false;

    [Header("XP given to player when an enemy dies")]
    public int xpGiven;
    [Header("Move speed")]
    public float velocity;

    public List<Quest> questsImIn = new List<Quest>();

    public bool IsStatic { get => isStatic; protected set => isStatic = value; }

    public virtual void SetupStats (Enemy e)
    {
        e.Hp = max_HP;
        e.Mp = max_SP;
        e.Sp = max_SP;

        e.Armor = Armor;
        e.MagicResistance = MagicResistance;
    }

    public override int CalculatePhysicalDamage() =>
        (int)(UnityEngine.Random.Range(minPhysicalDamage, MaxPhysicalDamage));

    public override int CalculateMagicalDamage() =>
        (int)(UnityEngine.Random.Range(minMagicDamage, MaxMagicDamage));

    public override int CalculateBasicAttackDamage() =>
        (int)(UnityEngine.Random.Range(minPhysicalDamage, MaxPhysicalDamage));
}
