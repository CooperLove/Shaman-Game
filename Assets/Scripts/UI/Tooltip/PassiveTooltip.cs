using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PassiveTooltip : Tooltip
{
    [SerializeField] private TMP_Text passiveName = null;
    public float fixedWidth = 450;

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

    public void SetPassiveName (MultiplePassive passive){
        passiveName.text = passive.learned ? $" <color=green>{passive.passiveName}</color>" : passive.passiveName;
    }
    public override void Show(string s)
    {
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

        float height =  numLinesAfter.Length > numLinesBefore.Length ? numLinesAfter.Length * 25 + 30 : numLinesBefore.Length * 25 + 30;
        
        rectTransform.sizeDelta = new Vector2(fixedWidth, height);
        //Debug.Log($"Preferred size: {rectTransform.sizeDelta} {passiveName.preferredWidth} {preferredAfter} => "+
        //            $"Lines [{numLinesBefore.Length},{numLinesAfter.Length}]");
    }
}
