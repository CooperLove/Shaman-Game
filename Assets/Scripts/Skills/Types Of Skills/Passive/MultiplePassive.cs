using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplePassive : MonoBehaviour
{
    public string passiveName;
    public List<PassiveValuePair> passives = new List<PassiveValuePair>();
    public bool learned = false;

    public void OnLearn (){
        foreach (PassiveValuePair passive in passives)
            passive.passive.OnLearn(passive.value);

        learned = true;
        GetComponent<Button>().interactable = false;
    }

    public string GetDescription (){
        string description = "";
        foreach (PassiveValuePair p in passives)
        {
            bool last = p.passive.Equals(passives[passives.Count-1]);
            description += $"+{p.value}"+(p.passive.isFlat ? "" : "%" )+$" {p.passive.Description}"+(last ? "" : "\n");
        }
        return description;
    }
}
