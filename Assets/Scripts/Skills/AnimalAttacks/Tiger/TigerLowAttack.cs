using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerLowAttack : MonoBehaviour
{
    Player player = null;
    [SerializeField] private float velocity = 0;
    private Vector3 dir = new Vector3();
    [SerializeField] private float duration = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        dir = (player.IsFacingRight ? Vector3.right : Vector3.left);
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * velocity * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Enemy")){
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
