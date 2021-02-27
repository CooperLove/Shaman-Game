using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool adaptRotation;
    public bool destroyOnGround;
    public Vector3 speed;
    public Vector3 rotation;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        if (adaptRotation){
            speed.z = player.IsFacingRight ? speed.z : -speed.z;
            rotation.y = player.IsFacingRight ? rotation.y : 0;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
        if (destroyOnGround && player.OnGround)
            Destroy(gameObject);
    }
}
