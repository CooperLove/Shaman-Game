using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Slash : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    [SerializeField] private float speed = 3.5f;
    public bool right = true;
    private void Start() {
        transform.localScale = new Vector3((right ? transform.localScale.x : -transform.localScale.x),transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((right ? Vector2.right : Vector2.left) * speed);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
