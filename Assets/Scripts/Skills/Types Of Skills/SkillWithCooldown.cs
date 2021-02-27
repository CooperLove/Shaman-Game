using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillWithCooldown : SkillWithCost
{
    public abstract void OnEnterOnCooldown ();
}
