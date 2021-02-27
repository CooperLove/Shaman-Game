using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skull : MonoBehaviour
{
    [SerializeField, Range(0,3)] private int offset = 2;
    [SerializeField] private bool fill = false, resetFill = false;

    [SerializeField, Range(0,7.99f)] private float value = 0;
    [SerializeField] private float waitTime = 0;
    [SerializeField] private List<Image> skulls = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fill();
        if (fill){
            fill = false;
            StartCoroutine (Fullfill());
        }
        if (resetFill){
            resetFill = false;
            StartCoroutine (ResetFill());
        }
    }

    private void Fill (){
        Image child = skulls[(int) value].transform.GetChild(0).GetComponent<Image>();
        child.fillAmount = value - (int) value;
        Color s = child.color;
        if (s.a <= 0.05f)
            s = new Color (s.r, s.g, s.b, 0.0f);

        float alpha = 0.25f * offset;
        float alphaValue = 25f;
        float fillValue  = 75f;
        for (int i = 1; i < skulls.Count; i++)
        {
            Color c = skulls[i].color;
            alpha = (skulls[i-1].transform.GetChild(0).GetComponent<Image>().fillAmount/100f) * fillValue + (skulls[i-1].color.a/100f) * alphaValue;
            skulls[i].color = new Color (c.r, c.g, c.b, alpha);
            c = skulls[i].color;

            if (c.a <= 0.05f)
                skulls[i].color = new Color (c.r, c.g, c.b, 0.0f);
        }
    }

    public IEnumerator Fullfill (){
        while (value < 7.99f){
            yield return new WaitForSeconds(waitTime);
            value += 0.02f;
        }
        value = 7.99f;
    }

    public IEnumerator ResetFill (){
        while (value > 0){
            yield return new WaitForSeconds(waitTime);
            value -= 0.02f;
        }
    }
}
