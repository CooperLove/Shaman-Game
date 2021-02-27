using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : Item
{
    public const int MAX_NUMBER_OF_COLLECTABLE_ENCH = 3;
    public abstract string GetEffectsText (); 
}
