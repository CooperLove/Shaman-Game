using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSliding : MonoBehaviour
{
    private Player player;
    private PlayerInfo playerInfo;

    [SerializeField] private float wallVelocity = 0;
    [SerializeField] private float wallSlindingVelocity = 0;

    private JumpHandler jumpHandler;
    private bool canJump = false;
    private bool justJumped = false;

    public bool CanJump { get => canJump; set => canJump = value; }

    private Transform currentWall = null;

    private float fallMultiplier = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerInfo = Player.Instance.playerInfo;
        jumpHandler = player.GetComponent<JumpHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        OnEnterWallSliding();
        OnReEnterWallSliding();
        OnJumpFromAnotherWall();
        OnJumpOffWall();

        if (!player.IsWallSliding){
            if (player.IsClimbingWall)
                player.IsClimbingWall = false;
            if (jumpHandler.FallMultiplier != -35f)
                jumpHandler.FallMultiplier = -35f;
            return;
        }

        OnEnterWallClimbing();
        OnStayWallClimbing();
        OnExitWallClimbing();

        OnExitWallSliding();
    }

    private bool OnCheckIfWallSliding (){
        return player.IsWallSliding = player.IsTouchingWall && !player.OnGround && player.Rb.velocity.y < 0; 
    }

    private void OnEnterWallSliding () {
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.IsTouchingWall){
            player.IsWallSliding = true;
            currentWall = player.RecentTouchedWall;
            fallMultiplier = jumpHandler.FallMultiplier;
            jumpHandler.FallMultiplier = 0f;
        }
    }

    private void OnReEnterWallSliding (){
        if (!player.IsWallSliding && player.IsTouchingWall && Input.GetKey(KeyCode.LeftShift))
            player.IsWallSliding = true;
    }

    private void OnExitWallSliding (){
        if (Input.GetKeyUp(KeyCode.LeftShift) || !player.IsTouchingWall){
            player.IsWallSliding = false;
            if (!justJumped)
                jumpHandler.FallMultiplier = fallMultiplier;
        }
    }

    private void OnJumpFromAnotherWall (){
        if (justJumped && currentWall != null && !currentWall.Equals(player.RecentTouchedWall) && player.IsTouchingWall){
            Debug.Log($"Reset wall");
            justJumped = false;
            jumpHandler.ResetJump();
            player.IsClimbingWall = true;
            player.IsWallSliding = true;
            currentWall = player.RecentTouchedWall;
        }
    }

    private void OnJumpOffWall (){
        if (Input.GetKeyUp(KeyCode.Space) && canJump && player.IsTouchingWall){
            jumpHandler.OnJump();
            canJump = false;
            justJumped = true;
            player.IsClimbingWall = false;
            player.IsWallSliding = false;
            Debug.Log($"Jumping --");
        }
    }

    private void WallSlidingHandler (){
        var playerRb = player.Rb;

        if (CanWallJump())
            canJump = true;

        if (OnCheckIfWallSliding() && playerRb.velocity.y < -wallSlindingVelocity){
            playerRb.velocity = new Vector2 (0, -wallSlindingVelocity);
        }
    }

    private void OnEnterWallClimbing (){
        if (!Input.GetKeyDown(KeyCode.W))
            return;

        player.IsClimbingWall = true;
    }

    private void OnStayWallClimbing (){
        if (Input.GetKey(KeyCode.W) && player.IsClimbingWall)
            OnClimbWall(player.Vertical);
    }

    private void OnClimbWall (float v){
        if (player.IsClimbingWall && !player.IsTouchingWall)
            return;

        if (CanWallJump())
            canJump = true;

        player.Rb.velocity = new Vector2 (0, v * wallVelocity);
    }

    private void OnExitWallClimbing (){
        if (!Input.GetKeyUp(KeyCode.W))
            return;
        
        player.IsClimbingWall = false;
        
    }

    private bool CanWallJump () => (player.Horizontal != 0 && jumpHandler.CurrentJumps > 0);
}
