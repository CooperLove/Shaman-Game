using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionTree : MonoBehaviour
{
    private static EvolutionTree instance;

    public static EvolutionTree Instance { get => instance;}

    EvolutionTree (){
        if (instance == null)
            instance = this;
    }

    public void Open () => gameObject.SetActive(true);
    public void Close () => gameObject.SetActive(false);
}
