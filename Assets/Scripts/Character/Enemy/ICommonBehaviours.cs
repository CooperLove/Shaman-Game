using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommonBehaviours 
{
    void TakeDamage (Enemy e, int h);
    void OnDie (Enemy e);
    IEnumerator Flash (Enemy e);
    IEnumerator HitPhysics (Enemy e);
    void ChangeState (IState currentState, IState newState, Enemy e);
    void FlipSprite (Enemy e, Transform t);
}
