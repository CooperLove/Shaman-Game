using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpectreMechanic : SkillMechanic
{
    [SerializeField] private ShieldSpectre spectre;
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Shield spectre");
        spectre = Resources.Load("ScriptableObjects/Skills/Animals/Crocodile/Scripts/Skill - Shield Spectre/Shield Spectre") as ShieldSpectre;
        
        var isFacingRight = Player.Instance.IsFacingRight;

        if (!isFacingRight)
            transform.parent.localScale = new Vector3(-1, 1, 1);
        
        var transforms = transform.parent.GetComponentsInChildren<ParticleSystem>();
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
        
    }

    private void ApplyDamage (Enemy e)
    {
        e.TakeDamage(spectre.Damage);
    }

    private void ApplySlow (Enemy e)
    {
        StartCoroutine(Debuff.Slow(e, spectre.SlowPercentage, spectre.SlowDuration));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (!other.CompareTag("Enemy")) return;

        var e = other.GetComponent<Enemy>();
        ApplyDamage(e);
        ApplySlow(e);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.name);
    }
}
