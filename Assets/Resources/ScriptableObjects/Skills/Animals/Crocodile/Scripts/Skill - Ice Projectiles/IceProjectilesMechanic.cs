using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectilesMechanic : MonoBehaviour
{
    public int numProjectiles = 1;
    private List<Transform> children = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            children.Add(transform.GetChild(i));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ThrowProjectile (int index) {
        // Debug.Log($"Child {index} => {index+1} >= {numProjectiles} = {index + 1 >= numProjectiles}");
        children[index]?.gameObject.SetActive(index + 1 <= numProjectiles);
    } 
    
}
