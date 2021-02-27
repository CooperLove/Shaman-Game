using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltipHandler : TooltipHandler
{
    [SerializeField] private SkillTooltip skillTooltip = null;
    [SerializeField] private Active skill = null;
    private void Start() {
        TryGetComponent<SkillTreeComponent>(out var component);
        if (component)
            skill = (Active) component.Skill;
        else{
            TryGetComponent<UseSkill>(out var useSkill);
            if (useSkill)
                skill = (Active) useSkill.Skill;
        }
    }
    public override void OnPointerEnter (PointerEventData eventData) {
        if (!skillTooltip) 
            return;
        skillTooltip.Show(skill);
    }

    public override void OnPointerExit (PointerEventData eventData) {
        //Debug.Log("Pointer Exit "+this.name);
        skillTooltip?.Hide();
    }
}
