using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMonoBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
        shield = GameObject.Find("Crocodile Shield");
    }

    public void Shield (float duration){
        if (shield == null)
            return;
            
        GameObject s = Instantiate(shield, shield.transform.position, shield.transform.rotation);
        s.transform.SetParent(Player.Instance.transform);
        s.transform.localPosition = Vector3.zero;
        Destroy(s, duration);
    }
}
