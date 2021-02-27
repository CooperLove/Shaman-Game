using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollow : MonoBehaviour
{
    private bool follow = false;
    private Transform target = null;

    // Update is called once per frame
    void Update()
    {
        if (follow && target != null){
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }

    public void Follow (Transform target){
        this.target = target;
        follow = true;
    }


}
