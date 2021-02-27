using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour , IDropHandler 
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null){
            eventData.pointerDrag.transform.SetParent(this.transform);
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta * 0.75f;

        }
    }
}
