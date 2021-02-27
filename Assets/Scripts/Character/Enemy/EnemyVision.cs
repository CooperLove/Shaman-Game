using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private Enemy enemy = null;
    private Transform playerTransform = null;

    private void Awake (){
        enemy = transform.parent.GetComponent<Enemy>();
        playerTransform = Player.Instance.transform;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log($"Other => {other.name}");
        if (other.tag.Equals("Player") && !enemy.Target){
            enemy.OnVisionTrigger();
        }
    }
}
