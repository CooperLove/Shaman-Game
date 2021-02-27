using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBulletPhysics : MonoBehaviour
{
    public Skill iceBullet;
    public bool shouldSpawn;
    [SerializeField] private GameObject iceShard;
    [SerializeField] private Vector3 spawnPoint;
    public float xOffest;
    public float timer;
    [SerializeField, Range(0,2)] private float spawnInterval;
    [SerializeField, Range(-10,10)] private float lowerBound, upperBound;
    // Start is called before the first frame update
    void Start()
    {
        shouldSpawn = false;
        timer = 0f;
        spawnInterval = 0.25f;
        lowerBound = -5;
        upperBound = 5;
        xOffest = 10;
        spawnPoint = new Vector3 (transform.position.x + xOffest, transform.position.y, transform.position.z);
        iceShard = GameObject.Find("Crocodile Ice Shard");
        //iceShard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldSpawn)
            return;

        //Caso não possua mana, interrompe a habilidade.
        if (Player.Instance.playerInfo.Mana < ((Active) iceBullet).MpCost/(float)System.Math.Round((1/spawnInterval), 2)){
            shouldSpawn = false;
            return;
        }

        if (timer < spawnInterval){
            timer += Time.deltaTime;
        }else {
            //Reduz a mana do player
            Player.Instance.playerInfo.Mana -= ((Active) iceBullet).MpCost/(float)System.Math.Round((1/spawnInterval), 2);
            //Zera o timer
            timer = 0;
            //Atualiza o ponto em que o projetil será lançado
            spawnPoint = new Vector3 (transform.position.x + xOffest, transform.position.y, transform.position.z);
            
            //Instancia um projetil 
            GameObject g = InstantiateBullet();
            //Setup para fazer o projetil se mover
            SetupBullet(g);
            // Setup de dependency injection
            IceBulletProjectile bullet = g.GetComponent<IceBulletProjectile>();
            bullet.iceBullet = (IceBullet) iceBullet;

            //Passiva
            ApllyPassive();
        }
    }

    private void ApllyPassive (){
        Player player = Player.Instance;
        PassiveCrocodileSO passive = (PassiveCrocodileSO) player.GetComponent<CrocodileAttacks>().Passive;
        passive.Use(((IceBullet)iceBullet).energyPerProjectile);
    }

    private GameObject InstantiateBullet (){
        //Calcula um offset no eixo Y para que os projeteis apareçam em alturas diferentes
        float yOffset = Random.Range(lowerBound, upperBound);
        Vector3 v = new Vector3 (spawnPoint.x, spawnPoint.y + yOffset, spawnPoint.z);
        //Instancia o projetil
        GameObject g = Instantiate (iceShard, v, iceShard.transform.rotation);

        //Adicionando componentes necessários para causar dano e movimentar o projetil
        g.AddComponent(typeof(Projectile_Shoot_Test));
        g.AddComponent(typeof(IceBulletProjectile));

        return g;
    }

    private void SetupBullet (GameObject g){
        Projectile_Shoot_Test projectile = g.GetComponent<Projectile_Shoot_Test>();
        projectile.shouldDestroy = true;
        projectile.destroyTimer = 2f;
        projectile.velocity = 205;
    }
}
