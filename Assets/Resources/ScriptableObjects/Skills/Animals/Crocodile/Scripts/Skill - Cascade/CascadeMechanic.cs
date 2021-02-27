using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeMechanic : SkillMechanic
{

    [SerializeField] private float startDelay = 0.25f;
    [SerializeField] private Vector2 area = new Vector2();
    [SerializeField] private Vector2 hitBoxOffset = new Vector2();
    [SerializeField] private float initialOffset = 0f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float range = 100f;

    private Player player;
    [SerializeField] private LayerMask enemyLayer =  1 << 1;
    [SerializeField] private LayerMask groundLayer = 1 << 1;
    // Start is called before the first frame update
    private void Start()
    {
        player = Player.Instance;
        enemyLayer = 1 << 9;
        groundLayer = 1 << 10;
        StartCoroutine(Damage());
    }

    private void OnEnable()
    {
        StartCoroutine(Damage());
    }

    private IEnumerator Damage()
    {
        var pos = SetupPosition();
        yield return new WaitForSeconds(startDelay);
        ApplyDamage(pos);
    }

    private Vector3 SetupPosition()
    {
        if (!player)
            return new Vector3();
        
        var position = player.transform.position;

        var groundRay = Physics2D.Raycast(position, Vector2.down, range, groundLayer);

        var raycastHit2D = Physics2D.Raycast(position, player.IsFacingRight ? Vector2.right : Vector2.left, range, enemyLayer);

        if (!raycastHit2D)
        {
            position = new Vector3 (position.x + (player.IsFacingRight ? initialOffset : -initialOffset), groundRay.point.y, 0);
            transform.position = position;
            return position;
        }
        
        //Debug.Log($"Hit {raycastHit2D.collider.name}");
        position = new Vector3 (raycastHit2D.transform.position.x, groundRay.point.y, 0);
        transform.position = position;

        return position;
    }

    private void ApplyDamage (Vector3 position){
        if (!player)
            return;

        var enemies = Physics2D.OverlapBoxAll(new Vector2(position.x, position.y) + hitBoxOffset, area, 0, enemyLayer);

        foreach (var enemy in enemies)
        {
            //Debug.Log($"E => {enemy.name}");
            var e = enemy.GetComponent<Enemy>();
            StartCoroutine(Debuff.Slow(e, 25,3));
            e.TakeDamage(Player.Instance.playerInfo.CalculatePhysicalDamage());
        }


    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        var position = transform.position;
        Gizmos.DrawWireCube(position + new Vector3(hitBoxOffset.x, hitBoxOffset.y, 0), area);
        Gizmos.DrawWireSphere(position + new Vector3(initialOffset, 0, 0), radius);

        Gizmos.color = Color.cyan;
        if (player != null)
            Gizmos.DrawRay(position, (player.IsFacingRight ? Vector3.right : Vector3.left ) * range);
        else
            Gizmos.DrawRay(position, Vector3.right * range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(position, Vector3.down * range);
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }
}
