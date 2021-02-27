using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Consumable/Mana Potion")]
public class ManaPotion : Consumable
{
    public int manaAmount;
    public override string GetDescriptionText()
    {
        return $"This potion helps you in times of danger. Adventurers are advised to stock up"+
                $" before entering dangerous situations.\n"+
                $"This potion helps you in times of danger. Adventurers are advised to stock up"+
                $" before entering dangerous situations.\n"+
                $"This potion helps you in times of danger. Adventurers are advised to stock up"+
                $" before entering dangerous situations.\n";
    }

    public override string GetEffectsText()
    {
        return $"Effects: \n"+ 
            "\t Recovers <color=blue>"+manaAmount+"</color> of your total mana!\n"+
            "\t Recovers <color=blue>"+manaAmount+"</color> of your total mana!\n"+
            "\t Recovers <color=blue>"+manaAmount+"</color> of your total mana!";
    }


    public override void Use()
    {
        
    }
}
