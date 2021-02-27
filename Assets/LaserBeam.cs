using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public ParticleSystem smoke;
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float raycastLength = 0.1f;
    [SerializeField] private float workTime = 0.1f;
    private float timer = 0.1f;
    private bool stop = false;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(StartParticles());
    }

    private void Update() {
        if (stop)
            return;

        var trans = transform;
        var position = trans.position;
        var right = trans.right;
        var ray = Physics2D.Raycast(position, right, 1000f, 1 << 10);
        Debug.DrawRay(position, right * raycastLength, Color.green, 0.5f);
        if (ray.collider){
            smoke.transform.position = ray.point;
        }

        timer += Time.deltaTime;
        if (!(timer >= workTime)) 
            return;
        timer -= timer;
        stop = true;
        smoke.gameObject.SetActive(false);

    }

    private void OnEnable() {
        smoke.gameObject.SetActive(true);
        StartCoroutine(StartParticles());
        stop = false;
        timer = 0;
    }

    private IEnumerator StartParticles (){
        yield return new WaitForSeconds(startDelay);
        smoke.Play();
    }
}
