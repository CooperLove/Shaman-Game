using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OneTimePassive : Skill
{   
   public bool isFlat = false;
   public abstract bool OnLearn (float value, bool isFlat = true);
   public override void OnLearn(){}
}
