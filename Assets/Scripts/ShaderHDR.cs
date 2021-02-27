using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderHDR : MonoBehaviour
{
    public Material mat;

    [ColorUsage (true,true)]
    public Color color;

    // Update is called once per frame
    void Update()
    {
        mat.SetColor("_TintColor", color);
    }
}
