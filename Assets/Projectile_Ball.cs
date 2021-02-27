using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Ball : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    private Rigidbody2D rb;
    [SerializeField]private float destroyTimer = 0, time = 0;

    public Vector2 Force { get => force; set => force = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Force;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (! (time > destroyTimer)){
            time += Time.fixedDeltaTime;
        }else{
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Player"){
            Player.Instance.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
