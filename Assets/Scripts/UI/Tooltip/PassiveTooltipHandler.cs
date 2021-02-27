using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveTooltipHandler : TooltipHandler
{
    public PassiveTooltip passiveTooltip = null;
    [SerializeField] private MultiplePassive passive = null;
    
    [TextArea(1,5)] public string description;

    public override void OnPointerEnter (PointerEventData eventData) {
        //Debug.Log("Pointer Enter "+this.name);
        passiveTooltip.SetPassiveName(passive);
        passiveTooltip?.Show(passive.GetDescription());
    }

    public override void OnPointerExit (PointerEventData eventData) {
        //Debug.Log("Pointer Exit "+this.name);
        passiveTooltip?.Hide();
    }
}
