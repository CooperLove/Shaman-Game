using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticEnemy : Enemy
{
    private bool hit = false;

    private SpriteRenderer blockBar = null;
    [SerializeField] private float animationDuration = 0.0f;
    [SerializeField] private Vector2 blockInterval = new Vector2();
    private float animTimer = 0.0f;
    [SerializeField] private Color c1 = Color.green;
    [SerializeField] private Color c2 = Color.red;
    private void Start() {
        blockBar = GetComponentsInChildren<SpriteRenderer>().First (x => x.name.Equals("Block Bar"));
        
        attackSpeed = 1.5f;
        AttackTimer = attackSpeed;

        Initialize();
        SetState(Patrol);
    }

    protected override void UnblockableAttack()
    {
    }

    protected override void Attack()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= attackRange){
            SetState(Patrol);
            CanBeBlocked = false;
            AttackTimer -= AttackTimer;
        }

        if (AttackTimer >= 1/attackSpeed){
            SetState (SimulateAnimation);
        }else {
            AttackTimer += Time.deltaTime;
        }
    }

    private void SimulateAnimation (){
        // CAUSE DAMAGE ON PLAYER
        if (animTimer > blockInterval.y && !hit){
            var isPlayerInFront = player.transform.position.x > transform.position.x;
            AttackPoint.localPosition = isPlayerInFront ? new Vector3(0.5f,-0.15f,0) : new Vector3(-0.5f,-0.15f,0);
            spriteRenderer.flipX = !isPlayerInFront;

            var playerCollider = Physics2D.OverlapCircle(AttackPoint.position, attackRadius, playerLayer);
            if (playerCollider){
                player.TakeDamage(10);
            }
            hit = true;
        }

        // RESET
        if (animTimer >= animationDuration){
            hit = false;
            blockBar.color = c1;
            animTimer -= animTimer;
            AttackTimer -= AttackTimer;
            CanBeBlocked = false;
            CanBePerfectlyBlocked = false;
            SetState(Attack);
        }else {
            animTimer += Time.deltaTime;
            
            if (animTimer > blockInterval.y){
                CanBePerfectlyBlocked = false;
                return;
            }
            // WHEN THE ATTACK CAN BE BLOCK
            if (animTimer >= blockInterval.x && animTimer <= blockInterval.y && !CanBePerfectlyBlocked){
                blockBar.color = Color.green;
                CanBePerfectlyBlocked = true;
                return;
            }
            if (animTimer < blockInterval.x)
                blockBar.color = Color.Lerp(c1, c2, animTimer / blockInterval.x);
        }
    }

    protected override void Follow()
    {
        SetState(Patrol);
    }

    public override void Patrol()
    {
        var playerDist = Vector2.Distance(transform.position, player.transform.position);
        if (playerDist <= attackRange)
            SetState(Attack);
    }

    protected override IEnumerator WalkAway()
    {
        yield return null;
    }

    protected override void Idle()
    {
        SetState(Patrol);   
    }

    public  override void OnBeingHit(Vector3 offset, float duration, bool stun)
    {
        
    }
}
