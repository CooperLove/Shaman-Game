using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCrocodile : MonoBehaviour
{
    private GameObject passiveBar = null;
    [SerializeField] private float currentEnergy;
    private float maxEnergy;

    public float CurrentEnergy { 
        get => currentEnergy; 
        set {
            currentEnergy = value;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        //passiveBar = GetComponent<CrocodileAttacks>().passiveBar;
        maxEnergy = 100;
    }

    public void Use (float amount){
        CurrentEnergy += amount;
        float perc = currentEnergy/maxEnergy;

        if (passiveBar != null){
            passiveBar.transform.GetChild(0).localScale = new Vector3 (1, perc, 1);
        }
    }
}
