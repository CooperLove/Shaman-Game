using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStop : MonoBehaviour
{
    EagleAttacks eagle;
    [SerializeField] private HighArrow arrow = null;

    private void Start() {
        eagle = Player.Instance.GetComponent<EagleAttacks>();
    }
    private void OnParticleSystemStopped() {
        Debug.Log("Stopped");
        arrow?.gameObject.SetActive(true);
    }
}
