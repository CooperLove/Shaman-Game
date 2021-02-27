using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStopCrow : MonoBehaviour
{
    public GameObject luz;
    public GameObject crow;

    private void OnParticleSystemStopped() {
        luz.SetActive(true);
        Destroy(gameObject, 3);
    }

    public void DestroyLight () {
        luz.SetActive(false);
        GameObject g = Instantiate(crow, transform.position, transform.rotation);
        g.SetActive(true);
    } 
}
