using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighArrowCalllback : MonoBehaviour
{
    public GameObject arrow;
    public GameObject mainParticle;
    public ParticleSystem[] ps;
    public float amp, mag, dur;
    public Player player;
    public Vector2 force;
    public float interval;
    private void Start() {
        player = Player.Instance;
    }
    private void OnParticleSystemStopped() {
        GameObject g = Instantiate(arrow, transform.position, arrow.transform.rotation);
        g.SetActive(true);
        StartCoroutine(SimpleCameraShake.instance.ShakeCamera(amp, mag, dur));
        player.GetComponent<Rigidbody2D>().AddForce(force ,ForceMode2D.Force);
        player.StartDash(interval);
        mainParticle.SetActive(false);
        foreach (ParticleSystem p in ps)
        {
            p.transform.position = transform.position;
            p.gameObject.SetActive(true);
            p.Play();
        }
    }
}
