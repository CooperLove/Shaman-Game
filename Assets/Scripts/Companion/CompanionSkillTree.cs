using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSkillTree : MonoBehaviour
{
    //TODO Spells effects
    private static CompanionSkillTree instance;

    public static CompanionSkillTree Instance { get => instance; set => instance = value; }

    CompanionSkillTree (){
        if (Instance == null)
            Instance = this;
    }
    public void Open () => this.gameObject.SetActive(true);
    public void Close () => this.gameObject.SetActive(false);
}
