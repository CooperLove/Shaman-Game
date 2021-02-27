using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnhancementHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemHandler handler;

    public ItemHandler Handler { get => handler; set => handler = value; }

    private void Start() {
        Handler = GetComponent<ItemHandler>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right &&
            Enhancement.Instance != null && 
            Enhancement.Instance.gameObject.activeInHierarchy)
        {
            //Debug.Log("Right click enhance");
            Enhancement.Instance.ChangeImages(Handler);
            
        }   
    }
}
