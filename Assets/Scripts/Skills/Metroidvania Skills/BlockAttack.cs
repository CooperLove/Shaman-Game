using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockAttack : MonoBehaviour
{
    private Action<int> OnBlockEffect;

    private const float blockBarFullHealth = 100f;
    private const float onEmptyBarFillTimer = 3f;
    private const float onExitBlockModeFillTimer = 1.5f;
    private const int maxHistBlocked = 5;
    private float barDamagePerHit = 0f;
    
    public GameObject block = null;
    public Transform blockBar = null;
    [SerializeField] private int spRecorvered = 0;
    [SerializeField] private float nearDist = 0;
    [SerializeField] private float farDist = 0;
    [SerializeField] private float nearStunDuration = 0.0f;
    [SerializeField] private float farStunDuration = 0.0f;
    [SerializeField] private Vector3 centerOffset = new Vector3 ();
    [SerializeField] private float radius = 0.0f;
    [SerializeField, Range (0.25f, 0.75f)] private float gameSlowedTime = 0.0f;
    [SerializeField, Range (0.25f, 0.75f)] private float timeScale = 0.0f;

    private bool resetingBar = false;

    private Player player = null;
    private PlayerInfo playerInfo = null;
    private Coroutine blockCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerInfo = player.playerInfo;
        blockBar = Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(x => x.name.Equals("Player Block Bar Fill"));
        blockBar.localScale = Vector3.one;

        barDamagePerHit = 1f / maxHistBlocked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.IgnoreInputs())
            return;

        if (Input.GetMouseButtonDown(1) && player.OnGround && blockCoroutine == null){
            player.IsBlocking = true;

            if (resetingBar)
                return; 

            Block();
        }

        if (Input.GetMouseButtonUp(1)){
            player.IsBlocking = false;
            if (blockBar.localScale.x > 0)
                StartCoroutine (OnResetNonEmptyBar());
        }
    }

    private void Block (){
        // VERIFY IF HAS ENOUGH SP
        if (playerInfo.Stamina < spRecorvered){
            blockCoroutine = null; 
            return;
        }

        playerInfo.Stamina -= spRecorvered;

        // GET ALL ENEMIES ARROUND
        var colliders = Physics2D.OverlapCircleAll (transform.position + centerOffset, radius, 1 << 9);

        if (colliders.Length == 0){
            blockCoroutine = null; 
            return;
        }

        var enemies = OrderEnemiesByDistance (colliders);

        var perfBlock = FindAnyEnemyToBePerfectlyBlocked(enemies);
        // PERFECT BLOCK
        if (perfBlock.Key != null && blockCoroutine == null){
            var blockedDamage = perfBlock.Key.GetComponent<Enemy>().enemyInfo.CalculateBasicAttackDamage();
            blockCoroutine = StartCoroutine( OnPerfectBlock(enemies, blockedDamage) );
            return;
        }

        // NORMAL BLOCK
        // if (FindAnyEnemyToBeBlocked(enemies).Key != null){
        //     StartCoroutine( OnBlock(enemies) );
        // }else
        //     return;
        

    }

    public void OnBlock (){

        if (blockBar.localScale.x <= 0 || blockBar.localScale.x < barDamagePerHit){
            player.IsBlocking = false;
            StartCoroutine (OnResetNonEmptyBar());
            return;
        }

        SetBlockBarFill (-barDamagePerHit);
    }

    private void SetBlockBarFill (float amount){
        if (resetingBar)
            return;

        var fill = Mathf.Clamp01 (blockBar.localScale.x + amount);
        blockBar.localScale = new Vector3(fill < 0.01f ? 0 : fill, 1, 1);

        if (blockBar.localScale.x == 0){
            player.IsBlocking = false;
            StartCoroutine ( OnResetEmptyBar() );
        }
    }

    private IEnumerator OnPerfectBlock (IOrderedEnumerable<KeyValuePair<Collider2D, float>> enemies, int blockedDamage){
        OnBlockEffect?.Invoke(blockedDamage);

        // STUN ENEMIES
        foreach (var p in enemies)
        {
            var e = p.Key.GetComponent<Enemy>();
            // var dist = Vector2.Distance (transform.position, e.transform.position);
            var dist = p.Value;

            var dur = nearStunDuration;
            if (dist <= nearDist){
                player.StartCoroutine (Debuff.Stun(e, dur));
            }else{
                var d = dist - nearDist;
                var v = d / (radius - nearDist);
                dur = Mathf.Lerp (nearStunDuration, farStunDuration, v);
                player.StartCoroutine (Debuff.Stun(e, dur));
            }
            e.OnBeingBlocked (dur);

        }

        // INSTANTIATE ANIMATION
        var ani = Instantiate (block, transform.position, block.transform.rotation);
        ani.SetActive(true);
        Destroy(ani, 1f);

        // SLOW GAME
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime (gameSlowedTime);
        Time.timeScale = 1;

        // RECORVER SP
        playerInfo.Stamina += spRecorvered/2;


        // RESET COROUTINE
        yield return new WaitForSeconds (gameSlowedTime);
        blockCoroutine = null;
    }

    private KeyValuePair<Collider2D, float> FindAnyEnemyToBePerfectlyBlocked (IOrderedEnumerable<KeyValuePair<Collider2D, float>> enemies){

        // CHECK IF ANY ENEMY IS ATTACKING AND CAN BE BLOCKED
        var enemyAttacking = enemies.FirstOrDefault ((x) => x.Key.GetComponent<Enemy>().CanBePerfectlyBlockedCheck());

        if (enemyAttacking.Key == null)
            return new KeyValuePair<Collider2D, float>(null, -1);

        Debug.Log($"Perfect Blocking {enemyAttacking.Key} {enemyAttacking.Key.GetComponent<Enemy>().CanBePerfectlyBlocked} {enemyAttacking.Key.GetComponent<Enemy>().WasBlocked}");

        return enemyAttacking;
    }

    private KeyValuePair<Collider2D, float> FindAnyEnemyToBeBlocked (IOrderedEnumerable<KeyValuePair<Collider2D, float>> enemies){

        // CHECK IF ANY ENEMY IS ATTACKING AND CAN BE BLOCKED
        var enemyAttacking = enemies.FirstOrDefault ((x) => x.Key.GetComponent<Enemy>().CanBeBlockedCheck());

        if (enemyAttacking.Key == null)
            return new KeyValuePair<Collider2D, float>(null, -1);

        Debug.Log($"Blocking {enemyAttacking.Key} {enemyAttacking.Key.GetComponent<Enemy>().CanBeBlocked} {enemyAttacking.Key.GetComponent<Enemy>().WasBlocked}");

        return enemyAttacking;
    }

    private IOrderedEnumerable<KeyValuePair<Collider2D, float>> OrderEnemiesByDistance (Collider2D[] colliders)
    {
        var pairs = new List<KeyValuePair<Collider2D, float>>();
        var playerPos = player.transform.position;
        colliders.ToList().ForEach( (x) => pairs.Add(new KeyValuePair<Collider2D, float>(x, Vector2.Distance(x.transform.position, playerPos))));

        return pairs.OrderBy( (x) => x.Value);
    }

    private void OnDrawGizmosSelected (){  
        var position = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (position + centerOffset, radius);

        Gizmos.color = Color.cyan;
        var nearDir = new Vector3 (position.x + nearDist, position.y, position.z);
        var neardEsq = new Vector3 (position.x - nearDist, position.y, position.z);
        Gizmos.DrawLine (position, nearDir);
        Gizmos.DrawLine (position, neardEsq);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine (nearDir, new Vector3 (nearDir.x + farDist,  position.y, position.z));
        Gizmos.DrawLine (neardEsq, new Vector3 (neardEsq.x - farDist,  position.y, position.z));
    }

    private IEnumerator OnResetEmptyBar (){
        resetingBar = true;
        
        var timer = 0f;
        while (blockBar.localScale.x < 1){
            blockBar.localScale = new Vector3(Mathf.Lerp (0, 1, timer), 1, 1);
            timer += 1f / onEmptyBarFillTimer * Time.deltaTime;
            yield return null;
        }

        resetingBar = false;
    }

    private IEnumerator OnResetNonEmptyBar (){
        resetingBar = true;
        yield return new WaitForSecondsOrUntil(0.5f, () => player.IsBlocking);
        // Debug.Log($"Player Is blocking {player.IsBlocking}");
        resetingBar = false;

        var timer = 0f;
        var currentFill = blockBar.localScale.x;
        while (blockBar.localScale.x < 1 && !player.IsBlocking){
            // blockBar.localScale = new Vector3(Mathf.Lerp (currentFill, 1, timer), 1, 1);
            blockBar.localScale = new Vector3(Mathf.Clamp01(blockBar.localScale.x + timer), 1, 1);
            timer += 1f / onExitBlockModeFillTimer * Time.deltaTime;
            yield return null;
        }
    }

    public void AddEffect (Action<int> effect){
        OnBlockEffect += effect;
    }
}
