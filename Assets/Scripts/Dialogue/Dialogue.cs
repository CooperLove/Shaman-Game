using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class Dialogue 
{
    public string name;
    public GameObject nameDisplay;
    public TMP_Text nameText;

    [TextArea(3,10)]
    public string[] sentences;

}
