using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter "+this.name);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit "+this.name);
    }
}
