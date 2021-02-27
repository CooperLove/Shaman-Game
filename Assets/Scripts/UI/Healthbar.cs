using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public CharacterInfo character;
    private float hp, maxHp;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private TMP_Text hpText = null;
    private void Awake() {
        maxHp = character.Max_HP;
        
        UpdateHP_Bar();
    }
    private void Update() {
        if (character.IsDead)
            return;
        
        hp = character.Health;
        maxHp = character.Max_HP;
        
        if ((int)hp > maxHp) {
            hp = maxHp;
            return;
        }
        
        if (character is PlayerInfo info){
            maxHp = info.Max_HP + info.Bonus_hp + info.Bonus_percentage_hp;
            RegenHp();
        }
        
        UpdateHP_Bar();
    }

    private void RegenHp (){
        character.Health += ((PlayerInfo) character).HPRegen * Time.deltaTime;
        hp = character.Health;
    }
    
    public void UpdateHP_Bar ()
    {
        var percentageHp = hp / maxHp;
       
        if (fillImage)
            fillImage.fillAmount = percentageHp;
        
        UpdateHP_Text (percentageHp);
    }
    
    private void UpdateHP_Text (float currentHp){
        if (!hpText)
            return;
        
        hpText.text = $"{System.Math.Round(currentHp * 100f, 0)}/{maxHp}";
    }
    
    public void LoseHp (){
        character.Health -= 10;
        hp = character.Health;
    }

    public void OnLevelUp()
    {
        maxHp = character.Max_HP;
        UpdateHP_Bar();
    }
}

