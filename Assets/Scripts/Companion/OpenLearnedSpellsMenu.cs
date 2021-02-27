using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenLearnedSpellsMenu : MonoBehaviour, IPointerClickHandler
{
    public Transform spellbar;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            Debug.Log("Left click");
            //TODO use spell
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right){
            Debug.Log("Right click");
            if (spellbar?.childCount >= Player.Instance.CompanionObj.MaxNumberOfSpells){
                return;
            }
            LearnedSpells.Instance.gameObject?.SetActive(!LearnedSpells.Instance.gameObject.activeInHierarchy);
            if (gameObject.tag != "CharacterMenu")
                LearnedSpells.Instance.SwitchSpells.spell01 = gameObject;
        }
    }
}
