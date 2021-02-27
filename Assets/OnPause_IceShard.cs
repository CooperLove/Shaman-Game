using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPause_IceShard : MonoBehaviour
{
    [SerializeField] private GameObject iceShard = null;
    private void OnParticleSystemStopped() {
        GameObject g = Instantiate(iceShard, transform.position, iceShard.transform.rotation);
        g.SetActive(true);
    }
}
