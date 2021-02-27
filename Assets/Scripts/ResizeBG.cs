using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBG : MonoBehaviour
{
    public RectTransform rectTransform, canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform.sizeDelta = canvasRect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(canvasRect.sizeDelta+" "+rectTransform.sizeDelta);
        rectTransform.sizeDelta = canvasRect.sizeDelta;
    }
}
