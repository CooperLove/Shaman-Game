using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class MP_Bar : MonoBehaviour
{
    private float mp, maxMP;
    public TMP_Text mpText;
    public RectTransform mpRect;
    private Player player;
    private Companion companion;
    private static MP_Bar instance;

    public static MP_Bar Instance { get => instance;}
    MP_Bar (){
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    private void Awake() {
        player = Player.Instance; 
        companion = player.CompanionObj;
        mp = companion.Mana;
        maxMP = companion.MaxMana;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.Instance.CompanionObj.CompName.Equals(Player.Instance.Companion.CompName)){
            return;
        }
        mp = companion.Mana;
        maxMP = companion.MaxMana;
        if ((int)companion.Mana > companion.MaxMana)
            return;
        UpdateMP();
        UpdateText();
        RegenMP ();
    }

    private void UpdateMP (){
        mp = companion.Mana;
        float currMP = mp/companion.MaxMana;
        mpRect.localScale = new Vector3 (currMP, 1, 1);
    }

    private void UpdateText (){
        if (mpText != null)
            mpText.text = (int) companion.Mana+"/"+(int) companion.MaxMana;
    }

    private void RegenMP (){
        companion.Mana += companion.ManaRegen * Time.unscaledDeltaTime;
    }

    public void CompUpdateManabar (){
        mpRect.localScale = new Vector3 (1, 1, 1);
        UpdateText();
    }
}
