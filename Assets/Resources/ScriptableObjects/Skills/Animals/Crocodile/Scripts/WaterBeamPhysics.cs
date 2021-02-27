using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaterBeamPhysics : MonoBehaviour
{
    [SerializeField] private GameObject waterBeam = null;
    public Vector3 point = new Vector3();
    public Vector2 size = new Vector2();
    public float offset = 0;
    public float yOffset = 0;
    public float dir = 0;
    private Player player = null;
    private LayerMask enemyLayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        point = player.transform.position + (player.IsFacingRight ? Vector3.right : Vector3.left) * offset;
        enemyLayer = 1 << 9;
    }

    // Update is called once per frame
    void Update()
    {
        return;
        // if (Input.GetKeyDown(KeyCode.W)){
        //     Use();
        // }
        // Debug.DrawRay(waterBeam.transform.position, waterBeam.transform.forward * dir, Color.blue);

    }

    public void Use (){
        point = player.transform.position + (player.IsFacingRight ? Vector3.right : Vector3.left) * offset;
        Collider2D[] enemies = Physics2D.OverlapBoxAll(point, size, 0, enemyLayer);
        if (enemies.Length > 0) {
            float d = Mathf.Infinity;
            Collider2D col = null;
            foreach (Collider2D c in enemies)
            {
                float dist = Vector2.Distance(player.transform.position, c.transform.position);
                if (dist < d){
                    d = dist;
                    col = c;
                }
            }
            GameObject g = Instantiate (waterBeam, player.transform.position + Vector3.up * yOffset, waterBeam.transform.rotation);
            
            Destroy(g, 2f);
            

            Vector2 targetDir = col.transform.position - g.transform.position;
            //targetDir = targetDir.normalized;
            Vector2 forward = Vector2.right;
            float dd = Vector2.Distance(g.transform.position, col.transform.position);
            float unit = dir;
            float res = dd / unit;
            Debug.Log($"Inimigo mais próximo: {col.name} {dd} => {res} {1*res} - {2.5f*res}");
            Transform child = g.transform.GetChild(1).GetChild(0);
            child.localScale = new Vector3(1 * res, 1, 1);
            child.localPosition = new Vector3(2.5f * res, child.localPosition.y, child.localPosition.z);
            g.transform.rotation = Quaternion.Euler(0,0,-Vector2.Angle(forward, targetDir));

        }
    }

    private void OnDrawGizmosSelected() {
        //point = player.transform.position + (player.IsFacingRight ? Vector3.right : Vector3.left) * offset;
        Gizmos.DrawWireCube(point, size);
        Gizmos.DrawRay(waterBeam.transform.position, waterBeam.transform.right * dir);
    }
}
