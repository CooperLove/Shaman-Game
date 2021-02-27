using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hit player "+other.name+" "+other.tag);
        if (other.gameObject.tag.Equals("Player")){
            other.GetComponent<Player>().TakeDamage(10);
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
        }
    }
}
