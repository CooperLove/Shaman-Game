using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Manabar : MonoBehaviour
{
    public CharacterInfo character;
    private float mp, maxMp;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private TMP_Text mpText = null;
    private void Awake() {
        maxMp = character.Max_MP;
        
        UpdateMP_Bar();
    }
    private void Update() {
        mp = character.Mana;
        maxMp = character.Max_MP;
        
        if ((int)mp > maxMp) {
            mp = maxMp;
            character.Mana = mp;
            return;
        }
        
        if (character is PlayerInfo info){
            maxMp = info.Max_MP + info.Bonus_mp + info.Bonus_percentage_mp;
            RegenMp();
        }
        
        UpdateMP_Bar();
    }

    private void RegenMp (){
        character.Mana += ((PlayerInfo) character).MPRegen * Time.deltaTime;
        mp = character.Mana;
    }

    private void UpdateMP_Bar ()
    {
        var percentageMp = mp / maxMp;

        if (fillImage)
            fillImage.fillAmount = percentageMp;
        
        UpdateMP_Text (percentageMp);
    }
    
    private void UpdateMP_Text (float currentMp){
        if (!mpText)
            return;
        
        mpText.text = $"{System.Math.Round(currentMp * 100f, 0)}/{maxMp}";
    }
    
    public void LoseMp (){
        character.Mana -= 10;
        mp = character.Mana;
    }

    public void OnLevelUp()
    {
        if (!character)
            return; 

        maxMp = character.Max_MP;
        UpdateMP_Bar();
    }
}
