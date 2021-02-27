using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemRotation : MonoBehaviour
{
    public ParticleSystem ps;
    public float x, y,z;
    public bool increase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var shape = ps.shape;
        Debug.Log(shape.rotation);
        Vector3 v = new Vector3(x,y,z);
        v *= Time.deltaTime;
        if (shape.rotation.z > 90 && increase)
            increase = false;
        if (shape.rotation.z < -90 && !increase)
            increase = true;

        if (increase)
            shape.rotation += v;
        else
            shape.rotation -= v;
        Debug.Log(shape.rotation);
    }
}
