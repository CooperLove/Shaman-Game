using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchSpell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform spellbar;

    public Transform Spellbar { get => spellbar; set => spellbar = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            //Debug.Log("Left click switch");
            if (LearnedSpells.Instance.SwitchSpells.spell01 == null && spellbar?.childCount < 5){
                gameObject.transform.SetParent(Spellbar);
            }
            LearnedSpells.Instance.SwitchSpells.spell02 = gameObject;
            LearnedSpells.Instance.SwitchSpells.OnSwitchSpell();
            LearnedSpells.Instance.UseSpellHelper(gameObject);
            //LearnedSpells.Instance.gameObject?.SetActive(false);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right){
            Debug.Log("Right click");
        }
    }

    
}
