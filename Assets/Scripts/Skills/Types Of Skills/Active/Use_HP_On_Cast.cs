using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Use_HP_On_Cast 
{
    private float hpCost = 0f;

    public float HpCost { get => hpCost; set => hpCost = value; }
}
