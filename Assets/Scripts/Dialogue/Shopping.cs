using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shopping : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    public GameObject icon;
    public Transform canvasTransform;
    private void Start() {
        button = GetComponent<Button>();
    }
    private void OnMouseEnter() {
        Debug.Log("Mouse over button!");
    }
    private void OnMouseOver() {
        Debug.Log("Over");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject.name);
        icon.SetActive(true);
        icon.transform.SetParent(this.transform);
        icon.transform.localPosition = new Vector3(-80,0,0);
        button.Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //icon.transform.SetParent(canvasTransform);
        //icon.SetActive(false);
    }
}
