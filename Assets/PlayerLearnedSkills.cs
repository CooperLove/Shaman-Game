using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;

public class PlayerLearnedSkills : MonoBehaviour
{
#region Variables
    [SerializeField] private static PlayerLearnedSkills instance  = null;
    [SerializeField] private GameObject skillTemplate = null;
    [FormerlySerializedAs("skillbar")] 
    [SerializeField] private Transform skillBar = null;
    [SerializeField] private Transform passiveObj = null;
    [SerializeField] private SwitchSkills switchSkills = null;
    [SerializeField] private GridLayoutGroup gridLayoutGroup = null;
    [SerializeField] private float sizePerRow = 80f;
    private RectTransform rect;
    private RectTransform consumableBar;
    public Transform slotTransform;

#region Properties
    public static PlayerLearnedSkills Instance { get => instance; set => instance = value; }
    public SwitchSkills SwitchSkills { get => switchSkills;
        private set => switchSkills = value; }
    public Transform SkillBar { get => skillBar; set => skillBar = value; }
#endregion
#endregion

    private PlayerLearnedSkills (){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Awake() {
        rect = GetComponent<RectTransform>();
        SwitchSkills = new SwitchSkills();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        consumableBar = Resources.FindObjectsOfTypeAll<RectTransform>().Where(x => x.name.Equals("Consumable Bar")).ToList()[0];
        skillBar = Resources.FindObjectsOfTypeAll<RectTransform>().Where(x => x.name.Equals("Skillbar - Skills")).ToList()[0];
        var size = skillBar.childCount;
    }

    public void Open (){
        gameObject.SetActive(!gameObject.activeInHierarchy);
        GameStatus.isUsingSomeMenu = gameObject.activeInHierarchy;
        Resize();
    }
    public void Close ()
    {
        GameStatus.isUsingSomeMenu = false;
        gameObject.SetActive(false);
        UpdateConsumableBarPos();
    }

    private void UpdateConsumableBarPos (){
        if (!rect || !consumableBar)
            return;

        var height = rect.anchoredPosition.y + (gameObject.activeInHierarchy ? rect.sizeDelta.y : 0);
        consumableBar.anchoredPosition = new Vector3(consumableBar.anchoredPosition.x, height, 0);
    }
    
    private void OnDisable() {
        slotTransform = null;
    }
    

    public void _OnLearnSkill (UpgradableSkill s) => OnLearnSkill(s);
    public void _OnLearnSkill (OneTimePassive s) => s.OnLearn();
    public void OnUpgradeSkill (UpgradableSkill s) => s.OnUpgrade();

    public GameObject OnLearnSkill (UpgradableSkill s)
    {
        if (!s)
            return null;
        
        Debug.Log($"Aprendendo {s.SkillName}");
        s.OnLearn();

        // Se for uma skill passiva não tem necessidade de colocá-la na barra de skill
        if (s is Passive){
            passiveObj.GetComponent<Image>().sprite = s.Sprite;
            return null;
        }

        var transform1 = transform;
        var skill = Instantiate (skillTemplate, transform1.position, transform1.rotation);
        skill.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        skill.transform.SetParent(transform);
        skill.AddComponent<Spell_Input>();
        skill.transform.localScale = Vector3.one;
        skill.TryGetComponent<UseSkill>(out var h);
        if (h != null){
            if (s is Active skillActive)
                h.CostText.text = skillActive.MpCost.ToString(CultureInfo.InvariantCulture);
            h.Skill = s;
            h.HideUI();
        }
        skill.name = s.SkillName;
        skill.SetActive(true);

        if (gameObject.activeInHierarchy)
            Resize();
        
        return skill;
    }
    
    public int GetSlotIndex (Active s) {
        if (s.PreferredKeys.Count == 0)
            return slotTransform == null ? -1 : slotTransform.GetSiblingIndex();
            
        switch (s.PreferredKeys[0])
        {
            case KeyCode.F1: return 0;
            case KeyCode.F2: return 1;
            case KeyCode.F3: return 2;
            case KeyCode.F4: return 3;
            default: return slotTransform == null ? -1 : slotTransform.GetSiblingIndex();
        }
    } 
    
    public void Resize () {
        var myRect = GetComponent<RectTransform>();

        if (!myRect || !gridLayoutGroup)
            return;

        var trans = transform;
        var childCount = trans.childCount;
        var numRows = childCount / 5;
        //Debug.Log($"Rows: {numRows} => {childCount % 5}");
        myRect.sizeDelta = new Vector2(myRect.sizeDelta.x, gridLayoutGroup.padding.top 
                                                        + (numRows * gridLayoutGroup.spacing.y) 
                                                        + (numRows+(childCount % 5 != 0 ? 1 : 0)) 
                                                        * sizePerRow);

        UpdateConsumableBarPos();
    }
    
}

[System.Serializable]
public class SwitchSkills {
    public GameObject spell01;
    public GameObject spell02;

