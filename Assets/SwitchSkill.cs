using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchSkill : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform spellbar;

    public Transform Spellbar { get => spellbar; set => spellbar = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
            {
                Debug.Log("Left click switch");
                var skills = PlayerLearnedSkills.Instance;
            
                skills.SwitchSkills.spell02 = gameObject;
                skills.SwitchSkills.OnSwitchSpell();
                //skills.UseSpellHelper(gameObject);
                //LearnedSpells.Instance.gameObject?.SetActive(false);
                break;
            }
            case PointerEventData.InputButton.Middle:
                Debug.Log("Middle click");
                break;
            case PointerEventData.InputButton.Right:
                Debug.Log("Right click");
                break;
            default:
                break;
        }
    }
}
