using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowMediumAttack : MonoBehaviour
{
    public float velocity;
    public float destroyTime;

    private void Start() {
        Destroy(gameObject, destroyTime);
        transform.localScale = Player.Instance.IsFacingRight ? new Vector3(5,5,5) : new Vector3(-5,5,5);
        velocity = Player.Instance.IsFacingRight ? velocity : -velocity;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.fixedDeltaTime);
    }
}
