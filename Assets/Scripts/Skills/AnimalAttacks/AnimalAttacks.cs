using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalAttacks : MonoBehaviour
{
    [Header("Cooldown de cada skill")]
    protected float basicAttackCd;
    protected float firstCd;
    protected float secondCd;
    protected float thirdCd;
    protected float fourthCd;
    protected float fifthCd;

    [Header("Controla quando se pode usar cada skill")]
    protected bool canUseBasicAttack;
    protected bool canUseFirstSpell;
    protected bool canUseSecondSpell;
    protected bool canUseThirdSpell;
    protected bool canUseFourthSpell;
    protected bool canUseFifthSpell;

    [Header("Skills")]
    [SerializeField] protected Active basicAttack;
    [SerializeField] protected Passive passive;
    [SerializeField] protected Active first;
    [SerializeField] protected Active second;
    [SerializeField] protected Active third;
    [SerializeField] protected Active fourth;
    [SerializeField] protected Active fifth;

    public Passive Passive { get => passive; }

    public virtual void CanUseAA () => canUseBasicAttack = true;
    public virtual void CanUseLow () => canUseFirstSpell = true;
    public virtual void CanUseMedium () => canUseSecondSpell = true;
    public virtual void CanUseHigh () => canUseThirdSpell = true;
}
