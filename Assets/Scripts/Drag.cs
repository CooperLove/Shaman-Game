using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rect = null;
    private CanvasGroup canvasGroup = null; 
    [SerializeField] private Canvas canvas = null;
    

    void Awake() {
        rect = GetComponent<RectTransform>();  
        canvasGroup = GetComponent<CanvasGroup>();  
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
	}

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rect.anchoredPosition += eventData.delta/canvas.scaleFactor;
       
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
    

    
}
