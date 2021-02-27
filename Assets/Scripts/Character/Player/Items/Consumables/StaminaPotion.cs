using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Consumable/Stamina Potion")]
public class StaminaPotion : Consumable
{
    public int staminaAmount;
    public override string GetDescriptionText()
    {
        return "This potion helps you in times of danger. Adventurers are advised to stock up"+
                " before entering dangerous situations.\n"+
                "This potion helps you in times of danger. Adventurers are advised to stock up"+
                " before entering dangerous situations.\n"+
                "This potion helps you in times of danger. Adventurers are advised to stock up"+
                " before entering dangerous situations.\n"+
                "This potion helps you in times of danger. Adventurers are advised to stock up"+
                " before entering dangerous situations.";
    }

    public override string GetEffectsText()
    {
        return "This recovers <color=green>"+staminaAmount+"</color> of your total stamina!";
    }


    public override void Use()
    {
        
    }
}
