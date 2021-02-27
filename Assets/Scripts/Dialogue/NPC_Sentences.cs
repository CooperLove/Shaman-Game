using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC_Sentences {
    public Sprite image;
    public string name;
    public List<string> sentences = new List<string>();
    public NPC_Sentences next;
}