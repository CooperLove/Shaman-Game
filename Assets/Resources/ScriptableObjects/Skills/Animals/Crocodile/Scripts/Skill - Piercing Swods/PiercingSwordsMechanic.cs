using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSwordsMechanic : SkillMechanic
{

    [SerializeField] private Vector2 size = new Vector2();
    [SerializeField] private Vector3 offset = new Vector3();

    private LayerMask enemyLayer;
    // Start is called before the first frame update
    private void Start()
    {
        enemyLayer = 1 << 9;
        ApplyDamage();
    }
    
    public override void ApplyDamage()
    {
        var enemies = Physics2D.OverlapBoxAll(transform.position + offset, size, 0, enemyLayer);

        foreach (var coll in enemies)
            coll.GetComponent<Enemy>().TakeDamage(10);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
