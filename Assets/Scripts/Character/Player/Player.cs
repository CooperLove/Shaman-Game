using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;
using System.Linq;
using UnityEngine.Serialization;

public class Player : Character
{
    public PlayerInfo playerInfo;
    private static Player instance;

    public static Player Instance { get => instance; set => instance = value; }

    [SerializeField] private Companion companion  = null;
    [SerializeField] private Companion companionObj  = null;
    private SpriteRenderer spriteRenderer = null;
    private AnimationsScript animationsScript;

    [FormerlySerializedAs("buff_debuff_Bar")] 
    public Transform buffDebuffBar;

    [Header("Transforms")]
    private Transform center;
    private Transform groundCheck;
    private Transform wallCheck;


    [Header("Booleans")]
    private bool isFacingRight = true;
    private bool isGrabbingObject = false;
    private bool isWallSliding = false;
    private bool isHittingWall = false;
    private bool isDashing = false;
    private bool isClimbingWall = false;
    private bool isWallJumping = false;
    private Transform recentTouchedWall = null;


    [Header("Floats")]
    private float vertical;
    private float horizontal;
    private float ccDuration = 2.5f;
    private float slowPercentage = 0.5f;
    private float jumpMultiplier = 52f;
    private float wallSpeed = 25f;
    private float wallJumpSpeed = 35f;
    private float lerpTime = 0.5f;
    private float xSpeed;

    
    // CONSTANTS
    private const float WALL_SLIDING_VELOCITY = 15f;
    private const float GROUND_CHECK_RADIUS = 1;
    private const float WALL_CHECK_DISTANCE = 3.5f;
    private const float HIT_WALL_DISTANCE = 4.5f;

    [Header("Ints")]
    public int jumpCount = 2;

    [Header("Layer Mask")]
    private LayerMask whatIsGround = 0;
    private LayerMask wallLayer = 0;

    [Header("Inventory")]
    private Inventory inventory;

    public Collider2D otherCollider;
    private bool isInteractingWith = false;

    private Vector3 rb2DVelocity = new Vector3();
    [SerializeField] private Vector2 aaForce = new Vector2(25, 0);

    private BlockAttack blockAttack = null;
    
    // [Header("Knock Up variables")]
    // [SerializeField] private Vector3 knockUpHeight = new Vector3();
    // [SerializeField] private float knockUpDuration = 0f;
    // [SerializeField] private Vector3 knockUpForce = new Vector3();

    [Header("Announcements tests")]
    public Vector3 pos = new Vector3(0.5f, 0.85f, 0f);
    public string areaName = "";
    public Item itemTest = null;
    public bool isRareItem = false;
    public Quest questTest = null;
    public Enemy enemyTest = null;

    [Header("Roll variables")]
    // [SerializeField] private Vector3 rollOffset = new Vector2();
    // [SerializeField] private float rollDistance = 0f;
    // [SerializeField] private float rollDuration = 0f;
    // [SerializeField] private float staminaToRoll = 0f;
    private bool isRolling;


