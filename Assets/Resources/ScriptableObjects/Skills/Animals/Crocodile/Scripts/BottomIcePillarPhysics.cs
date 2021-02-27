using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomIcePillarPhysics : MonoBehaviour
{
    public Enemy enemy;
    private GameObject icePillar;
    private List<GameObject> shards;
    [SerializeField] private List<Vector2> angles;
    [SerializeField] private Vector2 yValues;
    public Vector3 shardsInitialPoint;
    [SerializeField] private float startOffest;
    [SerializeField] private float yOffset;
    [SerializeField] private float shard_X_Offset;
    [SerializeField] private float shards_Dist_Offset;
    [SerializeField] private float createdShardInterval;
    [SerializeField] private float shardDestroyTimer;
    [SerializeField] private float yForce;
    [SerializeField] private ForceMode2D forceMode;
    [SerializeField, Tooltip("Variáveis para tremer a camera ... X => Amplitude, Y => Frequência e Z => Duração")] 
    private Vector3 minorShake, largeShake;
    [SerializeField]private LayerMask wallLayer;

    private void Start() {
        icePillar = GameObject.Find("Crocodile Bottom Ice Pillar - Mask");
        shards = new List<GameObject>();
        shards.Add(GameObject.Find("Crocodile Bottom Ice Pillar - Minor"));
        angles = new List<Vector2>();
        angles.Add(new Vector2(-25,15));
        yValues = new Vector2(0.15f, -2.5f);
        startOffest = 5f;
        yOffset = -6;
        shard_X_Offset = 3.25f;
        shards_Dist_Offset = 120f;
        createdShardInterval = 0.0175f;
        shardDestroyTimer = 2.5f;
        wallLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemy");
        yForce = 850;
        forceMode = ForceMode2D.Impulse;
        minorShake = new Vector3(2.5f, 2f, 0.01f);
        largeShake = new Vector3(7.5f, 5f, 0.3f);
       // wallLayer = ~wallLayer;
    }

    public IEnumerator IceShards (){
        enemy = null;
        //Direção que o player está olhando quando iniciou a skill
        bool fr = Player.Instance.IsFacingRight;
        //offset para o inicio dos blocos
        float xpos = shardsInitialPoint.x + (fr ? startOffest : -startOffest);
        Vector3 v = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        //Ray para detectar inimigos ou paredes. A skill vai parar quando atingir um dos dois ou o range maximo
        RaycastHit2D ray = Physics2D.Raycast(v, fr ? Vector2.right : Vector2.left, shards_Dist_Offset, wallLayer);
        //Checa se existe algum inimigo no alcance da skill
        float offset = ray.collider == null ? shards_Dist_Offset : ray.distance;
        Debug.Log($"Colidiu com {ray.collider}");
        //rnd => pra pegar um angulo aleatorio //yoff => altura variavel dos blocos de gelo
        float rnd, yOff;
        //Posição final/finaliza a skill e posição que inicia skill
        Vector2 finalPos = new Vector2 (xpos + (fr ? offset : -offset), 0);
        Vector2 currPos = new Vector2 (xpos, 0);
        //Fica criando blocos até algum limite ser atingido
        while (Vector2.Distance(currPos, finalPos) > 2 && (fr ? currPos.x < finalPos.x : currPos.x > finalPos.x)){
            for (int i = 0; i < shards.Count; i++)
            {
                yOff = Random.Range(yValues.x, yValues.y);
                rnd = Random.Range((fr ? angles[i].x : angles[i].x * -1), (fr ? angles[i].y : angles[i].y * -1));
                GameObject g = Instantiate(shards[i], new Vector3(xpos, v.y, 0), Quaternion.Euler(0,0, rnd));
                StartCoroutine(SimpleCameraShake.instance.ShakeCamera(minorShake.x, minorShake.y, minorShake.z));
                Destroy(g, shardDestroyTimer);
                xpos += fr ? shard_X_Offset : -shard_X_Offset;
                currPos.x = xpos;
            }
            yield return new WaitForSeconds(createdShardInterval);
        }
        //Aplica dano, levanta o inimigo e instancia o ultimo bloco de gelo
        if (ray.collider != null && ray.collider.tag == "Enemy"){
            enemy = ray.collider.GetComponent<Enemy>();
            enemy.TakeDamage(10);
            StartCoroutine(Debuff.Stun(enemy, 2f));
            v = new Vector3(enemy.transform.position.x, v.y, 0);
            GameObject g = Instantiate (icePillar, v, icePillar.transform.rotation);
            Destroy(g, shardDestroyTimer);
            enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * yForce, forceMode);
            //StopAllCoroutines();
                StartCoroutine(SimpleCameraShake.instance.ShakeCamera(largeShake.x, largeShake.y, largeShake.z));  
        }
        yield return new WaitForSeconds(shardDestroyTimer);
        enemy = null;
    }
}
