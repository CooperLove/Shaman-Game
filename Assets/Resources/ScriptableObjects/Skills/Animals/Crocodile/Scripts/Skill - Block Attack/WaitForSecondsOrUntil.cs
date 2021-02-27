using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsOrUntil : CustomYieldInstruction
{

    private float m_Timer;
    private float timer;
    private Func<bool> m_Predicate;

    public WaitForSecondsOrUntil (float waitTime, Func<bool> predicate)
    {
        m_Timer = waitTime;
        m_Predicate = predicate;
        timer = 0f;
    }

    public override bool keepWaiting {
        get
        {
            timer += Time.deltaTime;
            var b = !m_Predicate();
            // Debug.Log($"Coroutine: {b} && {timer < m_Timer} => Continue {b && timer < m_Timer}");
            return b && timer < m_Timer;
        }
    }
}
