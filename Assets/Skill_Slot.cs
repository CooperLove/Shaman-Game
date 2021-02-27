using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject skill = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right || transform.parent.name != "Skills")
            return;
        
        var playerLearnedSkills = PlayerLearnedSkills.Instance;
        if (skill)
            playerLearnedSkills.SwitchSkills.spell01 = skill;
        
        playerLearnedSkills.slotTransform = transform;
        playerLearnedSkills.Open();
    }
}
