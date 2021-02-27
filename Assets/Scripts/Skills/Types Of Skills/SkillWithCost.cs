using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillWithCost : UpgradableSkill
{
    [SerializeField] private float hpCost;                   // Custo para usar a skill
    [SerializeField] private float hpCostIncrement;          // Incremento no custo ao se aprimorar a skill
    [SerializeField] private float mpCost;                   // Custo para usar a skill
    [SerializeField] private float mpCostIncrement;          // Incremento no custo ao se aprimorar a skill
    [SerializeField] private float spCost;                   // Custo para usar a skill
    [SerializeField] private float spCostIncrement;          // Incremento no custo ao se aprimorar a skill

    public float HpCostIncrement { get => hpCostIncrement; protected set => hpCostIncrement = value; }
    public float MpCostIncrement { get => mpCostIncrement; protected set => mpCostIncrement = value; }
    public float SpCostIncrement { get => spCostIncrement; protected set => spCostIncrement = value; }
    public float HpCost { get => hpCost; protected set => hpCost = value; }
    public float SpCost { get => spCost; protected set => spCost = value; }
    public float MpCost { get => mpCost; protected set => mpCost = value; }

    
}
