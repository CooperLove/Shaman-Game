using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetHandler : GearHandler
{
    public override ItemHandler FillHandler (Item item){
        return null;
    }
    [SerializeField] private float percentageMagicResistance;
    public float PercentageMagicResistance { get => percentageMagicResistance; set { percentageMagicResistance = value; Mathf.Clamp(percentageMagicResistance,0, 8);} }

}
