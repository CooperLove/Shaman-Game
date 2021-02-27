using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Test : MonoBehaviour
{
    public GameObject projectile;
    public Transform pos;
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        return;
        // if (Input.GetMouseButtonDown(0) && projectile != null){
        //     GameObject g = Instantiate (projectile, pos.position, projectile.transform.rotation);
        //     g.TryGetComponent<Projectile_Shoot_Test>(out Projectile_Shoot_Test shoot);
        //     if (shoot != null) {
        //         shoot.shouldDestroy = true;
        //         shoot.destroyTimer = 2f;
        //         shoot.velocity = velocity;
        //     }
        // }
    }
}
