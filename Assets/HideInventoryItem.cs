using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInventoryItem : MonoBehaviour
{
    private void OnBecameInvisible() {
        Debug.Log("Invisible "+gameObject.name);
    }
    private void OnBecameVisible() {
        Debug.Log("Visible "+gameObject.name);
    }
}
