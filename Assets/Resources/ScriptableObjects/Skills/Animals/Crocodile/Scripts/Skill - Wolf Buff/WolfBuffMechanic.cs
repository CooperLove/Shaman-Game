using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBuffMechanic : SkillMechanic
{
    [SerializeField] private WolfBuff wolfBuff;
    [SerializeField] private float startDelay = 0.25f;

    private bool applied = false;

    private Coroutine buffTimer;

    private AutoDestroy autoDestroy;
    // Start is called before the first frame update
    private void Start()
    {
        autoDestroy = GetComponent<AutoDestroy>();
        wolfBuff = Resources.Load("ScriptableObjects/Skills/Animals/Crocodile/Scripts/Skill - Wolf Buff/Wolf Buff") as WolfBuff;
        if (wolfBuff) 
            wolfBuff.BuffObject = gameObject;
        
        StartCoroutine(ApplyBuff());
    }

    private void OnEnable()
    {
        transform.position = new Vector3(Player.Instance.transform.position.x, transform.position.y, 0);
        StartCoroutine(ApplyBuff());
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }

    public void Buff() => StartCoroutine(ApplyBuff());

    private IEnumerator ApplyBuff()
    {
        yield return new WaitForSeconds(startDelay);
        
        switch (applied)
        {
            case false:
                Player.Instance.playerInfo.CcDurationReduction += wolfBuff.CcReduction;
                break;
            case true when buffTimer != null:
                StopCoroutine(buffTimer);
                autoDestroy.ResetTimer();
                break;
        }

        buffTimer = StartCoroutine(BuffTimer());

    }

    private IEnumerator BuffTimer()
    {
        applied = true;
        autoDestroy.destroyTime = wolfBuff.BuffDuration;
        
        yield return new WaitForSeconds(wolfBuff.BuffDuration);
        
        Player.Instance.playerInfo.CcDurationReduction -= wolfBuff.CcReduction;
        applied = false;
    }
}
