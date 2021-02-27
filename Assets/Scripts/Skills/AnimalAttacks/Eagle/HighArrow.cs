using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighArrow : MonoBehaviour
{
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    [SerializeField] private Transform currPoint = null;
    [SerializeField] private float dist = 0;
    [SerializeField]private float minDist = 0;
    [SerializeField] private float velocity = 0;
    [SerializeField] private int numDashes = 0;
    public float rot = 0, speed = 0;
    public ParticleSystem ps = null;
    public float offset = 0;
    public GameObject cameraCollider = null;
    public GameObject center = null;
    public ParticleSystem trail = null;
    public LayerMask cameraLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        ContactFilter2D contact;
        pointB = Player.Instance.transform;
        contact.useLayerMask = true;
        contact.layerMask = cameraLayer;
        RaycastHit2D[] raycast = Physics2D.RaycastAll(transform.position, Player.Instance.IsFacingRight ? Vector3.right : Vector3.left, contact.layerMask);
            Debug.DrawRay(transform.position, new Vector3(50, 0, 0), Color.yellow, 5f);
        foreach (RaycastHit2D ray in raycast)
        {
            if (ray.transform.tag.Equals("MainCamera")){
                float xPos = Player.Instance.IsFacingRight ? ray.distance + transform.position.x + offset : 
                                                           -ray.distance + transform.position.x - offset;
                ps.transform.localScale = Player.Instance.IsFacingRight ? Vector3.one : new Vector3(-1, 1, 1); 
                pointA.transform.position = new Vector3( xPos, pointA.transform.position.y, pointA.transform.position.z);
                //pointB.transform.position = new Vector3(-ray.distance - transform.position.x - offset, pointB.transform.position.y, pointB.transform.position.z);
                Debug.Log($"{xPos} Distance: {ray.distance} => {ray.transform.name} {ray.transform.gameObject.layer} {cameraCollider.transform.position}");
                Debug.DrawRay(transform.position, Vector3.right * ray.distance, Color.blue, 5f);
            }
        }
        currPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        DoubleDash();
    }

    public void DoubleDash (){
        if (currPoint.Equals(pointA) && numDashes == 2){
            center.SetActive(false);
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            trail.Stop();
            var t = trail.velocityOverLifetime;
            t.enabled = false;
            //Destroy(transform.parent.gameObject, 0.5f);
            return;
        }

        dist = Vector2.Distance(transform.position, currPoint.position);
        if (dist < minDist){
            currPoint = currPoint.Equals(pointA) ? pointB : pointA;
            numDashes++;
            ps.transform.localScale = new Vector3(ps.transform.localScale.x * -1, 1, 1);
            var vel = ps.velocityOverLifetime;
            
            
        }
        
        transform.position = Vector2.MoveTowards(transform.position, currPoint.position, velocity * Time.fixedDeltaTime);
    }

    
}
