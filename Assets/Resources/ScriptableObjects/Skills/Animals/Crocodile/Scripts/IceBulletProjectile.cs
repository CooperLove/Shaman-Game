using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBulletProjectile : MonoBehaviour
{
    public IceBullet iceBullet;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy" && iceBullet != null){
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy == null)
                return; 
                
            enemy.TakeDamage(iceBullet == null ? 0 : iceBullet.damagePerProjectile);
            Destroy(gameObject);
        }
    }
}
