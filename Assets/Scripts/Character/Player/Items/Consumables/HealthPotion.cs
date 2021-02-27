using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Consumable/Health Potion")]
public class HealthPotion : Consumable
{
    public int healAmount;
    public override string GetDescriptionText()
    {
        return "This potion helps you in times of danger. Adventurers are advised to stock up"+
                " before entering dangerous situations.";
    }

    public override string GetEffectsText()
    {
        return "<align=left>></align> heals you for <color=red>"+healAmount+"</color>!"+
                "\n<align=left>></align> heals you for <color=red>"+healAmount+"</color>!";
    }


    public override void Use()
    {
        
    }

}
