using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceCreeperPhysics : MonoBehaviour
{
    private List<GameObject> shards;
    private List<Vector2> angles;
    public Vector3 shardsInitialPoint;
    private float startOffest;
    [SerializeField] private float yOffset;
    private float shard_X_Offset;
    private float shards_Dist_Offset;
    private float createdShardInterval;
    private float shardDestroyTimer;
    [SerializeField]private LayerMask wallLayer;

    private void Start() {
        shards = new List<GameObject>();
        shards.Add(GameObject.Find("Ice Shard 00_"));
        shards.Add(GameObject.Find("Ice Shard 01_"));
        shards.Add(GameObject.Find("Ice Shard 02_"));
        angles = new List<Vector2>();
        angles.Add(new Vector2(-60,-70));
        angles.Add(new Vector2(-35,-60));
        angles.Add(new Vector2(-30,-60));
        startOffest = 5f;
        yOffset = -6;
        shard_X_Offset = 1.65f;
        shards_Dist_Offset = 120f;
        createdShardInterval = 0.04f;
        shardDestroyTimer = 1.5f;
        wallLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemy");
       // wallLayer = ~wallLayer;
    }

    public IEnumerator JadeShards (){
        
        bool fr = Player.Instance.IsFacingRight;
        float xpos = shardsInitialPoint.x + (fr ? startOffest : -startOffest);
        Vector3 v = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        RaycastHit2D ray = Physics2D.Raycast(v, fr ? Vector2.right : Vector2.left, shards_Dist_Offset, wallLayer);
        float offset = ray.collider == null ? shards_Dist_Offset : ray.distance;
        Debug.Log($"Colidiu com {ray.collider}");
        float rnd;
        float x = transform.position.x;
        Vector2 finalPos = new Vector2 (xpos + (fr ? offset : -offset), 0);
        Vector2 currPos = new Vector2 (xpos, 0);
        Debug.Log($"Indo de {currPos} para {finalPos}");
        //while (xpos + (fr ? shard_X_Offset : -shard_X_Offset) < x + (fr ? offset : -offset)){
        while (Vector2.Distance(currPos, finalPos) > 2 && (fr ? currPos.x < finalPos.x : currPos.x > finalPos.x)){
            Debug.Log($"Facing Right: {fr} => {Vector2.Distance(currPos, finalPos)} {currPos.x} - {finalPos.x} => {currPos.x < finalPos.x} {currPos.x > finalPos.x}");
            for (int i = 0; i < shards.Count; i++)
            {
                rnd = Random.Range((fr ? angles[i].x : angles[i].x * -1), (fr ? angles[i].y : angles[i].y * -1));
                GameObject g = Instantiate(shards[i], new Vector3(xpos, v.y, 0), Quaternion.Euler(0,0, rnd));
                Destroy(g, shardDestroyTimer);
                xpos += fr ? shard_X_Offset : -shard_X_Offset;
                currPos.x = xpos;
                //shards[i].GetComponent<SpriteRenderer>().flipX = fr ? false : true;
            }
            yield return new WaitForSeconds(createdShardInterval);
        }
        if (ray.collider != null && ray.collider.tag == "Enemy"){
            Enemy enemy = ray.collider.GetComponent<Enemy>();
            enemy.TakeDamage(10);
            StartCoroutine(Debuff.Stun(enemy, 2f));
        }
    }
}
