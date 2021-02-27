using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    private bool isStatic = false;
    protected event Action Action;
    protected Transform target = null;
    public EnemyInfo enemyInfo;
    // protected Transform transform;
    protected SpriteRenderer spriteRenderer;

    [Space(5), Header("HP/MP/SP")]
    private float hp;
    private float mp;
    private float sp;


    [Header("Armor / MR")]
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;

   
    [Header("Basic attack variables")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackRadius;
    private Transform attackPoint;
    private float attackTimer = 0f;


    [Header("Ponto de retorno, A e B: pontos de patrulha")]
    private Transform originalPos;
    private Transform pointA;
    private Transform pointB;
    private Transform currentPoint;


    [Header("Ground check"),Space(5)]
    [SerializeField] private float groundCheckRadius = 0f;
    private Transform groundPoint = null;
    private LayerMask groundLayer = 1 << 0;
    private static readonly int Hit = Animator.StringToHash("Hit");

    
    [Header("Being attack on air - Air Handler"), Space(5)]
    [SerializeField] private float airDropTimer = 0.5f;
    private float airTimer = 0f;
    public bool onAir = false;
    private bool isGettingAttacked = false;
    
    
    [Header("Unblockable Attack"), Space(5)]
    [SerializeField] private bool wasBlocked = false;
    [SerializeField] private bool canBeBlocked = false;
    [SerializeField] private bool canBePerfectlyBlocked = false;
    [SerializeField] protected float unblockableAttackChance = 35f;
    protected BoxCollider2D boxCollider2D = null;


    [Header("State Timers"), Space(5)]
    [SerializeField] protected Vector2 patrolTimeRange = new Vector2();
    protected float patrolTimer = 0.0f;
    [SerializeField] protected Vector2 idleTimeRange = new Vector2();
    protected float idleTimer = 0.0f;



    [Header("General context variables")]
    [SerializeField] protected float followDist;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float stunDuration = 0.15f;
    [SerializeField] protected float halfHp = 0.0f;
    private bool isDead = false;
    protected float attackSpeed;
    protected LayerMask playerLayer;
    protected Player player;
    private Coroutine onEnemyHit = null;
    private Coroutine onAirEnemyHit = null;
    protected Coroutine WalkingAway = null;
    protected Coroutine IdlePatrolCoroutine = null;
    protected Coroutine OnBeingBlockedReset = null;
    protected Animator animator = null;
    protected Transform visionCollider = null;


#region Properties
    public Coroutine OnEnemyHit { get => onEnemyHit; set => onEnemyHit = value; }
    public Coroutine OnAirEnemyHit { get => onAirEnemyHit; set => onAirEnemyHit = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public float StunDuration { get => stunDuration; set => stunDuration = value; }
    protected Transform CurrentPoint { get => currentPoint; set => currentPoint = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Mp { get => mp; set => mp = value; }
    public float Sp { get => sp; set => sp = value; }
    public Transform PointB { get => pointB; set => pointB = value; }
    public Transform PointA { get => pointA; set => pointA = value; }
    public Transform GroundPoint { get => groundPoint; set => groundPoint = value; }
    public Transform OriginalPos { get => originalPos; set => originalPos = value; }
    public Transform AttackPoint { get => attackPoint; set => attackPoint = value; }
    public float AirTimer { get => airTimer; set => airTimer = value; }
    public bool CanBeBlocked { get => canBeBlocked; set => canBeBlocked = value; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public Transform Target { get => target; protected set => target = value;}
    public bool WasBlocked { get => wasBlocked; set => wasBlocked = value; }
    public bool IsStatic { get => isStatic; protected set => isStatic = value; }
    public bool CanBePerfectlyBlocked { get => canBePerfectlyBlocked; set => canBePerfectlyBlocked = value; }
    public float Armor { get => armor; set => armor = value; }
    public float MagicResistance { get => magicResistance; set => magicResistance = value; }

    #endregion

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        Velocity = enemyInfo.velocity;
    }

    private void Awake() {
        groundLayer = 1 << 10;
    }
    
    private void Update() {
        
        OnGroundCheck();

        if (!OnGround && onAir && isGettingAttacked)
        { 
            AirAttackHandler();
        }

        if (IgnoreCommands || IsStuned)
            return;
        
        Action?.Invoke();
        
        if (Rb.velocity.y < 0)
            Rb.velocity += Vector2.down * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);

    }
    
    public override void TakeDamage (int h){
        //Debug.Log($"{name} taking {h} damage");
        Hp = Hp - h < 0 ? 0 : Hp -= h;

        isGettingAttacked = onAir;
        
        GameAnnouncement.On_Show_Enemy_Name_When_Enter_Combat(this);

        canBeBlocked = false;
        attackTimer -= attackTimer;
        
        if (!Target)
            target = player?.transform;
        
        if (WalkingAway != null){
            StopCoroutine(WalkingAway);
            SetState (Patrol);
        }

        if (onAir)
            AirTimer -= AirTimer;
        
        InstantiateDamageTextAndFX (h);
        
        if (!(Hp <= 0)) 
            return;
        
        OnDie();
    }

    public override void TakeDamage (int h, bool showHitFX = true, bool ignoreDamage = false){
        //Debug.Log($"{name} taking {h} damage");
        if (ignoreDamage)
            h = 0;

        Hp = Hp - h < 0 ? 0 : Hp -= h;

        isGettingAttacked = onAir;
        
        GameAnnouncement.On_Show_Enemy_Name_When_Enter_Combat(this);

        canBeBlocked = false;
        attackTimer -= attackTimer;
        
        if (!Target)
            target = player?.transform;
        
        if (WalkingAway != null){
            StopCoroutine(WalkingAway);
            SetState (Patrol);
        }

        if (onAir)
            AirTimer -= AirTimer;
        if (showHitFX)
            InstantiateDamageTextAndFX (h);
        
        if (!(Hp <= 0)) 
            return;
        
        OnDie();
    }

    private void InstantiateDamageTextAndFX (int h){
        
        if (dmgText){
            dmgText.GetComponent<DamageText>().SetText(h.ToString(), h, enemyInfo.Max_HP);
            Instantiate (dmgText, transform.position, transform.rotation);
        }

        var facing = player?.IsFacingRight;
        
        if (hitFX){
            var hit = Instantiate (hitFX, transform.position, hitFX.transform.rotation);
            hit.transform.localScale = player.IsFacingRight ? Vector3.one : new Vector3 (-1, 1, 1);
        }
    }
    
    protected void Initialize () {
        SetState(Idle);

        AttackPoint = GetComponentsInChildren<Transform>(true).First(x => x.name.Equals("Attack Point"));
        GroundPoint = GetComponentsInChildren<Transform>(true).First(x => x.name.Equals("Ground Point"));

        PointA = transform.parent.GetComponentsInChildren<Transform>(true).First(x => x.name.Equals("Point A"));
        PointB = transform.parent.GetComponentsInChildren<Transform>(true).First(x => x.name.Equals("Point B"));

        visionCollider = transform.GetComponentsInChildren<Transform>(true).First(x => x.name.Equals("Vision Collider"));

        dmgText = Resources.Load ("Prefabs/Combat/DamageText") as GameObject;
        hitFX = Resources.Load($"Prefabs/Combat/Hit FX/Basic Hit FX") as GameObject;

        Transform = transform;

        playerLayer = 1 << 8;
        player = Player.Instance;

        Rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
 
        // var q = Resources.Load("ScriptableObjects/Enemy/Enemy3") as EnemyInfo;
        // var i = ScriptableObject.CreateInstance<EnemyInfo>() as EnemyInfo;

        enemyInfo.SetupStats(this);
        IsStatic = enemyInfo.IsStatic;

        halfHp = (enemyInfo.Max_HP/100.0f) * 50.0f;
        animator = GetComponent<Animator>();
    } 

    public void OnVisionTrigger (){
        SetState(Follow);
        target = player.transform;
        if (IdlePatrolCoroutine != null)
            StopCoroutine(IdlePatrolCoroutine);
    }

    protected void SetState (Action method){
        Action = method;
    }
    protected virtual void OnDie (){
        if (hp > 0)
            return;
            
        Player.Instance.playerInfo.OnGainEXP (enemyInfo.xpGiven);
        //GetComponent<Animator>()?.SetTrigger("Dead");
        Rb.isKinematic = true;
        boxCollider2D.enabled = false;
        IsDead = true;
        enemyInfo.IsDead = true;
        enemyInfo.RaiseEvent();
        Destroy(gameObject, 2);
    }

    protected abstract void Idle ();
    protected abstract void Attack ();
    public abstract void Patrol ();
    protected abstract void UnblockableAttack ();
    protected abstract IEnumerator WalkAway ();

    protected virtual void Follow (){
        // Debug.Log($"Follow");
        if (Target == null)
            return;

        var position = transform.position;
        
        if (Vector2.Distance(position, Target.position) >= attackRange){
            var playerPosition = Target.position;
            var pos = Vector2.MoveTowards(position, playerPosition, Velocity * Time.deltaTime);
            position = new Vector2 (pos.x, position.y);
            
            var isPlayerInFront = playerPosition.x > position.x;
            spriteRenderer.flipX = !isPlayerInFront;
            
            transform.position = position;
        }else
            SetState(Attack);
    }

    public virtual void OnBeingBlocked (float dur) {
        AttackTimer -= AttackTimer;
        WasBlocked = true;
        canBeBlocked = false;
        CanBePerfectlyBlocked = false;
        SetState(Patrol);
        if (OnBeingBlockedReset != null)
            StopCoroutine(OnBeingBlockedReset);
        OnBeingBlockedReset = StartCoroutine(ResetBlock(dur));
    }

    public bool CanBeBlockedCheck ()=> canBeBlocked && !wasBlocked;
    public bool CanBePerfectlyBlockedCheck ()=> canBePerfectlyBlocked && !wasBlocked;

    protected IEnumerator ResetBlock (float dur){
        yield return new WaitForSeconds (dur);
        WasBlocked = false;
    }
    
    public virtual IEnumerator Flash (){
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    

    public virtual void ResetHurt () => GetComponent<Animator>().ResetTrigger(Hit);

    private void OnGroundCheck (){
        var c = Physics2D.OverlapCircle(GroundPoint.position, groundCheckRadius, groundLayer);
        
        OnGround = c != null;
        IgnoreCommands = !OnGround;
        if (OnGround)
            onAir = false;
        if (OnGround && AirTimer != 0)
            AirTimer = 0f;
    }

    protected void OnHit (Vector3 offset, float duration, bool stun)
    {
        if (OnEnemyHit != null)
            StopCoroutine(OnEnemyHit);
                
        OnEnemyHit = StartCoroutine(OnHitCall(transform, this, offset, duration, stun));
    }

    
    private IEnumerator OnHitCall (Transform enemyTransform, Enemy e, Vector3 onHitOffset, float duration, bool stun)
    {
        IgnoreCommands = true;
        var position = enemyTransform.position;
        var rayCastHit2D = Physics2D.Raycast(position, 
            player.IsFacingRight ? Vector2.right : Vector2.left, 
            onHitOffset.x, 1 << 10);
        
        var point = position + (player.IsFacingRight ? onHitOffset : -onHitOffset);
        if (rayCastHit2D)
        {
            point = rayCastHit2D.point;
            if (rayCastHit2D.distance <= onHitOffset.x)
                yield break;
        }
        
        //Debug.Log($"Transform: {position}  Point: {point}");
        
        var startValue = position;
        var endValue = point;

        var timer = 0f;
        
        while (timer <= 1f)
        {
            var xPos = Mathf.Lerp(startValue.x, endValue.x, timer);
            //Debug.Log($"Nova Posição => {xPos} com {timer}s");
            var enemyPos = enemyTransform.position;
            enemyPos = new Vector3(xPos, enemyPos.y, enemyPos.z);
            enemyTransform.position = enemyPos;
            
            timer += (1f / (1f/duration)) * Time.deltaTime;
            yield return null;
        }
        
        if (!e.IsStuned && stun)
            StartCoroutine(Debuff.Stun(e, e.StunDuration));

        // Debug.Log("OnHitEnd");
        enemyTransform.position = point;
    }
    
    public void OnAirHit ()
    {
        if (onAirEnemyHit != null)
            StopCoroutine(onAirEnemyHit);

        onAirEnemyHit = StartCoroutine(AirAttackHandler(0.5f));
    }
    
    private IEnumerator AirAttackHandler (float duration){
        Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        var time = Time.timeSinceLevelLoad;
        
        yield return new WaitForSeconds(duration);
        //Debug.Log($"{this.name} air duration {duration} - {time}");
        Rb.velocity += Physics2D.gravity;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void AirAttackHandler()
    {
        if (!onAir)
            return;
        
        if (AirTimer < airDropTimer)
        {
            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            AirTimer += Time.deltaTime;
            return;
        }

        //airTimer -= airTimer;
        isGettingAttacked = false;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        if (pointA && pointB && groundPoint){
            Gizmos.DrawWireSphere(pointA.position, radius);
            Gizmos.DrawWireSphere(pointB.position, radius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GroundPoint.position, groundCheckRadius);
        }
        Gizmos.color = Color.cyan;
        if (transform){
            var position = transform.position;
            Gizmos.DrawLine(position, position + new Vector3(followDist, 0));
            Gizmos.DrawLine(position, position - new Vector3(followDist, 0));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + new Vector3(attackRange, 0));
            Gizmos.DrawLine(position, position - new Vector3(attackRange, 0));
        }

        if (AttackPoint == null) return;
        {
            var position = AttackPoint.position;
            Gizmos.DrawWireSphere(position, attackRadius);
            Gizmos.DrawWireSphere(new Vector3(-position.x, position.y, position.z), attackRadius);
        }
    }
}
