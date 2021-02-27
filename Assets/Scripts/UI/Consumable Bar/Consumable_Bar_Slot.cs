using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Consumable_Bar_Slot : MonoBehaviour, IPointerClickHandler
{
    private int slotIndex;
    public ConsumableBar consumableBar;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right){
            OpenConsumablesMenu();
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        slotIndex = transform.parent.GetSiblingIndex();
    }

    private void OpenConsumablesMenu (){
        consumableBar.Open(slotIndex);
    }

}
