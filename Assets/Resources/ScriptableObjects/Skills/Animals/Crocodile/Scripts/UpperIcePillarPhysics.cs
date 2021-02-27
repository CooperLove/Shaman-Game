using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperIcePillarPhysics : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] private GameObject pillar = null;
    [SerializeField] private float range = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float destroyTimer = 0;
    [SerializeField] private float force = 0;
    [SerializeField] private LayerMask wallLayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        pillar = GameObject.Find("Crocodile Upper Ice Pillar");
        destroyTimer = 2.0f;
        range = 120;
        yOffset = 20f;
        wallLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemy");
    }

    public void IceCollumn (){
        enemy = null;
        bool fr = Player.Instance.IsFacingRight;
        Vector3 v = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        RaycastHit2D ray = Physics2D.Raycast(v, fr ? Vector2.right : Vector2.left, range, wallLayer);

        if (TryGetComponent<BottomIcePillarPhysics>(out BottomIcePillarPhysics pilar)){
            enemy = pilar.enemy;
        }
        if (enemy != null){
            StartCoroutine(Debuff.Stun(enemy, 2f));
            v = new Vector3(enemy.transform.position.x , enemy.transform.position.y + yOffset, 0);
            GameObject g = Instantiate (pillar, v, pillar.transform.rotation);
            enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force, ForceMode2D.Impulse);
            Destroy(g, destroyTimer);
            return;
        }
        
        if (ray.collider != null && ray.collider.tag == "Enemy"){
            enemy = ray.collider.GetComponent<Enemy>();
            StartCoroutine(Debuff.Stun(enemy, 2f));
            v = new Vector3(ray.transform.position.x , ray.transform.position.y + yOffset, 0);
            GameObject g = Instantiate (pillar, v, pillar.transform.rotation);
            Destroy(g, destroyTimer);
        }
        enemy = null;
    }
}
