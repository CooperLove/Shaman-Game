using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileHeadPhysics : MonoBehaviour
{
    public GameObject ps;
    public Transform point;
    
    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("Croc Head Center");
        point = GameObject.Find("CrocHighPos").GetComponent<Transform>();
    }

    public void Use (){
       GameObject g = Instantiate(ps, point.position, ps.transform.rotation);
       CrocHeadCallback callback = g.transform.GetChild(0).gameObject.AddComponent<CrocHeadCallback>();
       callback.point = point;
       callback.velocity = 185;
       Destroy(g, 3f);
    }
    
    
}
