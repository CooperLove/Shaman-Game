using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillMechanic : MonoBehaviour
{

    public abstract void ApplyDamage();

    public virtual void ApplyDamage(Enemy e, int damage)
    {
        e.TakeDamage(damage);
    }
    public void UsingSkill() => GameStatus.UsingSkill(true);
    public void NotUsingSkill() => GameStatus.UsingSkill(false);

}
