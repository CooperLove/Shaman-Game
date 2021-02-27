using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTooltipHandler : TooltipHandler
{

    public SimpleTooltip simpleTooltip = null;
    
    [TextArea(1,5)] public string description;

    public override void OnPointerEnter (PointerEventData eventData) {
        //Debug.Log("Pointer Enter "+this.name);
        simpleTooltip?.Show(description);
    }

    public override void OnPointerExit (PointerEventData eventData) {
        //Debug.Log("Pointer Exit "+this.name);
        simpleTooltip?.Hide();
    }
}
