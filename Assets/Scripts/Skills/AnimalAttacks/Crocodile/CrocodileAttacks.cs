using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileAttacks : AnimalAttacks
{
    public GameObject passiveBar;
    [Header("Habilidades Especificas")]
    // [SerializeField] private Active shield = null;
    // [SerializeField] private Active iceSpike = null;
    // [SerializeField] private Active upperIcePillar = null;
    // [SerializeField] private Active bottomIcePillar = null;
    //[Header("Basic attack variables")]

    [Header("First Attack variables")]

    [Header("Second Attack variables")]
    //[SerializeField] private bool canUseMediumAttack = false;
    

    //[Header("Third Attack variables")]
    //[Header("Fourth Attack variables")]
    //[Header("Fifth Attack variables")]
    
    bool isRunning = false;
    PlayerInfo player;
    // Start is called before the first frame update
    void Start()
    {
        canUseBasicAttack = true;
        canUseFirstSpell  = true;
        canUseSecondSpell = true;
        canUseThirdSpell  = true;
        canUseFourthSpell = true;
        canUseFifthSpell  = true;
        player = Player.Instance.playerInfo;
        passiveBar?.SetActive(false);
        //passive.Use();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameStatus.isOnChallenge);
        if (Input.GetKeyDown(KeyCode.J) && canUseBasicAttack){
            basicAttack.OnUse();
        }
        return;
        // if (Input.GetKeyDown(KeyCode.K) && canUseFirstSpell && (GameStatus.isOnChallenge || player.Level >= 7)){
        //     if (Input.GetKey(KeyCode.W)){
        //         shield.Use();
        //         return;
        //     }
        //     first.Use(); // => Heal
        // }
        // if (Input.GetKeyDown(KeyCode.L) && canUseSecondSpell && (GameStatus.isOnChallenge || player.Level >= 12)){
        //     //shardsInitialPoint = transform.position;
        //     second.Use();
        // }
        // if (Input.GetKeyDown(KeyCode.U) && canUseThirdSpell && (GameStatus.isOnChallenge || player.Level >= 15)){
        //     if (Input.GetKey(KeyCode.W)){
        //         iceSpike.Use();
        //         return;
        //     }
        //     third.Use(); // => Ice Bullet
        // }
        // if (Input.GetKeyUp(KeyCode.U)){
        //     Player pi = Player.Instance;
        //     pi.TryGetComponent<IceBulletPhysics>(out IceBulletPhysics iceBullet);
        //     if (iceBullet != null){
        //         iceBullet.shouldSpawn = false;
        //         iceBullet.timer = 0f;
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.I) && canUseFourthSpell && (GameStatus.isOnChallenge || player.Level >= 18)){
        //     if (Input.GetKey(KeyCode.S)){
        //         upperIcePillar.Use();
        //         return;
        //     }
        //     if (Input.GetKey(KeyCode.W)){
        //         bottomIcePillar.Use();
        //         return;
        //     }
        //     fourth.Use(); // => Middle Pillar
        // }
        // if (Input.GetKeyDown(KeyCode.O) && canUseFifthSpell && (GameStatus.isOnChallenge || player.Level >= 22))
        //     fifth.Use();
    }


    


    // private IEnumerator Shake (float interval, GameObject g){
    //     yield return new WaitForSeconds(interval);
    //     Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, aoeArea, enemyLayer);

    //     if (enemies.Length > 0){
    //         foreach (Collider2D enemy in enemies)
    //             enemy.GetComponent<Enemy>().TakeDamage(10);
    //         StartCoroutine(SimpleCameraShake.instance.ShakeCamera(ShakeAmplitude, ShakeFrequency, secondShakeDuration));
    //         StartCoroutine(loseLifeOvertime(Player.Instance.playerInfo.HP, enemies.Length * healPerEnemy, healPerSecond));
    //         Debug.Log($"Start: {Time.time}");
    //     }
    //     else{
    //         Destroy(g);
    //     }
    // }


    private void OnDrawGizmosSelected() {

    }

    // private void healPlayer (){
    //     if (hp <= 0){
    //         Debug.Log($"End: {Time.time}");
    //         heal = false;
    //         return;
    //     }

    //     hp -= 1/(hp * healPerSecond * Time.deltaTime);
    //     Player.Instance.playerInfo.HP += 1/(hp * healPerSecond * Time.deltaTime);
    // }

    

    IEnumerator loseLifeOvertime(float currentLife, float lifeToLose, float duration)
    {
        //Make sure there is only one instance of this function running
        if (isRunning)
        {
            yield break; ///exit if this is still running
        }
        isRunning = true;

        float counter = 0;

        //Get the current life of the player
        float startLife = currentLife;

        //Calculate how much to lose
        float endLife = currentLife + lifeToLose;

        //Stores the new player life
        float newPlayerLife = currentLife;
        float lifeGain = currentLife;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            newPlayerLife = Mathf.Lerp(startLife, endLife, counter / duration);
            lifeGain = newPlayerLife - lifeGain;
            Player.Instance.playerInfo.Health += lifeGain;
           // Debug.Log("Current Life: " + newPlayerLife+" healed: "+lifeGain);
            lifeGain = newPlayerLife;
            yield return null;
        }

        //The latest life is stored in newPlayerLife variable
        //yourLife = newPlayerLife; //????

        isRunning = false;
    }

    
}
