using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggle : Active
{
    [SerializeField] private bool active = false;
    public GameObject highlight = null;

    public bool Active { get => active; set => active = value; }

    public abstract void TurnOn ();
    public abstract void TurnOff ();

    private void OnDisable() {
        highlight = null;
    }
    
}
