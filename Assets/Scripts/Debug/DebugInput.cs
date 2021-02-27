using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class DebugInput
{
    private static GameObject template = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name.Equals("Input Text Template"));
    private static GameObject panel = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name.Equals("Input Panel"));
    private static Transform contentPanel = Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(x => x.name.Equals("Input Content Panel"));

    public static void AddInput (string inputText, string inputName){
        var input = Object.Instantiate(template, Vector3.zero, template.transform.rotation);

        var text = $"<color=green><b>{inputText}</b></color> - {inputName}";
        input.GetComponent<TMP_Text>().SetText(text);
        input.name = inputName;

        input.transform.SetParent(contentPanel);
        input.transform.localScale = Vector3.one;
        input.SetActive(true);

        if (contentPanel.childCount >= 15){
            for (int i = 0; i < 5; i++)
            {
                contentPanel.GetChild(0).SetParent(null);
                Object.Destroy(contentPanel.GetChild(0).gameObject);
            }
        }
    }

    public static void ShowPanel (bool show){
        panel.SetActive(show);
    }


}
