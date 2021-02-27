using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockUpArrow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float velocity;
    [SerializeField] private EagleAttacks eagle;
    private float dist;
    [SerializeField] private float minDist;
    // Start is called before the first frame update
    void Start()
    {
        eagle = Player.Instance.GetComponent<EagleAttacks>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
