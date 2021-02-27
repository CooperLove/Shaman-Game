using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerHighAttack : MonoBehaviour
{
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    [SerializeField] private Transform currPoint = null;
    [SerializeField] private float dist = 0;
    [SerializeField]private float minDist = 0;
    [SerializeField] private float velocity = 0;
    [SerializeField] private int numDashes = 0;
    // Start is called before the first frame update
    void Start()
    {
        currPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPoint();
    }

    public void FollowPoint (){
        if (currPoint.Equals(pointA) && numDashes == 2){
            //Destroy(gameObject);
        }

        dist = Vector2.Distance(transform.position, currPoint.position);
        if (dist < minDist){
            currPoint = currPoint.Equals(pointA) ? pointB : pointA;
            numDashes++;   
        }
        
        transform.position = Vector2.MoveTowards(transform.position, currPoint.position, velocity * Time.fixedDeltaTime);
    }
}
