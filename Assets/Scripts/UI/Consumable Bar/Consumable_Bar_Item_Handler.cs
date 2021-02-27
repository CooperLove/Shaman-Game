using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Consumable_Bar_Item_Handler : MonoBehaviour, IPointerClickHandler
{
    public ConsumableHandler handler;
    public ConsumableBar consumableBar;
    public Transform bar;
    public Transform menu;
    public TMP_Text qtdText;
    private int qtd;
    private int slot;

    public void OnLoad() {
        //Debug.Log($"OnLoad {gameObject.name} {handler.CurrentSlot} {gameObject.tag}");
        if (handler.CurrentSlot != -1 && gameObject.tag == "ConsumableInMenu"){
            OnSelectConsumable();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && gameObject.tag == "ConsumableInBar")
            Use();
        if (eventData.button == PointerEventData.InputButton.Left && gameObject.tag == "ConsumableInMenu")
            OnSelectConsumable();
        
        if (eventData.button == PointerEventData.InputButton.Right && gameObject.tag == "ConsumableInBar"){
            if (consumableBar.gameObject.activeInHierarchy)
                consumableBar.Close();
            else
                consumableBar.Open(slot);
        }
    }

    public void Use () {
        handler?.Use();
        if (handler != null){
            qtdText.text = handler.QuantityText.text;
            qtd = handler.Quantity;
        }
        if (qtd <= 0)
            Destroy(gameObject);
    }

    public void OnSelectConsumable (){
        //Debug.Log($"OnSelect({consumableBar.currentSlot}, {handler.CurrentSlot})");
        if (consumableBar.slot == -1 && handler.CurrentSlot == -1)
            return;

        int index = handler.CurrentSlot == -1 ? consumableBar.slot : handler.CurrentSlot;
        Transform slotTransform = bar.GetChild(index);
        
        if (slotTransform.childCount > 1){
            ChangeConsumables(slotTransform.GetChild(1));
            return;
        }

        gameObject.tag = "ConsumableInBar";

        transform.SetParent(slotTransform);
        slot = consumableBar.slot;
        handler.CurrentSlot = slot;
        ConfigureItemOnSelect();
        consumableBar.slot = -1;

        slotTransform.GetComponent<UseConsumable>().handler = handler;
        slotTransform.GetComponent<UseConsumable>().qtdText = qtdText;

        consumableBar.Close();
    }

    public void OnChangeConsumable (){
        if (consumableBar.slot == -1)
            return;

        gameObject.tag = "ConsumableInMenu";

        transform.SetParent(menu);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        consumableBar.slot = -1;
    }

    public void OnChangeConsumableToMenu (Transform t){
        if (consumableBar.slot == -1)
            return;

        t.gameObject.tag = "ConsumableInMenu";

        handler.CurrentSlot = -1;
        slot = -1;
        t.SetParent(menu);
        t.localScale = Vector3.one;
        t.localPosition = Vector3.zero;
    }

    private void OnChangeConsumableToBar (Transform t){
        if (consumableBar.slot == -1)
            return;

        t.gameObject.tag = "ConsumableInBar";

        Transform slotTransform = bar.GetChild(consumableBar.slot);
        transform.SetParent(slotTransform);
        slot = consumableBar.slot;
        ConfigureItemOnSelect();

        slotTransform.GetComponent<UseConsumable>().handler = handler;
        slotTransform.GetComponent<UseConsumable>().qtdText = qtdText;

        consumableBar.slot = -1;
    }

    private void ChangeConsumables (Transform consumableInBar){
        OnChangeConsumableToMenu(consumableInBar);
        OnChangeConsumableToBar(transform);
        consumableBar.Close();
    }

    private void ConfigureItemOnSelect () {
        //Muda o tamanho da imagem para ficar de acordo com o tamanho do slot
        GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

        //Configura o objeto da quantidade de consumiveis para ficar no modo Scretch e centralizado
        // RectTransform rect = transform.GetChild(0).GetComponent<RectTransform>();
        // rect.anchoredPosition = new Vector2(0.5f, 0.5f);
        // rect.anchorMin = new Vector2 (0,0);
        // rect.anchorMax = new Vector2 (1,1);
        // rect.sizeDelta = new Vector2 (0,0);

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

}
