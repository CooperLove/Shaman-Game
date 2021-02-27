using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserMechanic : SkillMechanic
{

    [SerializeField] private float startDelay = 0.25f;
    [SerializeField] private Vector2 area = new Vector2();
    [SerializeField] private Vector2 offset = new Vector2();
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine (Damage());
    }

    private IEnumerator Damage (){
        yield return new WaitForSeconds (startDelay);
        ApplyDamage();   
    }

    public override void ApplyDamage (){
        var enemies = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y) + offset, area, 0, 1 << 9);

        foreach (var enemy in enemies)
        {
            var e = enemy.GetComponent<Enemy>();
            StartCoroutine(Debuff.Slow(e, 45, 5));
            e.TakeDamage(Player.Instance.playerInfo.CalculatePhysicalDamage());
        }
    }

    public void ActivateCascade(int index)
    {
        var child = transform.GetChild(index);
        var rayCastHit2D = Physics2D.Raycast(child.position, Vector2.down, 15f, 1 << 10);

        child.gameObject.SetActive(rayCastHit2D.collider != null);

        Debug.Log($"Cascade {child.name} is {child.gameObject.activeInHierarchy}");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position + new Vector3(offset.x, offset.y, 0), area);
    }
}
