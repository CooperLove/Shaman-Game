using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armature : Equipment
{
    [Header("Defense values")]
    [SerializeField] private float armor = 0;
    [SerializeField] private float magicResistance = 0;

    public float Armor { get => armor; set => armor = value; }
    public float MagicResistance { get => magicResistance; set => magicResistance = value; }
}
