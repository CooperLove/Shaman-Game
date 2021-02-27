using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorTable
{
    private static readonly Color UncommonColor = new Color(0.5f ,0.9f, 0.7f);
    private static readonly Color CommonColor = new Color(0.5f, 0.7f, 0.95f);
    private static readonly Color GreatColor = Color.green;
    private static readonly Color RareColor = new Color(0.95f, 0.95f, 0.45f);
    private static readonly Color EpicColor = Color.red;
    private static readonly Color LegendaryColor = new Color(0.6f, 0.25f, 0.95f);
    private static readonly Color DivineColor = new Color(1, 0.55f, 0.3f);
    private static Color primaryColor = new Color(0.183f, 0.217f, 0.25f);
    private static Color secondaryColor = new Color(0.207f, 0.231f, 0.282f);

    public static Color PrimaryColor { get => primaryColor; set => primaryColor = value; }
    public static Color SecondaryColor { get => secondaryColor; set => secondaryColor = value; }

    public static Color ItemNameColor (int rarity){ 
        
        //Debug.Log(uncommonColor+" "+commonColor);
        Color c;
        switch (rarity)
        {
            case 0:
                c = UncommonColor; break;
            case 1:
                c = CommonColor; break;
            case 2:
                c = GreatColor; break;
            case 3:
                c = RareColor; break;
            case 4:
                c = EpicColor; break;
            case 5:
                c = LegendaryColor; break;
            case 6:
                c = DivineColor; break;
            default:
                c = Color.white; break;
        }
        return c;
    }
}
