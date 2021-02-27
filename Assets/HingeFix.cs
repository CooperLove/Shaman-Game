using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeFix : MonoBehaviour
{
    private HingeJoint2D hinge;
    private Vector2 anchor;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        anchor = hinge.anchor;
    }

    // Update is called once per frame
    void Update()
    {
        hinge.anchor = anchor;
    }
}
