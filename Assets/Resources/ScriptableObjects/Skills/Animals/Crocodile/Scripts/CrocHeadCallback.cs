using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocHeadCallback : MonoBehaviour
{
    public GameObject head;
    public Transform point;
    public float velocity;
    private void Start() {
        head = GameObject.Find("CrocodileHead");
    }
    private void OnParticleSystemStopped() {
        if (head == null)
            return;

        GameObject g = Instantiate(head, point.position, head.transform.rotation);
        Projectile_Shoot_Test projectile = g.AddComponent<Projectile_Shoot_Test>();
        projectile.shouldDestroy = true;
        projectile.destroyTimer = 3f;
        projectile.velocity = velocity;
        Destroy(gameObject, 1);
    }
}
