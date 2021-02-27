using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack_Spiral : MonoBehaviour
{
    [SerializeField] private List<float> timers = new List<float>();
    private Player player;
    private PlayerInfo playerInfo;

    [SerializeField] private Vector3 offset = new Vector3();
    [SerializeField] private Vector3 boxSize = new Vector3();
    [SerializeField] private float animationDuration = 2f;
    // [SerializeField] private 
    // Start is called before the first frame update
    private void Start()
    {
        player = Player.Instance;
        playerInfo = player.playerInfo;
        animationDuration = 2f;
        
        Setup();
        StartCoroutine(ApplyDamage());
        StartCoroutine(Animation());
    }


    private void Setup()
    {
        if (player == null)
            return;
        
        if (player.IsFacingRight)
            return;

        var childrenTransform = transform.GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in childrenTransform)
        {
            var localScale = ps.transform.localScale;
            ps.transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    private IEnumerator ApplyDamage()
    {
        if (player == null || timers.Count < 5)
            yield break;

        
        var position = transform.position;
        var off = player.IsFacingRight ? offset : -offset;
        var point = player.IsFacingRight ? position + off : position - off;
        const int enemyLayer = 1 << 9;
        
        yield return new WaitForSeconds(timers[0]);
        var hitbox = Physics2D.OverlapBoxAll(point, boxSize, 0, enemyLayer);

        foreach (var col in hitbox)
        {
            var enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(10);
        }
        
        yield return new WaitForSeconds(timers[1]);
        off = -off;
        point = position + off;
        hitbox = Physics2D.OverlapBoxAll(point, boxSize, 0, enemyLayer);
        
        foreach (var col in hitbox)
        {
            var enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(10);
        }
        
        yield return new WaitForSeconds(timers[2]);
        off = -off;
        point = position + off;
        hitbox = Physics2D.OverlapBoxAll(point, boxSize, 0, enemyLayer);
        
        foreach (var col in hitbox)
        {
            var enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(10);
        }
        
        yield return new WaitForSeconds(timers[3]);
        off = -off;
        point = position + off;
        hitbox = Physics2D.OverlapBoxAll(point, boxSize, 0, enemyLayer);
        
        foreach (var col in hitbox)
        {
            var enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(10);
        }
        
        yield return new WaitForSeconds(timers[4]);
        off = -off;
        point = position + off;
        hitbox = Physics2D.OverlapBoxAll(point, boxSize, 0, enemyLayer);
        
        foreach (var col in hitbox)
        {
            var enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(10);
        }

    }

    private IEnumerator Animation()
    {
        GameStatus.IgnoreCommands(true, this);
        yield return new WaitForSeconds(animationDuration);
        GameStatus.IgnoreCommands(false, this);
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(position + offset, boxSize);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(position - offset, boxSize);
    }
}
