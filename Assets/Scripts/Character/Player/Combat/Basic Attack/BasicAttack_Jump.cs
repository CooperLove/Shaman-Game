using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack_Jump : MonoBehaviour
{
    [SerializeField] private Vector3 pos = new Vector3();
    [SerializeField] private Vector3 size = new Vector3();
    [SerializeField] private float speed = 0f;
    [SerializeField] private float offset = 0f;
    [SerializeField] private float minDist = 0f;
    private Player player;
    [SerializeField] private LayerMask enemyLayer;
    private BasicAttack_Handler jump;
    private BasicAttack jumpAA;
    private Rigidbody2D target;

    private GhostController ghostController;
    
    private void Start() {
        enemyLayer = 1 << 9;
        player = Player.Instance;
        jump = player.gameObject.GetComponent<BasicAttack_Handler>();
        jumpAA = GetComponent<BasicAttack>();
        pos.x = player.IsFacingRight ? pos.x : -pos.x;
        offset = player.IsFacingRight ? -offset : offset;
        ghostController = Player.Instance.GetComponent<GhostController>();
        GetTarget();
    }
    private void GetTarget (){
        var col = Physics2D.OverlapBox(transform.position + pos, size, 0, enemyLayer);
        if (col == null) 
            return;
        
        //Debug.Log($"Enemy {col.name}");
        target = col.GetComponent<Enemy>().Rb;
        var end = col.transform.position + new Vector3(offset, 0, 0);
        //Debug.DrawLine(transform.position, end, Color.red, 3f);
        StartCoroutine(GoToTarget(end));
    }

    private IEnumerator GoToTarget (Vector3 target){
        player.IgnoreCommands = true;

        ghostController.ghostActive = true;

        player.Rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        while (Vector2.Distance(player.transform.position, target) >= minDist){
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, speed * Time.fixedDeltaTime);
            yield return null;
        }

        ghostController.ghostActive = false;
        player.IgnoreCommands = false;

        //StartCoroutine(jump.AirAttackHandler(jumpAA.AirDuration));
        StartCoroutine(StunTarget(jumpAA.AirDuration));
    }

    private IEnumerator StunTarget (float airDuration){
        // if (target == null)
        //     yield break;

        target.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(airDuration);
        target.constraints = RigidbodyConstraints2D.FreezeRotation;
        target.velocity += Physics2D.gravity;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position + pos, size);
    }
}
