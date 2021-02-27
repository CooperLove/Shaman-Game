using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UseSkill : UseAbility, IPointerClickHandler
{
    [SerializeField] private UpgradableSkill skill;
    [SerializeField] private TMP_Text cdText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text keyText;
    public float cd;

    public UpgradableSkill Skill { get => skill; set => skill = value; }
    public TMP_Text CostText { get => costText; set => costText = value; }
    public TMP_Text KeyText { get => keyText; set => keyText = value; }
    public TMP_Text CdText { get => cdText; set => cdText = value; }
    public void SetUp(SerializebleUseSKill values)
    {
        this.skill = values.skill;
        this.CdText = values.cdText;
        this.CostText = values.costText;
        this.KeyText = values.keyText;
    }

    public SerializebleUseSKill GetSerializable()
    {
        var output = new SerializebleUseSKill(skill, cdText, costText, keyText);
        return output;
    }
    private void Awake() {
        cooldownBG = transform.GetChild(2).GetComponent<Image>();
        canUseSpell = true;
        if (skill is ChainLike like)
            like.SetUseSkill(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            //Debug.Log("Left click use skill");
            if (transform.parent.name == "Slot "+transform.parent.transform.GetSiblingIndex() && canUseSpell){
                Use();
            }
        }
    }

    public override void Use() {
        //Debug.Log(canUseSpell ? $"Usando {skill.SkillName}" : $"Skill {skill.SkillName} em cd");
        if (!canUseSpell || skill == null || skill is Skill_OverTime)
            return;
        
        if (Skill is Active active && active.OnUse()){
            OnUseSkill();
        }
    } 

    public override void Stop() {
        if (skill is Skill_OverTime overTime)
            overTime.Stop();
        OnUseSkill ();
    }

    public override bool ToggleSkill()
    {
        if (skill is Toggle toggleSkill){
            if (toggleSkill.Active){
                if (toggleSkill.highlight == null)
                    toggleSkill.highlight = Highlight();

                toggleSkill.highlight?.SetActive(true);
                return true;
            }
        }

        ((Toggle)skill).highlight?.SetActive(false);
        return false;
   }

    private void OnUseSkill () {
        if (skill is Toggle){
            if(ToggleSkill())
                return;
        }
        if (skill is ChainLike && !((ChainLike)skill).enterOnCooldown)
            return;

        cd = ((Active) skill).Cooldown;
        CdText.gameObject.SetActive(true);
        cooldownBG.gameObject.SetActive(true);
        cooldownBG.fillAmount = 1;
        updateBG = true;
        StartCoroutine (SpellCooldown());
    }

    private IEnumerator SpellCooldown (){
        canUseSpell = false;
        yield return new WaitForSeconds(((Active) Skill).Cooldown);
        canUseSpell = true;
        updateBG = false;
        CdText.gameObject.SetActive(false);
    }

    private void Update (){
        if (cooldownBG == null || !updateBG)
            return;
        
        UpdateSpellCdImage();
    }

    private void UpdateSpellCdImage (){
        if (cooldownBG.fillAmount >= 0 && updateBG){
            cooldownBG.fillAmount -= 1.0f/ ((Active) Skill).Cooldown * Time.deltaTime;
            cd -= Time.deltaTime;
            CdText.text = System.Math.Round(cd, 1).ToString();
        }
        else{
            updateBG = false;
            
        }
    }

    public override void HideUI (){
        //KeyText.gameObject.SetActive(false);
        //CostText.gameObject.SetActive(false);
        CdText.gameObject.SetActive(false);
        cooldownBG.gameObject.SetActive(false);
    }

    public GameObject Highlight (){
        GameObject g = Resources.Load("Prefabs/Combat/Highlight") as GameObject;
        GameObject highlight = Instantiate(g, transform.position, g.transform.rotation);
        highlight.transform.SetParent(transform.parent);
        highlight.transform.localScale = Vector3.one;
        highlight.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        return highlight;
    }

    
}

[System.Serializable]
    public class SerializebleUseSKill {
        public UpgradableSkill skill;
        public TMP_Text cdText;
        public TMP_Text costText;
        public TMP_Text keyText;

    public SerializebleUseSKill(UpgradableSkill skill, TMP_Text cdText, TMP_Text costText, TMP_Text keyText)
    {
        this.skill = skill;
        this.cdText = cdText;
        this.costText = costText;
        this.keyText = keyText;
    }
}