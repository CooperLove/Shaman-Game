using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardPhysics : MonoBehaviour
{
    [SerializeField] private float velocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Player.Instance.IsFacingRight ? Quaternion.Euler(0,0,45) : Quaternion.Euler (0,0,-45);
    }

    // Update is called once per frame
    void Update()
    {   
        transform.Translate(Vector3.down * velocity * Time.fixedDeltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Ground")){
            //Debug.Log($"Collider: {other.name} {other.transform.parent.gameObject.name}");
            StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
            Destroy(gameObject);
        }
    }
}
