using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    private Player player;
    private PlayerInfo playerInfo;
    private Transform playerTransform;
    [SerializeField] private float angle = 0f;
    [SerializeField] private float jumpMinRange = 0f;
    [SerializeField] private float jumpDuration = 0f;
    [SerializeField] private float stickToWallDuration = 0f;

    [SerializeField] private bool isInRangeToJump = false;
    private bool isWallJumping = false;
    private bool wallJumping = false;
    private bool isOnAWall = false;
    private bool gotOnTopOfAWall = false;

    [SerializeField] private Vector2 size = Vector2.zero;
    [SerializeField] private Vector3 checkOffset = Vector3.zero;

    private RaycastHit2D ray;
    private GhostController ghostController;
    private LayerMask wallLayer = 0;

    public bool IsWallJumping { get => isWallJumping; private set => isWallJumping = value; }
    public bool WallJumping { get => wallJumping; private set => wallJumping = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerInfo = Player.Instance.playerInfo;
        playerTransform = player.Transform;
        wallLayer = 1 << 14;
        player.GetComponent<JumpHandler>().WallJump = this;
        ghostController = player.GetComponent<GhostController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !IsWallJumping){
            OnWallJumpEnter();
        }
    }

    private void OnWallJumpEnter (){
        IsWallJumping = true;
        var point = CheckNearbyWalls();

        if (point.Equals(Vector3.zero) && WallJumping)
            point = OnGetOnTopOfWall();

        if (point.Equals(Vector3.zero)){
            IsWallJumping = false;
            return;
        }
        
        StartCoroutine(GetToWall(point));
        StartCoroutine(OnGetOnGround());
    }   

    private Vector3 CheckNearbyWalls (){
        var cosseno = 1 * (Mathf.Cos(Mathf.Deg2Rad * angle));
        var seno = 1 * (Mathf.Sin(Mathf.Deg2Rad * angle));
        var cosseno180 = 1 * (Mathf.Cos(Mathf.Deg2Rad * (180 - angle)));
        var seno180 = 1 * (Mathf.Sin(Mathf.Deg2Rad * (180 - angle)));
        Vector3 dir;
        if (isOnAWall){
            var onRightSideOfWall = player?.RecentTouchedWall?.position.x  < transform.position.x;
            dir = onRightSideOfWall ? new Vector3(cosseno, seno, 0) : new Vector3(cosseno180, seno180, 0);
            player.SpriteRenderer.flipX = onRightSideOfWall ? false : true; 
        }
        else 
            dir = player.IsFacingRight ? new Vector3(cosseno, seno, 0) : new Vector3(cosseno180, seno180, 0);


        ray = Physics2D.Raycast (transform.position, dir, jumpMinRange, wallLayer);
        Debug.DrawRay(transform.position, dir * jumpMinRange, Color.magenta, 2f);

        return ray.collider != null ? (Vector3) ray.point : Vector3.zero;
    }

    private Vector3 OnGetOnTopOfWall (){
        var dir = player.IsFacingRight;

        var overlap = Physics2D.OverlapBox (transform.position + (player.IsFacingRight ? -checkOffset : checkOffset), size, 0, wallLayer);

        gotOnTopOfAWall = true;
        if (!overlap)
            return Vector3.zero;

        overlap.TryGetComponent(out Wall top);

        if (!top || (top && !top.topOfWall))
            return Vector3.zero;

        return top.topOfWall.position;
    }

    private IEnumerator GetToWall (Vector3 point){
        var initialPoint = playerTransform.position;
        player.Rb.isKinematic = true;
        ghostController.ghostActive = true;
        WallJumping = true;

        var dist = Vector2.Distance(playerTransform.position, point);
        var dur = Mathf.Lerp (0, jumpDuration, dist/jumpMinRange);
        // Debug.Log($"Jump to wall from {dist} gonna take {dur}s");

        var timer = 0f;
        while (timer < 1f && Vector2.Distance(playerTransform.position, point) > 3.5f){
            playerTransform.position = Vector3.Lerp (initialPoint, point, timer);

            timer += (1f/dur) * Time.deltaTime;
            yield return null;
        }

        playerTransform.position = point;
        player.Rb.isKinematic = false;
        IsWallJumping = false;
        isOnAWall = true;
        StartCoroutine(StickToWall());
    }

    private IEnumerator StickToWall (){
        player.Rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        // player.Rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSecondsOrUntil (stickToWallDuration, () => IsWallJumping || gotOnTopOfAWall || player.Horizontal != 0);
        // Debug.Log($"Fall from wall");
        player.Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isOnAWall = false;
    }

    private IEnumerator OnGetOnGround () {
        yield return new WaitUntil (() => player.OnGround);
        // Debug.Log("Player is on ground");
        ghostController.ghostActive = false;
        WallJumping = false;
        gotOnTopOfAWall = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.grey;
        var cosseno = jumpMinRange * (Mathf.Cos(Mathf.Deg2Rad * angle));
        var seno = jumpMinRange * (Mathf.Sin(Mathf.Deg2Rad * angle));

        if (player){
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(player.IsFacingRight ? cosseno : -cosseno, seno, 0));
            Gizmos.DrawWireCube(transform.position + (player.IsFacingRight ? checkOffset : -checkOffset), size);
        }else{
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(cosseno, seno, 0));
            Gizmos.DrawWireCube(transform.position + checkOffset, size);
        }
    }
}
