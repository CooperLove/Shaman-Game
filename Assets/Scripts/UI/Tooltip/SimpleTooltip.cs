using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleTooltip : Tooltip
{
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    public override void Show(string s)
    {
        if (!Options.showTooltips)
            return;
            
        Resize(s);
        gameObject.SetActive(true);
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Resize(string s)
    {
        string[] numLinesBefore = descText.text.Split('\n');
        Vector2 preferredBefore = descText.GetPreferredValues();
        descText.text = s;
        string[] numLinesAfter = descText.text.Split('\n');
        Vector2 preferredAfter = descText.GetPreferredValues();

        float height =  numLinesAfter.Length > numLinesBefore.Length ? numLinesAfter.Length * 25 + 20 : numLinesBefore.Length * 25 + 20;
        
        rectTransform.sizeDelta = new Vector2(preferredAfter.x, height);
        //Debug.Log($"Preferred size: {rectTransform.sizeDelta} {preferredBefore} {preferredAfter} => "+
        //            $"Lines [{numLinesBefore.Length},{numLinesAfter.Length}]");
    }

}
