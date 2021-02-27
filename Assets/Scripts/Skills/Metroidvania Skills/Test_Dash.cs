using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Test_Dash : MonoBehaviour
{
    private const float DOUBLE_CLICK_TIME = 0.2f;
    private const float DASH_INTERVAL = 2;
    private float lastPressTime = 0;
    private float timeSinceLastPress = 0;
    private float lastPressDash = 0;
    private float timeSinceLastDash = 0;
    [SerializeField] private float dashTimer = 0;
    [SerializeField] private float jVel = 0;
    [SerializeField] private Player player;
    private Rigidbody2D playerRb;
    [SerializeField] public Vector3 distance;
    [SerializeField] public Vector3 pos;
    [SerializeField] public Transform pPos;
    [SerializeField] private float dashSpeed, dashForce, df, forceAfterStop, fas, stopTime, stopMultiplier;
    [SerializeField] private int keyDashD = 0, keyDashA = 0;
    [SerializeField] private KeyCode lastKeyPressed;
    [SerializeField] private LayerMask layerMask = 0;

    public float JVel { get => jVel; set => jVel = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float DashForce { get => dashForce; set => dashForce = value; }
    public float Df { get => df; set => df = value; }
    public float ForceAfterStop { get => forceAfterStop; set => forceAfterStop = value; }
    public float Fas { get => fas; set => fas = value; }
    public float StopTime { get => stopTime; set => stopTime = value; }
    public float StopMultiplier { get => stopMultiplier; set => stopMultiplier = value; }
    Dash dash;
    private bool canDash;

    private float delta;
    public Test_Dash (){
        dashTimer = 0.3f;
        DashSpeed = 85;
        ForceAfterStop = 45;
        distance = new Vector3 (35, 0 ,0);
        canDash = true;
    }

    private void Start() {
        player = Player.Instance;
        playerRb = Player.Instance.Rb;
        dash = Player.Instance.GetComponent<Dash>();
    }

    private void LateUpdate() {
        if(Player.Instance.IsWallSliding)
            return;
        if(Player.Instance.HitWall)
            StopAllCoroutines();

        if (player.IgnoreCommands)
            return;

        if (Input.GetKeyDown(KeyCode.D) && !player.IsTouchingWall && !player.IsWallSliding){
            KeyDashD();
            //Debug.Log("D "+(timeSinceLastPress < DOUBLE_CLICK_TIME)+" "+(timeSinceLastDash > DASH_INTERVAL) +" "+ keyDashD);
            if (timeSinceLastDash > DASH_INTERVAL){
                //DashHelper ();
            }
            if (timeSinceLastPress < DOUBLE_CLICK_TIME && canDash && keyDashD == 2){
                dash.StartDash();
                canDash = false;
            }

            lastPressTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.A) && !player.IsTouchingWall && !player.IsWallSliding){
            KeyDashA();
            //Debug.Log("A "+(timeSinceLastPress < DOUBLE_CLICK_TIME)+" "+(timeSinceLastDash > DASH_INTERVAL) +" "+ keyDashA);
            if (timeSinceLastPress < DOUBLE_CLICK_TIME && canDash && keyDashA == 2){
                dash.StartDash();
                canDash = false;
            }

            lastPressTime = Time.time;
        }

        if (delta < DASH_INTERVAL && !canDash){
            delta += Time.deltaTime;
        }else {
            canDash = true;
            delta = 0.0f;
        }

        if (Time.time - lastPressTime > DOUBLE_CLICK_TIME){
            keyDashA = 0; keyDashD = 0;
        }
    }

    private bool CheckIfCanDash () {
        Debug.Log($"Dash timer {lastPressTime} - {Time.timeSinceLevelLoad} = {lastPressTime- Time.timeSinceLevelLoad} - {DASH_INTERVAL}");
        return Time.timeSinceLevelLoad - lastPressTime > DASH_INTERVAL;
    }
    private void KeyDashD (){
        timeSinceLastPress = Time.time - lastPressTime;
        timeSinceLastDash = Time.time - lastPressDash;
        keyDashD++;
        if (keyDashD > 1 && timeSinceLastPress > DOUBLE_CLICK_TIME)
            keyDashD = 0;
        //if ( !(timeSinceLastPress < DOUBLE_CLICK_TIME) )
            //keyDashD = 0;
        keyDashA = 0;
    }
    private void KeyDashA (){
        timeSinceLastPress = Time.time - lastPressTime;
        timeSinceLastDash = Time.time - lastPressDash;
        keyDashA++;
        if (keyDashA > 1 && timeSinceLastPress > DOUBLE_CLICK_TIME)
            keyDashA = 0;
        //if ( !(timeSinceLastPress < DOUBLE_CLICK_TIME) )
            //keyDashA = 0;
        keyDashD = 0;
    }
    private void DashHelper (){
        pos = player.IsFacingRight ? transform.position + distance : transform.position + distance * -1;
        Fas = player.IsFacingRight ? ForceAfterStop : ForceAfterStop * -1;
        Player.Instance.IgnoreCommands = true;
        timeSinceLastDash = Time.time;
        StopAllCoroutines();
        StartCoroutine(DashReset(dashTimer));
        keyDashD = 0; keyDashA = 0;
        Debug.Log("Double Click");
    }
    private IEnumerator DashReset (float time){
        yield return new WaitForSeconds(time);
        player.IsDashing = false;
        player.IgnoreCommands = false;
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StopAllCoroutines();
        Debug.Log("DashReset");
    }
    public IEnumerator Dash (Vector3 pos){
        Vector3 q = player.transform.position + new Vector3 (0, 7, 0);
        Vector3 q2 = player.transform.position + new Vector3 (0, 3, 0);
        
        RaycastHit2D h = Physics2D.Raycast (q, player.IsFacingRight ? Vector2.right : Vector2.left, distance.x, layerMask);
        RaycastHit2D h2 = Physics2D.Raycast (q2, player.IsFacingRight ? Vector2.right : Vector2.left, distance.x, layerMask);

        timeSinceLastDash = Time.time;

        if (h.transform != null || h2.transform != null){
            if (h.transform != null){
                Debug.Log("H1: "+h.transform.position+" "+h.transform.name);
            }else{
                Debug.Log("H2: "+h2.transform.position+" "+h2.transform.name);
            }
            Vector3 p1 = new Vector3 (h.transform == null ? 0 : h.transform.position.x, playerRb.velocity.y != 0 ? player.transform.position.y : 0, 0);
            Vector3 p2 = new Vector3 (h2.transform == null ? 0 : h2.transform.position.x, playerRb.velocity.y != 0 ? player.transform.position.y : 0, 0);
            Vector3 vector3 = h.transform != null ? p1 : p2;
            pos = vector3;
        }
 
        if (playerRb.velocity.y != 0)
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        if (player.IsFacingRight){
            while (transform.position.x < pos.x - 2)
            {
                float v = (!Player.Instance.HitWall ? DashSpeed * Time.deltaTime : DashSpeed * Time.fixedDeltaTime * StopMultiplier);
                transform.position = Vector2.MoveTowards (transform.position, pos,  v);
                yield return null;
            }
        }else{
            while (transform.position.x > pos.x + 2)
            {
                float v = (!Player.Instance.HitWall ? DashSpeed * Time.deltaTime : DashSpeed * Time.fixedDeltaTime * StopMultiplier);
                transform.position = Vector2.MoveTowards (transform.position, pos, v);
                yield return null;
            }
        }

        if (playerRb.velocity.y != 0){
            playerRb.AddForce(new Vector2(Fas, 0), ForceMode2D.Impulse);
        }
        
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;

        
        Player.Instance.IgnoreCommands = false;
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall") && player.IsDashing){
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Player.Instance.IgnoreCommands = false;
            player.IsDashing = false;
            StartCoroutine ( DashReset (dashTimer) );
            StopAllCoroutines();
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Wall") || !player.IsDashing) 
            return;
        
        //Debug.Log("Wall");
        player.IsDashing = false;
        Player.Instance.IgnoreCommands = false;
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDrawGizmos()
    {
        float height = 5;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        var position = transform.position;
        Gizmos.DrawSphere(position + new Vector3(distance.x, height, 0), 0.5f);
        Gizmos.DrawSphere(position + new Vector3(distance.x - 2, height, 0), 0.5f);
        Gizmos.DrawSphere(position + new Vector3(-1*distance.x, height, 0), 0.5f);
        Gizmos.DrawSphere(position + new Vector3(-1*distance.x + 2, height, 0), 0.5f);
        
        var playerPosition = player.transform.position;
        var o = playerPosition + new Vector3 (player.IsFacingRight ? distance.x : -distance.x, 2, 0);
        var v = playerPosition + new Vector3 (0, 2, 0);
        var o2 = playerPosition + new Vector3 (player.IsFacingRight ? distance.x : -distance.x, 7, 0);
        var v2 = playerPosition + new Vector3 (0 , 7, 0);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(o, v);
        Gizmos.DrawLine(o2, v2);
    }

}
