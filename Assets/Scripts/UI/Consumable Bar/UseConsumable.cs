using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UseConsumable : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    public ConsumableHandler handler = null;
    public TMP_Text qtdText;
    // Start is called before the first frame update
    private void Start()
    {
        key = (KeyCode) (282 + transform.GetSiblingIndex());
        // F1 = 282 ... F5 = 286
        //Debug.Log($"{(int)KeyCode.F1} {(int)KeyCode.F2} {(int)KeyCode.F3}");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(key) || !handler || !qtdText) 
            return;
        
        handler.Use();
        qtdText.text = handler.Quantity.ToString();
        
        if (handler.Quantity != 0) 
            return;
        
        handler = null;
        qtdText = null;
        Destroy(transform.GetChild(1).gameObject);
    }
}
