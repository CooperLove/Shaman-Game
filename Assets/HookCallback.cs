using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCallback : MonoBehaviour
{
    [SerializeField] private BasicAttack_Hook hook;
    private void Awake() {
        hook = GetComponentInParent<BasicAttack_Hook>();
    }
    private void OnParticleSystemStopped() {
        hook.Throw();
    }
}
