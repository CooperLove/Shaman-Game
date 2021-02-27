using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Player player = null;
    Rigidbody2D playerRb = null;
    [SerializeField] private float interval = 0;
    [SerializeField] private Vector2 force = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerRb = Player.Instance.Rb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDash () {
        playerRb.AddForce(new Vector2 (player.IsFacingRight ? force.x : -force.x, force.y), ForceMode2D.Force);
        player.StartDash(interval);
    }
}
