using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{

    public Material snowMaterial;
    public float t;
    public float seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        snowMaterial.SetFloat("_SnowOpacity", Mathf.Lerp(0, 10, t));

        t += (1/seconds) * Time.deltaTime;

        if (t >= 1){
            t -= t;
        }
    }
}
