using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform topOfWall = null;
    // Start is called before the first frame update
    void Start()
    {
        topOfWall = transform.GetChild(0);
    }
}
