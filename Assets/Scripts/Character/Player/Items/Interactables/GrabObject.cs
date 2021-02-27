using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : Interactable
{
    private Transform playerTransform;
    private Player player;
    private bool hitPlayer, playerDir;
    [SerializeField] private float grabbingVelocity = 0f;
    private float oldVelocity;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerTransform = Player.Instance.Transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitPlayer)
            return;

        // if (Input.GetKeyDown(KeyCode.E)){
        //     OnBeginInteraction();
        // }
        // if (Input.GetKeyUp(KeyCode.E)){
        //    OnEndInteraction();
        // }
    }

    public void OnTriggerEnter2D (Collider2D other){
        if (other.transform.tag.Equals("Player"))
            hitPlayer = true;
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            hitPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")){
            hitPlayer = false;
        }
    }

    public override void OnBeginInteraction()
    {
        var dir = playerTransform.position.x <= transform.position.x ? false : true;
        player.IsGrabbingObject = true;
        player.IsInteractingWith = true;
        player.SpriteRenderer.flipX = dir;
        oldVelocity = player.Velocity;
        player.Velocity = grabbingVelocity;
        transform.SetParent(playerTransform);
    }

    public override void OnEndInteraction()
    {
        player.IsGrabbingObject = false;
        player.IsInteractingWith = false;
        player.Velocity = oldVelocity;
        transform.SetParent(null);
    }
}
