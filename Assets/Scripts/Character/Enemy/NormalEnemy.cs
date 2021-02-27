using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEnemy : Enemy
{
    private float distance = 0f;
    private float step = 15;
    private bool hit = false;

    [Header("Unblockable attack variables - Dash"), Space(10)]
    [SerializeField] private float dashDuration = 0.0f;
    [SerializeField] private float dashUseDistance = 30.0f;
    [SerializeField] private float dashGapDistance = 0.0f;
    [SerializeField] private Vector3 dashOffset = new Vector3();
    private bool isDashing = false;
    private CapsuleCollider2D capsuleCollider2D = null;

    [Space(10)]
    private SpriteRenderer blockBar = null;
    private GameObject warning = null;
    [SerializeField] private float animationDuration = 0.0f;
    [SerializeField] private Vector2 blockInterval = new Vector2();
    private float animTimer = 0.0f;
    [SerializeField] private Color c1 = Color.green;
    [SerializeField] private Color c2 = Color.red;

    public float Distance { get => distance; set => distance = value; }
    public float Step { get => step; set => step = value; }

    private void Start() {
        blockBar = GetComponentsInChildren<SpriteRenderer>().First (x => x.name.Equals("Block Bar"));
        capsuleCollider2D = GetComponentsInChildren<CapsuleCollider2D>().FirstOrDefault (x => x.name.Equals ("Dash Collider"));
        warning = Resources.Load("Prefabs/Combat/Warnings/Unblockable Attack Warning") as GameObject;
        Initialize();
        
        CurrentPoint = PointA;
        attackSpeed = 1f;
        Velocity = enemyInfo.velocity;

        Hp = 4000;

        IdlePatrolCoroutine = StartCoroutine(IdleTimer());
    }

    protected override void UnblockableAttack()
    {
        if (!isDashing){
            StartCoroutine (UnblockableDash());
            Sp -= 10;
        }
    }

    private IEnumerator UnblockableDash (){
        isDashing = true;
        Rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        var position = transform.position;
        var onRightSide = position.x >= player.transform.position.x;
        var playerPosition = player.transform.position + (onRightSide ? -dashOffset : dashOffset);
        var ray = Physics2D.Raycast (transform.position, onRightSide ? Vector2.left : Vector2.right, dashUseDistance, 1 << 14);
        if (ray.collider != null)
            playerPosition = ray.point;

        var d = Vector2.Distance (transform.position, playerPosition) / dashUseDistance;

        var duration = Mathf.Lerp (0, dashDuration, d);

        spriteRenderer.flipX = onRightSide;

        var wrn = Instantiate(warning, transform.position, warning.transform.rotation);

        spriteRenderer.color = c2;
        yield return new WaitForSeconds(0.7f);
        capsuleCollider2D.enabled = true;
        spriteRenderer.color = Color.white;

        var timer = 0.0f;

        while (Vector2.Distance (transform.position, playerPosition) > dashGapDistance){
            var v = Vector3.Lerp (position, playerPosition, timer);
            transform.position = new Vector3(v.x, transform.position.y, 0);
            timer += (1f/duration) * Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        capsuleCollider2D.enabled = false;
        SetState (Attack);
    }

    private IEnumerator Dash () {
        yield return null;
    }

    protected override void Attack()
    {   
        if (isDashing)
            return;

        // Debug.Log($"Attack");

        if (IdlePatrolCoroutine != null)
            StopCoroutine(IdlePatrolCoroutine);

        var distance = Vector2.Distance(transform.position, player.transform.position);
        if ( distance >= attackRange){
            SetState(Follow);
            CanBeBlocked = false;
            AttackTimer -= AttackTimer;
        }


        if (AttackTimer >= 1/attackSpeed){
            if (CanUseUnblockableAttack(distance)){
                UnblockableAttack();
                AttackTimer -= AttackTimer;
                CanBeBlocked = false;
                return;
            }
            if (!WasBlocked){
                CanBeBlocked = true;
                SetState(SimulateAnimation);
            }
        }else {
            AttackTimer += Time.deltaTime;
        }
    }

    private void SimulateAnimation (){
        if (WasBlocked){
            animTimer -= animTimer;
            CanBeBlocked = false;
            CanBePerfectlyBlocked = false;
            return;
        }

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
            // Debug.Log($"{name} attacking player");
        }

        // RESET
        if (animTimer >= animationDuration){
            hit = false;
            AttackTimer -= AttackTimer;
            animTimer -= animTimer;
            CanBeBlocked = false;
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(3, 3), 0, 1 << 9);

            if (colliders.Length > 1){
                WalkingAway = StartCoroutine(WalkAway());
                return;
            }
            SetState(Patrol);
        }else {
            animTimer += Time.deltaTime;
            blockBar.color = animTimer <= blockInterval.y ? Color.Lerp(c2, c1, animTimer / blockInterval.x) : c2;

            // WHEN THE ATTACK CAN BE BLOCK
            if (animTimer >= blockInterval.x && animTimer <= blockInterval.y && !CanBePerfectlyBlocked){
                blockBar.color = c1;
                CanBePerfectlyBlocked = true;
            }

            if (animTimer > blockInterval.y){
                CanBePerfectlyBlocked = false;
            }
        }
    }

    

    public override void Patrol()
    {
        if (isDashing)
            return;

        if (target){
            SetState(Follow);
            return;
        }

        var position = transform.position;
        var pos = Vector2.MoveTowards(position, CurrentPoint.position, Step * Time.deltaTime);

        spriteRenderer.flipX = CurrentPoint == PointA ? true : false;
        
        position = new Vector2(pos.x, position.y);
        transform.position = position;
    }
    
    private bool CanUseUnblockableAttack (float dist) => Hp <= halfHp && Sp >= 10 && player.OnGround && dist <= dashUseDistance && Random.Range (0,101) <= unblockableAttackChance;

    private Vector3 CalculatePoint (){
        var playerPos = player.transform.position;
        var position = transform.position;

        var onRightSide = position.x >= playerPos.x;

        var dist = Random.Range(20, 31);

        var rayCastHit2D = Physics2D.Raycast(position, onRightSide ? Vector2.right : Vector2.left, dist, 1 << 10);

        var point = rayCastHit2D.collider ? rayCastHit2D.distance : dist;

        return new Vector3 (position.x + (onRightSide ? point : -point), position.y, position.z);
    }
    protected override IEnumerator WalkAway()
    {
        if (IdlePatrolCoroutine != null)
            StopCoroutine(IdlePatrolCoroutine);

        SetState(null);
        var point = CalculatePoint();
        // Debug.Log($"{name} walking towards {point}");
        
        while (Vector3.Distance(transform.position, point) > 5f){
            transform.position = Vector3.MoveTowards(transform.position, point, Velocity * Time.deltaTime);
            yield return null;
        }
    
        WalkingAway = null;
        SetState(Attack);
    }

    protected override void Idle()
    {
        if (target){
            SetState(Follow);
            return;
        }
        
        // Debug.Log($"Idle");
        var playerDist = Vector2.Distance(transform.position, player.transform.position);
        if (playerDist <= followDist)
            SetState(Follow);
    }

    private IEnumerator IdleTimer () {
        if (target)
            yield break;

        animator.SetBool("Idle", true);
        
        var dur = Random.Range(idleTimeRange.x, idleTimeRange.y);
        yield return new WaitForSeconds(dur);

        SetState(Patrol);

        IdlePatrolCoroutine = StartCoroutine (PatrolTimer());
    }

    private IEnumerator PatrolTimer (){
        if (target)
            yield break;

        animator.SetBool("Idle", false);

        var dur = Random.Range(patrolTimeRange.x, patrolTimeRange.y);
        yield return new WaitForSeconds(dur);

        SetState(Idle);

        IdlePatrolCoroutine = StartCoroutine (IdleTimer());
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag.Equals("Point")){
            var isPointA = CurrentPoint == PointA;
            CurrentPoint = isPointA ? PointB : PointA;
            visionCollider.localScale = new Vector3(isPointA ? 1 : -1, 1, 1);
        }
    }

    public override void OnBeingHit(Vector3 offset, float duration, bool stun)
    {
        OnHit (offset, duration, stun);
    }

    //Vector3 offset, float duration, bool stun
}
