using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : SkillWithCost
{
    public abstract void OnUse ();
    public abstract void OnUse (float value, bool isPercentage = false);
}