    #region Properties
    public Transform WallCheck { get => wallCheck; set => wallCheck = value; }
    public Transform GroundCheck { get => groundCheck; set => groundCheck = value; }
    public Companion Companion { get => companion; set => companion = value; }
    public Companion CompanionObj { get => companionObj;}
    public Inventory Inventory { get => inventory; set => inventory = value; }
    public Transform Center { get => center; set => center = value; }
    public bool IsFacingRight { get => isFacingRight; private set => isFacingRight = value; }
    public bool IsTouchingWall { get; private set; }
    public bool IsWallSliding { get => isWallSliding; set => isWallSliding = value; }
    public bool HitWall { get => isHittingWall; set => isHittingWall = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool IsClimbingWall { get => isClimbingWall; set => isClimbingWall = value; }
    public bool IsWallJumping { get => isWallJumping; set => isWallJumping = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float CcDuration { get => ccDuration; set => ccDuration = value; }
    public float SlowPerc { get => slowPercentage; set => slowPercentage = value; }
    public float JumpMultiplier { get => jumpMultiplier; set => jumpMultiplier = value; }
    public float WallSpeed { get => wallSpeed; set => wallSpeed = value; }
    public float WallJumpSpeed { get => wallJumpSpeed; set => wallJumpSpeed = value; }
    public float LerpTime { get => lerpTime; set => lerpTime = value; }
    public float XSpeed { get => xSpeed; set => xSpeed = value; }
    public float WallSlindingVelocity => WALL_SLIDING_VELOCITY;
    public Vector2 AaForce { get => aaForce; set => aaForce = value; }
    public bool IsRolling { get => isRolling; set => isRolling = value; }
    public AnimationsScript AnimationsScript { get => animationsScript; set => animationsScript = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; protected set => spriteRenderer = value; }
    public bool IsGrabbingObject { get => isGrabbingObject; set => isGrabbingObject = value; }
    public bool IsInteractingWith { get => isInteractingWith; set => isInteractingWith = value; }
    public Transform RecentTouchedWall { get => recentTouchedWall; set => recentTouchedWall = value; }

    private Player (){
        Instance = this;
    }   

    #endregion
    private void Awake() {
        whatIsGround = 1 << 10 | 1 << 14;
        wallLayer = 1 << 14;
        Transform = transform.parent;
        Rb = Transform.GetComponent<Rigidbody2D>();
        Transform.position = playerInfo.Position;
        AnimationsScript = GetComponent<AnimationsScript>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        blockAttack = GetComponent<BlockAttack>();

        var checks = transform.GetChild(0);
        groundCheck = checks.GetComponentsInChildren<Transform>().Where(x => x.name.Equals("GroundCheck")).ToList()[0];
        wallCheck = checks.GetComponentsInChildren<Transform>().Where(x => x.name.Equals("WallCheck")).ToList()[0];
        // center = checks.GetComponentsInChildren<Transform>().Where(x => x.name.Equals("PlayerCenter")).ToList()[0];

        if (playerInfo.AnimalPath != null) 
            playerInfo.AnimalPath.Passive();
        jumpCount = 2;
        DontDestroyOnLoad(Transform.gameObject);
        Velocity = 1250f;

        CreateSkills ();

        // Debug.Log($"{Screen.currentResolution} dpi {Screen.dpi}");

    }

    private void Update() {
        CheckSurroundings();
        
        if (HitWall)
            IgnoreCommands = false;

        if (IgnoreCommands || GameStatus.IgnoreInputs())
            return;

        playerInfo.Position = transform.position;

        HandleInput();
        
        //Debug.Log(_rb.velocity.y);

        //InputTest ();
    }

    private void FixedUpdate()
    {
        if (IgnoreCommands || GameStatus.IgnoreInputs() || AnimationsScript.attacking || GameStatus.IsAttacking)
            return;

        Vertical   = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        
        Movement(Vertical, Horizontal);
        
        if (IsGrabbingObject || IsClimbingWall) 
            return;
        
        IsFacingRight = !FlipSprite();
        Run();


    }

    private void HandleInput (){
        if (Input.GetKeyDown(KeyCode.Q))
            GameAnnouncement.On_Show_Enemy_Name_When_Enter_Combat(enemyTest);
        
        // INTERACT WITH AN NPC
        if (Input.GetKeyDown(KeyCode.E) && otherCollider){
            otherCollider.GetComponent<Interactable>()?.OnBeginInteraction();
        }
        if (Input.GetKeyUp(KeyCode.E) && otherCollider){
            otherCollider.GetComponent<Interactable>()?.OnEndInteraction();
        }

        

        // // JUMP
        // if (Input.GetKeyDown(KeyCode.Space) && !AnimationsScript.attacking){
        //     OnJump();
        // }

    }

    private void InputTest (){
        
    }
    
    public static Player GetInstance () => Instance == null ? null : Instance;

    public override void TakeDamage(int damage){
        if (IsBlocking){
            damage = 0;
            blockAttack.OnBlock();
        }
            
            
        var damageAfterShield = playerInfo.Shield > 0 ? (int) Mathf.Abs(playerInfo.Shield - damage) : damage;
        playerInfo.Shield = playerInfo.Shield > 0 ? playerInfo.Shield - damage : 0; 
        playerInfo.Health -= damageAfterShield;
         
        playerInfo.DamageReduction = 1;
        var dmgText = Resources.Load("Prefabs/Combat/DamageText") as GameObject;
        if (!dmgText) 
            return;
        
        dmgText.GetComponent<DamageText>().SetText(damage.ToString(), damage, playerInfo.Max_HP);
        var myTransform = transform;
        Instantiate (dmgText, myTransform.position, myTransform.rotation);

        if (playerInfo.Health <= 0)
            playerInfo.IsDead = true;
    }

    public override void TakeDamage(int damage, bool showHitFX = true, bool ignoreDamage = false){
        if (IsBlocking){
            blockAttack.OnBlock();
        }
        damage = IsBlocking || ignoreDamage ? 0 : damage;

        var damageAfterShield = playerInfo.Shield > 0 ? (int) Mathf.Abs(playerInfo.Shield - damage) : damage;
        playerInfo.Shield = playerInfo.Shield > 0 ? playerInfo.Shield - damage : 0; 
        playerInfo.Health -= damageAfterShield;
         
        playerInfo.DamageReduction = 1;
        var dmgText = Resources.Load("Prefabs/Combat/DamageText") as GameObject;
        if (!dmgText || !showHitFX) 
            return;
        
        dmgText.GetComponent<DamageText>().SetText(damage.ToString(), damage, playerInfo.Max_HP);
        var myTransform = transform;
        Instantiate (dmgText, myTransform.position, myTransform.rotation);

        if (playerInfo.Health <= 0)
            playerInfo.IsDead = true;
    }

    public void Heal (int value){
        playerInfo.Health += value;

        var healText = Resources.Load("Prefabs/Combat/HealText") as GameObject;
        if (!healText) 
            return;
        
        healText.GetComponent<DamageText>().SetText("+"+value.ToString(), value, playerInfo.Max_HP);
        var myTransform = transform;
        Instantiate (healText, myTransform.position, myTransform.rotation);
    }

    private void Movement (float v, float h){
        if (IsWallSliding || isClimbingWall || IsDashing)
            return;    
    
        rb2DVelocity.x = h * Velocity * Time.fixedDeltaTime;
        rb2DVelocity.y = Rb.velocity.y;
        Rb.velocity = rb2DVelocity;
        
    }

    
    private void WallJump (float h)
    {
        var rbVelocity = Rb.velocity;
        Rb.velocity = Vector2.Lerp (rbVelocity, new Vector2 (h * WallJumpSpeed, rbVelocity.y ), LerpTime * Time.fixedDeltaTime);
    }
    
    private IEnumerator WallJumpReset (float t){
        yield return new WaitForSeconds(t);
        IsWallJumping = false;
    }
    
    
    
    private bool FlipSprite()
    {
        if (horizontal == 0)
            return SpriteRenderer.flipX;

        SpriteRenderer.flipX = horizontal < 0 ? true : false;
        return SpriteRenderer.flipX;
    }

    private void Run (){
        if ((Horizontal > 0 || Horizontal < 0)){
            AnimationsScript.OnRunEnter(Horizontal);
        }else {
            AnimationsScript.OnRunExit();
        }
    }


    private void CheckSurroundings (){

        var check = Physics2D.OverlapCircle(GroundCheck.position, GROUND_CHECK_RADIUS, whatIsGround);
        OnGround = check != null;
        AnimationsScript.animator.SetBool("OnGround", OnGround);

        var wallCheckPosition = WallCheck.position;
        
        HitWall = Physics2D.Raycast(wallCheckPosition, IsFacingRight ? Vector3.right : Vector3.left, HIT_WALL_DISTANCE, whatIsGround);

        var ray = Physics2D.Raycast(wallCheckPosition, IsFacingRight ? Vector3.right : Vector3.left, WALL_CHECK_DISTANCE, whatIsGround);
        IsTouchingWall = ray.collider != null;
        if (ray.transform == null || (RecentTouchedWall != null && RecentTouchedWall.Equals(ray.transform)))
            return;

        // Debug.Log($"Now {ray.transform?.name} Was {RecentTouchedWall?.name}");
        RecentTouchedWall = ray.transform;
    }

    public void StartDash (float interval){
        StartCoroutine(Dashing(interval));
    }
    private IEnumerator Dashing (float interval){
        IsDashing = true;
        yield return new WaitForSeconds (interval);
        IsDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall")){
            jumpCount = 2;
            IsDashing = false;
            rb2DVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("NPC") || other.CompareTag("Interactable Object")){
            otherCollider = other;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (isInteractingWith)
            return;

        if (other.CompareTag("NPC") || other.CompareTag("Interactable Object")){
            otherCollider = null;
        }
    }
    
    public void OnCreateItem (Item item){
        Inventory.CreateItem(item);
    }

    private void CreateSkills (){
        //Debug.Log(playerInfo.LearnedSkills.Count);
        // return;
        var i = 0;
        foreach (Active skill in playerInfo.LearnedSkills)
        {
            // Debug.Log($"Creating {skill.SkillName}");
            var s = PlayerLearnedSkills.Instance.OnLearnSkill(skill);
            if (i++ >= 4)
            {
                // skill.PreferredKeys = new List<KeyCode>() {};
                continue;
            }
            
            // skill.PreferredKeys = new List<KeyCode>() { (KeyCode) (282 + i-1)};
            PlayerLearnedSkills.Instance.SwitchSkills.spell02 = s;
            PlayerLearnedSkills.Instance.SwitchSkills.OnSwitchSpell();
        }
    }

    public IEnumerator Dash (Vector3 finalPos, float dashDuration){
        IsDashing = true;
        Rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        var position = Transform.position;
        var dashPosition = finalPos;
        var dashDistance = Vector3.Distance(position, finalPos);
        Debug.DrawRay(Transform.position, (isFacingRight ? Vector2.right : Vector2.left) * dashDistance, Color.magenta, 2f);
        var d = Vector2.Distance (Transform.position, dashPosition) / dashDistance;
        var duration = Mathf.Lerp (0, dashDuration, d);

        var timer = 0.0f;

        while (Vector2.Distance (Transform.position, dashPosition) > 5f){
            var v = Vector3.Lerp (position, dashPosition, timer);
            Transform.position = new Vector3(v.x, Transform.position.y, 0);
            timer += (1f/duration) * Time.deltaTime;
            yield return null;
        }
        Debug.Log("Finished dashing");
        IsDashing = false;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public IEnumerator FadeSpriteColor (float duration, bool toTransparent){
        var renderer = GetComponent<SpriteRenderer>();
        var timer = 0.0f;
        var color = renderer.color;
        var transparent = toTransparent ? 0 : 1;
        var opaque = toTransparent ? 1 : 0;

        while (renderer.color.a >= 0){
            var alpha = Mathf.Lerp(opaque, transparent, timer);
            renderer.color = new Color (color.r, color.g, color.b, alpha);    
            timer += (1f/duration) * Time.deltaTime;
            yield return null;
        }
    }

    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        if (GroundCheck)
            Gizmos.DrawWireSphere (GroundCheck.position, GROUND_CHECK_RADIUS);

        var p = Camera.main.ViewportToScreenPoint(pos);
        var p2 = Camera.main.ScreenToWorldPoint(p);
        Gizmos.DrawWireSphere(p2, 10f);
        // Debug.Log($"Screen {p2} {Input.mousePosition} {Camera.main.ScreenToViewportPoint(Input.mousePosition)}");
        
        if (!WallCheck) return;
        
        var position = WallCheck.position;
        var v = new Vector3(position.x + (IsFacingRight ? WALL_CHECK_DISTANCE : -WALL_CHECK_DISTANCE), position.y, position.z);
        Gizmos.DrawLine (position, v);
        v = new Vector3(position.x + (IsFacingRight ? HIT_WALL_DISTANCE : -HIT_WALL_DISTANCE), position.y + 1, position.z);
        Gizmos.DrawLine (position + new Vector3(0,1,0), v );
        Debug.DrawRay (v, IsFacingRight ? Vector3.right : Vector3.left, Color.red);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine();
    }

    public  override void OnBeingHit(Vector3 offset, float duration, bool stun)
    {
        throw new NotImplementedException();
    }
}
