using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Collider2D _collider = null;
    private AnimationsScript _animScript = null;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask enemies = 0;
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 0;
    [SerializeField] private float freezeTimer = 0;
    private Player player;

    private void Start() {
        player = Player.Instance;
        //_collider = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _animScript = GetComponent<AnimationsScript>();
    }
    private void Update() {
        if (player.IgnoreCommands)
            return;

        //GetInput ();

    }

    public void EnterAirLayer () => _animScript.animator.SetLayerWeight(1, 1);
    public void ExitAirLayer () {
        _animScript.animator.SetLayerWeight(1, 0);
        _animScript.OnAttackExit();
    }

    private void GetInput(){
        if (_animScript.attacking)
            return;

        if (!Player.Instance.OnGround){
            
            if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad4))){
                _animScript.OnAttackEnter(5);
                //rb.velocity = Vector2.zero;
            }else if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Keypad5))){
                _animScript.OnAttackEnter(6);
                //rb.velocity = Vector2.zero;
            }
            return;
        }

        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad4))){
            _animScript.OnAttackEnter(1); 
            rb.velocity = Vector2.zero;
        }else
        if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Keypad5))){
            _animScript.OnAttackEnter(2); 
            rb.velocity = Vector2.zero;
        }else
        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Keypad6))){
            _animScript.OnAttackEnter(3); 
            rb.velocity = Vector2.zero;
        }else
        if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Keypad8))){
            _animScript.OnAttackEnter(4); 
            rb.velocity = Vector2.zero;
        }
    }
    private void Attack (){
        Collider2D[] en = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);
        if(en.Length > 0){
            //StartCoroutine(OnAttackFreeze());
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
        }
        foreach (Collider2D enemy in en)
        {
            Debug.Log(enemy.gameObject.name);
            enemy.gameObject.TryGetComponent<Enemy>(out Enemy e);
            enemy.gameObject.TryGetComponent<Boss>(out Boss b);
            e?.TakeDamage(10);
            b?.TakeDamage(10);
        }
    }

    private IEnumerator OnAttackFreeze (){
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(freezeTimer);
        Time.timeScale = 1;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag.Equals("Enemy")){
            //other.transform.SendMessage("LoseHP", 5);
            //StartCoroutine (Debuff.Root(other.GetComponent<Character>(), 2));
        }
    }
    public void ActivateCollider (){
        _collider.enabled = true;
        Attack();
    }
    public void DeactivateCollider (){
        _collider.enabled = false;
        _animScript.OnAttackExit();
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
