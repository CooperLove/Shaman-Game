using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCallback : MonoBehaviour
{
    public bool isCollision = false;
    public string targetTag = "";
    public bool destroyOnHit = false;
    public bool destroyChildrenOnHit = false;
    public int listSize = 0;
    public List<GameObject> children = new List<GameObject>();

    public bool stopOnFirstEnemyHit = false;
    public float rayDistance = 0.0f;

    public bool isAOE = false;
    public float damageDelay = 0f;
    public Vector3 aoeCenterOffset = new Vector3();
    public Vector2 aoeArea = new Vector2();

    public bool isDOT = false;
    public int numTicks = 0;
    public float tickInterval = 0.0f;

    public bool stun = false;
    public float stunDuration = 0.0f;
    public bool slow = false;
    public float slowPercentage = 0.0f;
    public float slowDuration = 0.0f;
    public bool root = false;
    public float rootDuration = 0.0f;
    public bool knockUp = false;

    [SerializeField] private  Enemy enemy = null;
    private Player player;
    public LayerMask targetLayer;

    private void Awake() {
        player = Player.Instance;
    }
    private void Start() {
        if (isAOE)
            StartCoroutine(AoeDamage());
    }

    private void OnEnable() {
        enemy = null;
    }

    private void SingleTarget (){
        var enemies = Physics2D.RaycastAll(transform.position, player.IsFacingRight ? Vector2.right : Vector2.left, rayDistance, targetLayer);

        if (enemies.Length == 0)
            return;

        if (stopOnFirstEnemyHit){
            enemies[0].collider.GetComponent<Character>().TakeDamage(17);
            return;
        }

        foreach (var col in enemies)
        {
            col.collider.GetComponent<Character>().TakeDamage(17);
        }
    }

    private IEnumerator AoeDamage (){
        if (damageDelay > 0)
            yield return new WaitForSeconds(damageDelay);

        var enemies = Physics2D.OverlapBoxAll(aoeCenterOffset + transform.position, aoeArea, 0, targetLayer);

        foreach (var col in enemies)
        {
            var e = col.GetComponent<Enemy>();
            e.TakeDamage(10);
            ApplyDebuffs(e);
        }

        if (isDOT){
            var tick = 1;
            while (tick++ < numTicks){
                enemies = Physics2D.OverlapBoxAll(aoeCenterOffset + transform.position, aoeArea, 0, targetLayer);
                foreach (var col in enemies)
                {
                    var e = col.GetComponent<Character>();
                    e.TakeDamage(10);
                    ApplyDebuffs(e);
                }
                yield return new WaitForSeconds(tickInterval);
            }
        }
        

        yield return null;
    }

    private void ApplyDebuffs (Character c){
        if (stun)
            c.StartCoroutine(Debuff.Stun(c, stunDuration));
        if (slow)
            c.StartCoroutine(Debuff.Slow(c, slowPercentage, slowDuration));
        if (root)
            c.StartCoroutine(Debuff.Root(c, rootDuration));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isCollision)
            return;
        
        var tag = targetTag.Equals("") ? "Enemy" : targetTag;
        if (other.tag.Equals(tag)){
            ApplyDamage(other, tag);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!isCollision)
            return;

        var tag = targetTag.Equals("") ? "Enemy" : targetTag;
        if (other.collider.tag.Equals(tag)){
            
            ApplyDamage(other, tag);
        }
    }

    private void ApplyDamage (Collision2D other, string tag){
        switch (tag)
        {
            case "Player":
                player.TakeDamage(25);
                break;
            case "Enemy": 
                Enemy e = other.gameObject.GetComponent<Enemy>();
                if (enemy && enemy.Equals(e))
                    return;

                enemy = e;
                e?.TakeDamage(10);
                ApplyDebuffs(e);
                break;
        }

        if (destroyOnHit)
            Destroy(gameObject);
    }

    private void ApplyDamage (Collider2D other, string tag){
        switch (tag)
        {
            case "Player":
                player.TakeDamage(25);
                break;
            case "Enemy": 
                Enemy e = other.GetComponent<Enemy>();
                if (enemy && enemy.Equals(e))
                    return;

                enemy = e;
                e?.TakeDamage(10);
                ApplyDebuffs(e);
                break;
        }

        if (destroyOnHit)
            Destroy(gameObject);
    }
    

    private void OnDrawGizmosSelected() {
        if (isAOE)
            Gizmos.DrawWireCube(aoeCenterOffset + transform.position, aoeArea);

        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position, new Vector3(position.x + rayDistance, position.y, position.z));
        Gizmos.DrawLine(position, new Vector3(position.x - rayDistance, position.y, position.z));
    }
}
