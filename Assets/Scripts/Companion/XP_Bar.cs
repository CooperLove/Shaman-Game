using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XP_Bar : MonoBehaviour
{
    private float xp, maxXP;
    public TMP_Text xpText;
    public RectTransform xpRect;
    private Player player;
    private Companion companion;
    // Start is called before the first frame update
    private void Awake() {
        player = Player.Instance; 
        companion = Player.Instance.CompanionObj;
        xp = companion.Xp;
        maxXP = companion.XpToLevelUp;
    }

    // Update is called once per frame
    void Update()
    {
        xp = companion.Xp;
        maxXP = companion.XpToLevelUp;
        if (companion.Xp >= companion.XpToLevelUp)
            return;
        
        UpdateXP();
        UpdateText();
        RegenXP ();
    }

    private void UpdateXP (){
        xp = companion.Xp;
        float currXP = xp/companion.XpToLevelUp;
        xpRect.localScale = new Vector3 (currXP, 1, 1);
    }

    private void UpdateText (){
        if (xpText != null)
            xpText.text = (int) companion.Xp+"/"+(int) companion.XpToLevelUp;
    }

    private void RegenXP (){

    }
}
