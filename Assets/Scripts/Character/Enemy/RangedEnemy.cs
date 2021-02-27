using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    protected override void UnblockableAttack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Follow()
    {
        throw new System.NotImplementedException();
    }

    public override void Patrol()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator WalkAway()
    {
        throw new System.NotImplementedException();
    }

    protected override void Idle()
    {
        throw new System.NotImplementedException();
    }

    public  override void OnBeingHit(Vector3 offset, float duration, bool stun)
    {
        throw new System.NotImplementedException();
    }
}
