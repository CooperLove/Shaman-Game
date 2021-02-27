using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowChain : MonoBehaviour
{
    public Chain skill;
    public KeyCode key;
    public GameObject chain = null;
    public GameObject aim = null;
    public GameObject enemy = null;
    public float dist = 5f;
    public float interval = 0.01f;
    public float tickInterval = 0.4f;
    public float xValue = 1f;
    public float pullTargetMinDist = 5f;
    public float pullInterval = 0.01f;
    public float pullTimer = 0.5f;
    public float pullValue = 1f;
    public float pullOffset = 1f;
    public LayerMask enemyLayer;
    public Transform target = null;
    public bool thrown = false;
    public bool canPull = false;
    public bool canDoDamage = false;
    public bool pulling = false;
    public bool pull = false;
    public bool doingDamage = false;
    public bool reachedTarget = false;
    public bool action = false;
    private GameObject hitFX;
    private GameObject smokeTrail;
    [SerializeField] private Material chainMaterial;
    private void Start() {
        enemyLayer = 1 << 9;
        pullTimer = 0.5f;
        pullValue = 105;
        pullOffset = 1.2f;
        pullInterval = 0.01f;
        pullTargetMinDist = 15f;
        xValue = 22.5f;
        tickInterval = 0.6f;

        chainMaterial = Resources.Load("Materials/2D Chain Material") as Material;
        chainMaterial.SetVector("_scrollSpeed", new Vector4(0, -0.15f, 0, 0));
    }
    private void Update() {
        // if (Input.GetKeyDown(key) && !canPull && !canDoDamage)
        //     Throw();
        // else if (Input.GetKeyDown(key) && canPull && !canDoDamage){
        //     Debug.Log($"Pulling");
        //     StartCoroutine(PullTarget());
        //     Destroy(aim);
        // }
        // else if (Input.GetKeyDown(key) && !canPull && canDoDamage){
        //     Debug.Log($"Doing damage");
        //     doingDamage = true;
        //     StartCoroutine(Damage());
        //     Destroy(aim);
        // }
        if (thrown && !reachedTarget && Input.GetKeyDown(key)){
            Debug.Log("Puxar imediatamente");
            pull = true;
        }
        if (doingDamage && Input.GetKeyUp(key)){
            doingDamage = false;
            skill.enterOnCooldown = true;
            skill.useSkill.Stop();
            RemoveChains ();
        }
    }
    public bool Throw (){
        var player = Player.Instance;
        var pi = Player.Instance.playerInfo;

        if ( NotEnoughMPorSP()){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        }

        thrown = true;
        GameStatus.IgnoreCommands(true, this);
        ApllyCost();

        var isFacingRight = Player.Instance.IsFacingRight;
        CreateChain(isFacingRight);
        
        var ray = Physics2D.Raycast(transform.position, 
                                            isFacingRight ? 
                                            Vector2.right : 
                                            Vector2.left, 80, enemyLayer);

        target = ray.transform;
        
        StartCoroutine(ReachTarget(this.chain, ray.transform != null ? Vector2.Distance(transform.position, ray.transform.position) : 80));
        return true;
    }

    private IEnumerator ReachTarget (GameObject g ,float size){
        var isFacingRight = Player.Instance.IsFacingRight;
        var sprite = Initialize(g);
        
        Debug.Log($"Distance {size}");

        while (sprite.size.x < (size/10f) ){
            yield return new WaitForSeconds(interval);
            if (!sprite)
                break;
            sprite.size = new Vector2(sprite.size.x + (xValue * Time.fixedDeltaTime), sprite.size.y);
            Debug.Log($"Size {sprite.size} Xvalue {xValue}");
        }
        Debug.Log($"Reached {target.name}");

        sprite.size = new Vector2(size/10f, sprite.size.y);
        reachedTarget = true;


        if (target){
            if (pull){
                StartCoroutine(PullTarget());
                yield break;
            }

            StartCoroutine(TargetAim());

            var chainHit = Resources.Load("Prefabs/Combat/Chain Hit VFX") as GameObject;
            if (chainHit != null){
                hitFX = Instantiate(chainHit, target.position, chainHit.transform.rotation);
            }

            yield return new WaitUntil(() => action);
            
            
            Destroy(this.aim);
        }
        if (!doingDamage && !pulling){
            OnEndTimer();
        }
    }

    public IEnumerator PullTarget (){
        pulling = true;

        RemoveChains();
        yield return new WaitForSeconds(pull ? 0 : 0.45f);


        var isFacingRight = Player.Instance.IsFacingRight;
        var sprite = this.chain.transform.GetChild(1).GetComponent<SpriteRenderer>();

        SetUpTrail ();

        float distance;
        while ((distance = Vector2.Distance(transform.position, target.position)) > pullTargetMinDist){
            var targetPosition = target.position;
            targetPosition += new Vector3((isFacingRight ? -pullValue : pullValue) * Time.fixedDeltaTime, 0, 0);
            target.position = targetPosition;

            yield return new WaitForSeconds(pullInterval);

            distance = Vector2.Distance(transform.position, targetPosition);
            sprite.size = new Vector2((distance/10f), sprite.size.y);

            if (TargetPassedMe(isFacingRight))
                break;

        }
        pulling = false;
        pull = false;
        EndAction();
    }

    public IEnumerator Damage (){
        var pi = Player.Instance.playerInfo;
        chainMaterial.SetVector("_scrollSpeed", new Vector4(0, -2, 0, 0));

        while(doingDamage){
            if (NotEnoughMPorSP()){
                Stop(); break;
            }
            ApplyTick();
            yield return new WaitForSeconds(tickInterval);
        }
        EndAction();
    }
    

    private void OnEndTimer (){
        skill.enterOnCooldown = true;
        skill.useSkill.Stop();
        EndAction();
    }

    private SpriteRenderer Initialize (GameObject g){
        var sprite = g.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sprite.size = new Vector2(0, sprite.size.y);
        reachedTarget = false;

        return sprite;
    }

    private IEnumerator TargetAim (){
        var aim = Resources.Load("Prefabs/Combat/TargetAim") as GameObject;
        this.aim = Instantiate(aim, target.position+ new Vector3(0, 5, 0), aim.transform.rotation);

        action = false;
        canPull = true;

        yield return new WaitForSeconds(pullTimer);
        canPull = false;
        canDoDamage = true;

        yield return new WaitForSeconds(pullTimer);
        canDoDamage = false;

        action = true;
    }

    private bool TargetPassedMe (bool isFacingRight)
    {
        var targetPosition = target.position;
        var transformPosition = transform.position;
        return isFacingRight && targetPosition.x <= transformPosition.x
               ||  !isFacingRight && targetPosition.x >= transformPosition.x;
    }
    
    private void Stop(){
        GameAnnouncement.NotEnoughManaAnnouncement();
        doingDamage = false;
        skill.enterOnCooldown = true;
        skill.useSkill.Stop();
        RemoveChains();
    }

    private bool NotEnoughMPorSP (){
        PlayerInfo pi = Player.Instance.playerInfo;
        return pi.Stamina < skill.SpCost || pi.Mana < skill.MpCost;
    }
    private void ApplyTick (){
        if (target == null)
            return;

        PlayerInfo pi = Player.Instance.playerInfo;

        int damage = pi.CalculatePhysicalDamage();
        target.GetComponent<Enemy>().TakeDamage(damage);
        Player.Instance.Heal((damage/100)*10);
        pi.Mana -= skill.mpCostPerTick;
        pi.Stamina -= skill.spCostPerTick;
    }

    public void ApllyCost (){
        PlayerInfo pi = Player.Instance.playerInfo;

        pi.Stamina -= skill.SpCost;
        pi.Mana -= skill.MpCost;
    }

    private void CreateChain (bool isFacingRight) {
        var chain = Resources.Load("Prefabs/Combat/Chain 2") as GameObject;
        if (!chain)
            return;

        this.chain = Instantiate(chain, Player.Instance.transform.position, chain.transform.rotation);
        var localScale = chain.transform.localScale;
        this.chain.transform.localScale = isFacingRight ? localScale : 
                                    new Vector3(-localScale.x, localScale.y, 1);
    }

    private void RemoveChains ()
    {
        if (!hitFX) 
            return;
        
        hitFX?.GetComponent<Animator>()?.SetBool("Out", true);
        Destroy(hitFX, 0.75f);
    }

    private void SetUpTrail (){
        RaycastHit2D raycast = Physics2D.Raycast(target.position, Vector2.down, Mathf.Infinity, 1 << 10);
        if (raycast){
            Debug.Log($"Raycast hit {raycast.transform.name} Point {raycast.point}");
            GameObject smoke = Resources.Load("Prefabs/General FX/Smoke") as GameObject;
            smokeTrail = Instantiate(smoke, target.position, smoke.transform.rotation);
            smokeTrail.transform.position = new Vector3(raycast.point.x, raycast.point.y, target.position.z);
            smokeTrail?.GetComponent<TrailFollow>()?.Follow(target);
        }
    }

    private void EndAction (){
        thrown = false;
        GameStatus.IgnoreCommands(false, this);
        RemoveChains();
        Destroy(chain);
        if (smokeTrail != null)
            Destroy(smokeTrail);
    }
}
