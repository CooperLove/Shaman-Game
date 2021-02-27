using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public TigerAttacks player = null;
    public float velocity = 0;
    public float timer = 0;
    public float t = 0;
    public float destroyTime = 0;
    public float amp = 0, mag = 0, dur = 0;
    private bool hit = false;
    [SerializeField] Space space = Space.World;

    private void Start() {
        Destroy(gameObject, destroyTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (t < timer){
            t+= Time.deltaTime;
            return;
        }
        if (!hit)
            transform.Translate(Vector3.right * velocity * Time.fixedDeltaTime, space);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Ground"))
        {
            hit = true;
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera(amp, mag, dur));
            //transform.localRotation = Quaternion.Euler(0, 0, -90);
            //transform.position = new Vector3 (transform.position.x, 4.5f, transform.position.z);
            //Player.Instance.GetComponent<EagleAttacks>().ActivateDpsArea();
            Destroy(gameObject);
        }
    }
    private void OnDestroy() {
        //Player.Instance.GetComponent<EagleAttacks>().DeactivateDpsArea();
    }
}
