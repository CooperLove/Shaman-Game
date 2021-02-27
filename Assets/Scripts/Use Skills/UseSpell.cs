using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseSpell : UseAbility, IPointerClickHandler
{
    [SerializeField] private SpellRequirements skillTreeComponent; 
    [SerializeField] private Spell spell;
    

    private void Awake() {
        cooldownBG = transform.GetChild(0).GetComponent<Image>();
    }

    public SpellRequirements SkillTreeComponent { get => skillTreeComponent; set => skillTreeComponent = value; }
    public Spell Spell { get => spell; set => spell = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            Debug.Log("Left click use spell");
            if (transform.parent.name == "Spellbar" && canUseSpell){
                Use();
            }
        }
    }

    public override void Use() {
        spell?.Use();
        StartCoroutine (SpellCooldown());
        cooldownBG.fillAmount = 1;
        updateBG = true;
    } 

    private IEnumerator SpellCooldown (){
        canUseSpell = false;
        yield return new WaitForSeconds(spell.Cooldown);
        canUseSpell = true;
    }

    private void Update (){
        if (cooldownBG == null || !updateBG)
            return;
        
        UpdateSpellCdImage();
    }

    private void UpdateSpellCdImage (){
        if (cooldownBG.fillAmount >= 0 && updateBG)
            cooldownBG.fillAmount -= 1.0f/ spell.Cooldown * Time.deltaTime;
        else{
            updateBG = false;
        }
    }

    public override void HideUI()
    {
        throw new System.NotImplementedException();
    }

    public override void Stop()
    {
        throw new System.NotImplementedException();
    }

    public override bool ToggleSkill()
    {
        throw new System.NotImplementedException();
    }
}
