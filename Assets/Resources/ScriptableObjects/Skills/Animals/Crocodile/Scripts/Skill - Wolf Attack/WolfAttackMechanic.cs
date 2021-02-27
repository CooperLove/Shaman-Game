using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackMechanic : SkillMechanic
{
    [SerializeField] private GameObject paws;
    [SerializeField] private float animationDuration = 0.25f;
    [SerializeField] private int numberOfTicks = 6;
    [SerializeField] private float damageTickInterval = 0.25f;
    [SerializeField] private Vector3 dashOffset = new Vector3();
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float range = 100f;

    private Player player;
    private GhostController ghostController;
    [SerializeField] private LayerMask enemyLayer;

    private GameObject trail = null;
    // Start is called before the first frame update
    private void Start()
    {
        player = Player.Instance;
        ghostController = player.GetComponent<GhostController>();
        enemyLayer = 1 << 9;
        paws = transform.Find("Nails")?.gameObject;
        trail = transform.Find("Trail")?.gameObject;
        
        StartCoroutine(ApplyDamage(FindEnemy()));
    }  

    private void OnEnable() {
        if (player)
            StartCoroutine(ApplyDamage(FindEnemy())); 
    }
    

    private Enemy FindEnemy()
    {
        var rayCastHit2D = Physics2D.Raycast(transform.position, 
                                            player.IsFacingRight ? Vector2.right : Vector2.left,
                                            range, enemyLayer);

        return rayCastHit2D ? rayCastHit2D.collider.GetComponent<Enemy>() : null;
    }

    private IEnumerator ApplyDamage (Enemy e)
    {
        GameStatus.IgnoreCommands(true, this);
        SetupPawPosition(e);

        if (paws)
            paws.SetActive(false);
        
        yield return new WaitForSeconds(animationDuration);

        if (!e){
            OnNoEnemyHit();   
            yield break;
        }

        var t = InstantiateTrail ();

        PlayerDash (e);

        yield return new WaitUntil( () => !player.IsDashing );

        StartCoroutine(Debuff.Stun(e, numberOfTicks * damageTickInterval));
        paws.SetActive(true);
        
        var numTicks = 0;
        while (numTicks++ < numberOfTicks)
        {
            e.TakeDamage(10, showHitFX:false);
            yield return new WaitForSeconds(damageTickInterval);
        }

        OnEnd (t);
    }

    private void SetupPawPosition (Enemy e)
    {
        if (!paws || !e)
            return;

        paws.transform.position = e.transform.position;
    }

    private void PlayerDash (Enemy e){
        var enemyPosition = e.transform.position;
        var direction = player.IsFacingRight ? Vector2.right : Vector2.left;
        var enemyDashPoint = enemyPosition + (player.IsFacingRight ? dashOffset : -dashOffset);
        var distance = Vector2.Distance(enemyPosition, enemyDashPoint);
        var ray = Physics2D.Raycast(enemyPosition, direction, distance, 1 << 10);

        var finalPoint = ray.collider == null ? enemyDashPoint : new Vector3(ray.point.x, ray.point.y, 0);

        player.StartCoroutine(player.Dash(finalPoint , dashDuration));
        player.StartCoroutine(player.FadeSpriteColor(dashDuration, true));
        ghostController.ghostActive = true;
    }

    private GameObject InstantiateTrail (){
        if (!trail)
            return null;

        var t = Instantiate(trail, transform.position, trail.transform.rotation);
        var trailTransform = t.transform;
        trailTransform.SetParent(player.transform);
        trailTransform.localPosition = Vector3.zero;
        trailTransform.localScale = player.IsFacingRight ? Vector3.one : new Vector3 (-1, 1, 1);

        for (int i = 0; i < trailTransform.childCount; i++){
            trailTransform.GetChild(i).localScale = player.IsFacingRight ? Vector3.one : new Vector3 (-1, 1, 1);
        }

        var ray = Physics2D.Raycast(trailTransform.position, Vector2.down, 25f, 1 << 10);
        if (ray.collider){
            trailTransform.position = ray.point;
        }

        return t;
    }

    private void OnEnd (GameObject t){
        if (t)
            Destroy(t);
        GameStatus.IgnoreCommands(false, this);
        GameStatus.UsingSkill(false);
        ghostController.ghostActive = false;
        player.StartCoroutine(player.FadeSpriteColor(0.2f, false));
    }

    private void OnNoEnemyHit (){
        paws.SetActive(false);
        //Destroy(gameObject, 0.5f);
        GameStatus.IgnoreCommands(false, this);
        GameStatus.UsingSkill(false);
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        Gizmos.DrawLine(position, new Vector3(position.x + range, position.y, 0));
    }

    public override void ApplyDamage()
    {
        throw new NotImplementedException();
    }
}
