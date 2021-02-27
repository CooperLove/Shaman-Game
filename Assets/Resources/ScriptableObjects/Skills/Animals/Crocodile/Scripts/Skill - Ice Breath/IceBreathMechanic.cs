using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreathMechanic : SkillMechanic
{
    [SerializeField] private float startDelay = 0.25f;
    [SerializeField] private Vector2 area = new Vector2();
    [SerializeField] private Vector2 offset = new Vector2();
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine (Damage());
        var isFacingRight = Player.Instance.IsFacingRight;
        var transforms = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in transforms)
        {
            var transform1 = ps.transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(isFacingRight ? localScale.x : -localScale.x, localScale.y, 1);
            transform1.localScale = localScale;
        }

        offset = new Vector2(isFacingRight ? offset.x : -offset.x, offset.y);
    }

    private IEnumerator Damage (){
        yield return new WaitForSeconds (startDelay);
        ApplyDamage();   
    }

    public override void ApplyDamage (){
        var trans = transform;
        var position = trans.position;
        var enemies = Physics2D.OverlapBoxAll(new Vector2(position.x, position.y) + offset, area, 0, 1 << 9);

        foreach (var enemy in enemies)
        {
            var e = enemy.GetComponent<Enemy>();
            StartCoroutine(Debuff.Stun(e, 3));
            e.TakeDamage(Player.Instance.playerInfo.CalculatePhysicalDamage());
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position + new Vector3(offset.x, offset.y, 0), area);
    }
}
