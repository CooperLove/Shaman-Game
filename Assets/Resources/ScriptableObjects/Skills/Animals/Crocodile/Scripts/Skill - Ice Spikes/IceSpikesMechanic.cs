using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikesMechanic : SkillMechanic
{
    // Start is called before the first frame update
    private void Start()
    {
        var isFacingRight = Player.Instance.IsFacingRight;
        
        if (!isFacingRight)
            transform.localScale = new Vector3(-1, 1, 1);
        
        var transforms = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in transforms)
        {
            var transform1 = ps.transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(isFacingRight ? localScale.x : -localScale.x, localScale.y, 1);
            ps.transform.localScale = localScale;
        }
    }
    
    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            ApplyDamage(other.GetComponent<Enemy>(), 10);
        }
    }
}
