using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private string bossName = "";
    [SerializeField] private float maxHP = 0;
    [SerializeField] private float hp = 0;
    public int damage = 0;
    public int deff = 0;
    public int defm = 0;
    [SerializeField] private bool invunerable = false;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] public Vector2 dashForce = Vector2.zero;
    [SerializeField] private bool IsFacingRight = true;
    private bool isDashing = false;
    public ParticleSystem ps = null;
    public GameObject slash = null;
    private Transform player = null;
    [SerializeField] private Transform slashPos = null;
    private const float SLASH_TIMER = 6.0f, DASH_TIMER = 10.5f;
    private const float DOUBLE_DASH_TIMER = 18.0f;
    [SerializeField] private float slashTimer = 0;
    [SerializeField] private float dashTimer = 0;
    [SerializeField] private float ddTimer = 0;
    [SerializeField] private bool hit = false, isAttacking = false;
    public bool growing, stg01;
    public float lerpTimer;
    [SerializeField] public Transform minimum = null;
    [SerializeField] public Transform maximum = null;
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private Transform currPoint = null;
    [SerializeField] private float t = 0;
    [SerializeField] private float doubleDashTimer = 0;
    [SerializeField] private float ddTime = 0;
    [SerializeField] private bool doubleDash = false;
    [SerializeField] private float pointDist = 0;
    [SerializeField] private int numDashes = 0;
    [SerializeField] private float attackRange = 100;
    [SerializeField] private LayerMask playerLayer = 0;
    [SerializeField] private GameObject ddCollider = null;
    [SerializeField] private Conversation endConversation = null;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public string BossName { get => bossName; }
    public float MaxHP { get => maxHP;}
    public float Hp { get => hp; set => hp = value; }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = Player.Instance.transform;
        currPoint = maximum;
    }

    private void Update() {
        if (!isDashing)
            Flip();

        if (growing){
            Grow();
            return;
        }
        if (doubleDash){
            DoubleDash();
            return;
        }
        if(!growing)
            UpdateTimers ();
    }

    public void TakeDamage (int value){
        if (invunerable)
            return;

        Hp -= value;
        BossUIManager.Instance.UpdateHPBar();

        if (Hp <= 0){
            hp = 0;
            OnDie();
            return;
        }

        if (Hp <= MaxHP/2){
            OnStage01Enter();
        }
        
            
    }

    public void OnDie (){
        Destroy(gameObject);
        BossUIManager.Instance.OnEndBattle();
        endConversation.StartDialogue();
        GameStatus.isOnChallenge = false;
    }

    public void OnStage01Enter (){
        if (stg01)
            return;
        animator.SetTrigger("Stage01");
        invunerable = true;
        growing = true;
        stg01 = true;
        damage += 50;
        deff += 25;
        defm += 25;
    }

    public void ResetInvunerability () => invunerable = false;

    private void UpdateTimers (){
        if (hit || IsAttacking || animator.GetInteger("Attack") > 0)
            return;

        if (dashTimer <= DASH_TIMER)
            dashTimer += Time.deltaTime;
        else {
            dashTimer = 0;
            IsAttacking = true;
            animator.Rebind();
            animator.SetInteger("Attack", 1); 
        }
        if (slashTimer <= SLASH_TIMER)
            slashTimer += Time.deltaTime;
        else {
            slashTimer = 0;
            IsAttacking = true;
            animator.Rebind();
            animator.SetInteger("Attack", 2); 
        }
        if (ddTimer <= DOUBLE_DASH_TIMER)
            ddTimer += Time.deltaTime;
        else {
            ddTimer = 0;
            IsAttacking = true;
            animator.Rebind();
            animator.SetInteger("Attack", 3); 
        }
    }

    public void Grow (){
        float x = Mathf.Lerp(transform.localScale.x, (IsFacingRight ? 29 : -29), lerpTimer);
        float y = Mathf.Lerp(transform.localScale.y, 29, lerpTimer);
        hp = Mathf.Lerp(hp, (MaxHP/100)*75, lerpTimer);
        BossUIManager.Instance.UpdateHPBar();
        transform.localScale = new Vector3(x, y, 1);
        lerpTimer += t * Time.deltaTime;
        if (x >= 28.9f || x <= -28.9f){
            growing = false;
            lerpTimer = 0;
            invunerable = false;
        }
    }

    public void OnGrowEnd (){
        animator.ResetTrigger("Stage01");
    }

    public void onDoubleDashEnter () {
        ps.Play();
        doubleDash = true;
        ps.transform.localScale = new Vector3((IsFacingRight ? 1 : -1), 1, 1);
        ddCollider.SetActive(true);
        ps.gameObject.SetActive(true);
        transform.position = new Vector3 (currPoint.position.x, transform.position.y, transform.position.z);
    }
    
    public void onDoubleDashExit () {
        numDashes = 0;
        doubleDash = false;
        isAttacking = false;
        ps.gameObject.SetActive(false);
        ddCollider.SetActive(false);
        animator.SetInteger("Attack", -1);
    }
    public void DoubleDash (){
        if (numDashes == 3)
            onDoubleDashExit();

        if (currPoint.Equals(maximum))
            Flip(maximum);
        else
            Flip(minimum);
        // animate the position of the game object...
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, currPoint.position.x, ddTime), transform.position.y, transform.position.z);

        // .. and increase the t interpolater
        doubleDashTimer += ddTime * Time.deltaTime;

        //Debug.Log(Vector2.Distance(currPoint.position, transform.position));
        if (Vector2.Distance(currPoint.position, transform.position) < pointDist)
        {
            // Transform temp = maximum;
            // maximum = minimum;
            // minimum = temp;
            currPoint = currPoint.Equals(maximum) ? minimum : maximum;
            doubleDashTimer = 0.0f;
            numDashes++;
        }
    }

    public void Attack (){
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if(player.Length > 0){
            //StartCoroutine(OnAttackFreeze());
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
        }
        foreach (Collider2D p in player)
        {
            p.GetComponent<Player>().TakeDamage(10);
        }
    }

    public void TestLerp () {
        growing = !growing;
    }

    public void Flip (){
        if (transform.position.x >  player.position.x)
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, 1);
        else if (transform.position.x < player.position.x)
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        IsFacingRight = transform.localScale.x > 0 ? true : false;
    }
    public void Flip (Transform target){
        if (transform.position.x >  target.position.x)
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, 1);
        else if (transform.position.x < target.position.x)
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        IsFacingRight = transform.localScale.x > 0 ? true : false;
        ps.transform.localScale = new Vector3((IsFacingRight ? 1 : -1), 1, 1);
    }
    public void Slash () {
        GameObject g = Instantiate(slash, slashPos.position, transform.rotation);
        g.GetComponent<Slash>().right = IsFacingRight;
        g.SetActive(true);
    }
    public void DashTesting () => animator.SetInteger("Attack", 1);
    public void SlashTesting () => animator.SetInteger("Attack", 2);
    public void AttackReset () => isAttacking = false;
    public void ResetAttack () {
        IsAttacking = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack A")){
            animator.SetInteger("Attack", -1);
            Debug.Log("????");
            return;
        }
            animator.SetInteger("Attack", -1);
    }
    public void EndDashTesting () {
        animator.SetInteger("Attack", -1);
        ps.gameObject.SetActive(false);
        ddCollider.SetActive(false);
        rb.velocity = Vector2.zero;
        isDashing = false;
    } 
    public void Dash (){
        rb.AddForce(dashForce * (IsFacingRight ? 1 : -1), ForceMode2D.Impulse);
        animator.SetInteger("Attack", 1);
        ps.transform.localScale = new Vector3((IsFacingRight ? 1 : -1), 1, 1);
        ps.gameObject.SetActive(true);
        ddCollider.SetActive(true);
        ps.Play();
        isDashing = true;
    }

    
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (attackPoint.position, attackRange);
    }

}
