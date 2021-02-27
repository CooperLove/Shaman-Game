using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePhysics2D : MonoBehaviour
{
    [Header("Setup Basic Attack")] 
    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = false;
    
    [Space(20)]
    public Vector3 attackPoint = new Vector2(17f, 0f);
    [SerializeField] private float radius = 15;
    [SerializeField] private Vector3 onHitOffset = new Vector3(15f, 0, 0);
    [SerializeField] private float timerSpeed = 2f;
    
    private Vector2 pos;
    private Player player;
    private Coroutine onHit = null;
    private LayerMask enemyLayer;
    // private Coroutine stunEnemyOnAir = null;
    
    [Header("Knock Up variables")]
    [SerializeField] private bool knockUp = false;
    [SerializeField] private Vector3 knockUpHeight = new Vector3();
    [SerializeField] private float knockDuration = 0f;
    [SerializeField] private Vector3 knockUpForce = new Vector3();
    
    private void Awake() {
        player = Player.Instance;
        radius = 8.75f;
        //attackPoint = new Vector3(20f, 0,0);
        enemyLayer = 1 << 9;
        onHitOffset = new Vector3(8.5f, 0, 0);
        timerSpeed = 2f;
        
        SetupBasicAttack();
        ApplyDamage();
    }

    private void SetupBasicAttack()
    {
        if (player.IsFacingRight)
            return;
        
        var localScale = transform.localScale;

        transform.localScale = new Vector3(rotateX ? -localScale.x : localScale.x, rotateY ? -localScale.y : localScale.y, 1);
        var childScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = new Vector3(rotateX ? -childScale.x : childScale.x, rotateY ? -childScale.y : childScale.y, 1);
    }

    private void ApplyDamage()
    {
        var position = Player.Instance.transform.position;
        pos = Player.Instance.IsFacingRight ? position + attackPoint : position - attackPoint;
        var enemies = Physics2D.OverlapCircleAll(pos, radius, enemyLayer, -1000, 1000);
        // var s = "dsadasdasdsadsadsa";
        if (onHit != null)
            StopCoroutine(onHit);
        
        var areAllEnemiesStatic = true;

        foreach (var col in enemies)
        {
            var e = col.GetComponent<Enemy>();
            e.TakeDamage(Player.Instance.playerInfo.CalculateBasicAttackDamage());
            BasicAttack_Handler.Instance?.OnHitCharacter?.Invoke(e);

            if (areAllEnemiesStatic && !e.IsStatic)
                areAllEnemiesStatic = false;

            if (player.OnGround && !knockUp)
                e.OnBeingHit(onHitOffset, timerSpeed, !knockUp);
            // else 
            //     e.OnAirHit();
            //
            if (!knockUp)
                continue;
            
            e.StartCoroutine(Debuff.KnockUp(e, knockUpHeight, knockDuration, knockUpForce));
        }

        if (!areAllEnemiesStatic && enemies.Length > 0 && player.OnGround && !knockUp)
            onHit = StartCoroutine(OnHit());
        
        if (!knockUp || enemies.Length == 0) 
            return;
        
        player.StartCoroutine(Debuff.Jump(player, knockUpHeight, knockDuration, knockUpForce)); 
    }

    private IEnumerator StunTarget (Rigidbody2D target, float airDuration){
        
        target.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(airDuration);
        target.constraints = RigidbodyConstraints2D.FreezeRotation;
        target.velocity += Physics2D.gravity;
    }

    
    /// <summary> Faz com que o player se mova quando acertar algum inimigo </summary>
    private IEnumerator OnHit()
    {
        var playerTransform = player.transform;
        var position = playerTransform.position;
        var rayCastHit2D = Physics2D.Raycast(position, 
            player.IsFacingRight ? Vector2.right : Vector2.left, 
            onHitOffset.x * 2f, 1 << 10);
        
        var point = position + (player.IsFacingRight ? onHitOffset : -onHitOffset);
        if (rayCastHit2D)
        {
            point = rayCastHit2D.point;
            if (rayCastHit2D.distance <= onHitOffset.x * 2)
                yield break;
        }

        var startValue = position;
        var endValue = point;

        var timer = 0f;
        
        while (timer <= 1f)
        {
            var xPos = Mathf.Lerp(startValue.x, endValue.x, timer);
            position = new Vector3(xPos, position.y, position.z);
            playerTransform.position = position;
            
            timer += timerSpeed * Time.deltaTime;
            yield return null;
        }
        
        playerTransform.position = point;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        // if (Player.Instance != null)
        //     Gizmos.DrawWireSphere(pos, radius);
        // else
            Gizmos.DrawWireSphere(transform.position + attackPoint, radius);

        Gizmos.color = Color.cyan;
        
        var transformPosition = transform.position;
        Gizmos.DrawLine(transformPosition, transformPosition + knockUpHeight);
        
    }
}