    public void OnSwitchSpell (){
        if (spell01 != null && spell02 != null){
            SwitchAbilites();
        }else if (spell02 != null){
            PutAbilityOnSkillbar();   
        }
    }

    private void SwitchAbilites (){
        var parent = spell01.transform.parent;
        var p = parent;

        var spell01Index = spell01.transform.GetSiblingIndex();
        var spell02Index = spell02.transform.GetSiblingIndex();

        spell01.AddComponent<SwitchSkill>().Spellbar = parent;
        spell02.AddComponent<OpenLearnedSkillsMenu>();

        Object.Destroy(spell01.GetComponent<OpenLearnedSkillsMenu>());
        Object.Destroy(spell02.GetComponent<SwitchSkill>());
        Object.Destroy(spell01.GetComponent<Spell_Input>());

        spell01.transform.SetParent(spell02.transform.parent);
        spell02.transform.SetParent(p);

        //Switch the position on their parent
        spell01.transform.SetSiblingIndex(spell02Index);
        spell02.transform.SetSiblingIndex(spell01Index);

        spell02.transform.localPosition = Vector3.zero;

        spell01.TryGetComponent<UseSkill>(out var u);
        if (u != null){
            u.CostText.gameObject.SetActive(false);
        }
        spell02.TryGetComponent<UseSkill>(out var h);
        //int key = spell02.transform.GetSiblingIndex()-1 + 49;
        var input = spell02.AddComponent<Spell_Input>();
        var key = input.GetKey(spell02.transform.GetSiblingIndex()-1);

        input.SetKey(key);
        SetSkill(spell02.transform.parent.GetSiblingIndex(), h);

        if (h != null){
            h.CostText.gameObject.SetActive(true);
        }

        ResetAndResize();
    }

    private void PutAbilityOnSkillbar (){
        spell02.TryGetComponent<UseSkill>(out var h);
        if (h != null)
            h.CostText.gameObject.SetActive(true);

        if (PlayerLearnedSkills.Instance.slotTransform != null){
            spell02.transform.SetParent(PlayerLearnedSkills.Instance.slotTransform);
            spell02.transform.localPosition = Vector3.zero;
        }else {
            var index = GetIndexOfFirstFreeSlot();
            if (index != -1){
                SetSkillOnSlot(index);
                var rect = spell02.transform.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector3.zero;
                rect.sizeDelta = new Vector2(0, 0);
            }
            SetSkill(index, h);
        }
        spell02.AddComponent<OpenLearnedSkillsMenu>();
        Object.Destroy(spell02.GetComponent<SwitchSkill>());

        ResetAndResize();
    }

    private void SetSkillOnSlot (int slotIndex) => spell02.transform.SetParent(PlayerLearnedSkills.Instance.SkillBar.GetChild(slotIndex));

    private int GetIndexOfFirstFreeSlot (){
        var skillbar = PlayerLearnedSkills.Instance.SkillBar;
        var size = skillbar.childCount;

        for (int i = 0; i < size; i++){
            if (skillbar.GetChild(i).childCount == 0)
                return i;
        }

        return -1;
    }
    private void ResetAndResize (){
        spell01 = null;
        spell02 = null;
        PlayerLearnedSkills.Instance.Resize();
        PlayerLearnedSkills.Instance.Close();
    }

    private int SetSkill (int index, UseSkill s){
        var pi = Player.Instance.playerInfo;
        //Debug.Log($"Skill {index} agr é {s.Skill.SkillName}");
        var handlerSkill = (Active) s.Skill;
        switch (index)
        {
            case 0: 
                pi.Skill01 = s;
                return 0;
            case 1: 
                pi.Skill02 = s;
                return 1;
            case 2: 
                pi.Skill03 = s;
                return 2;
            case 3: 
                pi.Skill04 = s;
                return 3;
        }
        return -1;
    }
}
