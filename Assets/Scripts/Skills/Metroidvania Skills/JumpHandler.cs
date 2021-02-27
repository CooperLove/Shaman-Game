using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    private Player player;
    private PlayerInfo playerInfo;
    private Rigidbody2D playerRb = null;
    private const int MaxNumberOfJumps = 3;
    private int jumpCount = 3;
    [SerializeField] private int currentJumps = 1;

    [SerializeField] private float jumpForce = 75f;
    [SerializeField] private float chargeJumpForce = 125f;

    [SerializeField] private float fallMultiplier = 15.0f;
    [SerializeField] private float lowJumpMultiplier = -12.0f;

    private float timer = 0f;
    [SerializeField] private float chargeHoldTime = 0.5f;
    public bool canChargeJump = true;

    private WallSliding wallSliding = null;
    private WallJump wallJump = null;

    public int JumpCount { get => jumpCount; set => jumpCount = Mathf.Clamp(0, MaxNumberOfJumps, value); }
    public int CurrentJumps { get => currentJumps; set => currentJumps = Mathf.Clamp(0, JumpCount, value); }
    public float FallMultiplier { get => fallMultiplier; set => fallMultiplier = value; }
    public WallSliding WallSliding { get => wallSliding; set => wallSliding = value; }
    public WallJump WallJump { get => wallJump; set => wallJump = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerInfo = Player.Instance.playerInfo;
        playerRb = player.Rb;
        player.TryGetComponent(out WallSliding wallSliding);
        player.TryGetComponent(out WallJump wallJump);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.IgnoreInputs() || IsWallClimbing() || (WallJump && WallJump.WallJumping))
            return;

        if (Input.GetKey(KeyCode.Space)){
            timer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !player.AnimationsScript.attacking){

            if (timer >= chargeHoldTime && canChargeJump)
                OnChargeJump();
            else
                OnJump();
            
            timer -= timer;
        }

        JumpPhysics();
    }

    private void JumpPhysics (){
        if (player.IsWallSliding || player.IsClimbingWall)
            return;

        if (playerRb.velocity.y < 0){
            playerRb.velocity += Vector2.down * (Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime);
        }else if (playerRb.velocity.y > 0 && !Input.GetButton("Jump"))
            playerRb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
    }

    public void OnJump (){
        if (CurrentJumps < 1)
            return;
        else
            currentJumps--;

        var rbVelocity = new Vector2();
        
        rbVelocity = new Vector2(playerRb.velocity.x, 0);
        rbVelocity += Vector2.up * jumpForce;

        playerRb.velocity = rbVelocity;
        
        player.AnimationsScript.OnJumpEnter();
        StartCoroutine(OnResetJump());
    }

    private void OnChargeJump (){
        Debug.Log("Charge");
        canChargeJump = false;

        var rbVelocity = new Vector2();
        if (player.IsWallSliding || player.IsClimbingWall){
            rbVelocity = new Vector2(playerRb.velocity.x, 0);
            rbVelocity += new Vector2(player.Horizontal, 1) * chargeJumpForce;
        }else {
            rbVelocity = new Vector2(playerRb.velocity.x, 0);
            rbVelocity += Vector2.up * chargeJumpForce;
        }

        playerRb.velocity = rbVelocity;
        
        player.AnimationsScript.OnJumpEnter();
        StartCoroutine(OnResetJump());
    }

    private IEnumerator OnResetJump () {
        yield return new WaitUntil(() => playerRb.velocity.y < 0);
        if (WallSliding)
            WallSliding.CanJump = false;

        yield return new WaitUntil(() => player.OnGround);
        canChargeJump = true;
        CurrentJumps = JumpCount;
    }

    public void ResetJump () => CurrentJumps = JumpCount;
    
    private void OnGroundCheck ()
    {
        if (!player.OnGround) return;
        
        if (jumpCount != 2){
            jumpCount = 2;
        }
        if (!player.AnimationsScript.animator.GetBool("OnGround"))
            player.AnimationsScript.OnJumpExit();
    }

    private bool IsWallClimbing() => player.IsClimbingWall || player.IsWallSliding;
    private bool CanJumpOnWall() => WallSliding && WallSliding.CanJump;
}
