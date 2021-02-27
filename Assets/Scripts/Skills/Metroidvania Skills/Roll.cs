using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    private Player player;
    private PlayerInfo playerInfo;

    [SerializeField] private float staminaToRoll = 0f;
    [SerializeField] private float rollDuration = 0f;
    [SerializeField] private float rollDistance = 0f;
    [SerializeField] private Vector3 rollOffset = new Vector3();

    private LayerMask wallLayer = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerInfo = Player.Instance.playerInfo;

        wallLayer = 1 << 14;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.IgnoreInputs())
            return;
            
        // ROLL
        if (Input.GetKeyDown(KeyCode.LeftControl) && player.OnGround)
            OnRoll ();
    }

    private void OnRoll (){
        if (playerInfo.Stamina < staminaToRoll)
            return;

        player.IsRolling = true;
        Debug.Log($"Rolling");
        player.AnimationsScript.OnEnterRoll ();
        var position = player.Transform.position;
        var ray = Physics2D.Raycast(position + rollOffset, player.IsFacingRight ? Vector2.right : Vector2.left, rollDistance, wallLayer);
        var point = ray.collider != null ? new Vector2(ray.point.x, position.y) : new Vector2 (position.x + (player.IsFacingRight ? rollDistance : -rollDistance), position.y);
        
        StartCoroutine( player.Dash (point, rollDuration) );
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position + rollOffset, transform.position + new Vector3(rollDistance, rollOffset.y, 0));
    }
}
