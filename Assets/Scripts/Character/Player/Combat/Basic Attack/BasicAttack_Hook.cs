using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack_Hook : MonoBehaviour
{
    private GameObject chain;
   // private bool pulling = false;
    public float pullTargetMinDist = 5f;
    public float pullInterval = 0.01f;
    public float pullTimer = 0.5f;
    public float pullValue = 1f;
    public Vector3 pullOffset = new Vector3();
   // private bool reachedTarget = false;
    public float dist = 5f;
    public float interval = 0.01f;
    public float xValue = 1f;
    private Transform target;
    public Vector3 pullPoint;
    public Vector3 pullPointSize;
    private LayerMask enemyLayer;
    public Vector3 point, size, offset;
    public float radius;


    private void Start() {
        enemyLayer = 1 << 9;
        var IsFacingRight = Player.Instance.IsFacingRight;
        point.x = IsFacingRight ? point.x : -point.x;
        offset.x = IsFacingRight ? offset.x : -offset.x;
        pullOffset.x = IsFacingRight ? pullOffset.x : -pullOffset.x;
        //Throw();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.X))
            Throw();
    }

    public bool Throw (){
        GameStatus.IgnoreCommands(false, this);

        var isFacingRight = Player.Instance.IsFacingRight;
        CreateChain(isFacingRight);
        
        var col = Physics2D.OverlapBox(transform.position + point, size, 0, enemyLayer);

        target = col?.transform;
        if (target) {
            var rigidbody = target.GetComponent<Enemy>().Rb;
            rigidbody.velocity = Vector2.zero;
        }

        var hypotenusa = PrePullSetup ();
        
        StartCoroutine(ReachTarget(this.chain, hypotenusa));
        return true;
    }

    private IEnumerator ReachTarget (GameObject g ,float size){
        var sprite = Initialize(g);
        
        while (sprite.size.x < (size/10f)){
            yield return new WaitForSeconds(interval);
            if (sprite)
                break;
            var spriteSize = sprite.size;
            spriteSize = new Vector2(spriteSize.x + (xValue * Time.fixedDeltaTime), spriteSize.y);
            sprite.size = spriteSize;
        }

        sprite.size = new Vector2((size/10f), sprite.size.y);
        //reachedTarget = true;
        if (target)
            StartCoroutine(PullTarget());
        else
            EndAction();
    }
    private IEnumerator PullTarget (){
        //pulling = true;

        var isFacingRight = Player.Instance.IsFacingRight;
        var sprite = this.chain.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

        var  fixedPullValue = isFacingRight ? -pullValue : pullValue;
        while (Vector2.Distance(pullPoint, target.position) > pullTargetMinDist){
            var targetPosition = target.position;
            targetPosition = Vector2.MoveTowards(targetPosition, pullPoint, pullValue * Time.fixedDeltaTime);
            target.position = targetPosition;
            yield return new WaitForSeconds(pullInterval);

            var distance = Vector2.Distance(transform.position + offset, targetPosition);
            var hypotenusa = Mathf.Sqrt(distance * distance + distance * distance);

            var angle = GetAngleBetweenTargetAndTransform ();
            chain.transform.rotation = Quaternion.Euler(0,0, angle);
            Debug.DrawLine(pullPoint, targetPosition, Color.cyan, 0.2f);

            sprite.size = new Vector2((hypotenusa/10f), sprite.size.y);

            if (TargetPassedMe(isFacingRight))
                break;

        }
        //pulling = false;
        EndAction();
    }

    private bool TargetPassedMe (bool isFacingRight)
    {
        var targetPosition = target.position;
        var transformPosition = transform.position;
        return isFacingRight && targetPosition.x <= transformPosition.x
               ||  !isFacingRight && targetPosition.x >= transformPosition.x;
    }

    private void EndAction (){
        GameStatus.IgnoreCommands(false, this);
        target = null;
        Destroy(chain);
        Destroy(gameObject);
    }

    private SpriteRenderer Initialize (GameObject g){
        SpriteRenderer sprite = g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        sprite.size = new Vector2(0, sprite.size.y);
        //reachedTarget = false;

        return sprite;
    }

    private void CreateChain (bool isFacingRight) {
        var chainPrefab = Resources.Load("Prefabs/Combat/Chain 1") as GameObject;
        // this.chain = Instantiate(chain, transform.position + new Vector3(IsFacingRight ? 40 : -40, 0, 0), chain.transform.rotation);
        if (!chainPrefab)
            return;
        
        this.chain = Instantiate(chainPrefab, transform.position + offset, chainPrefab.transform.rotation);
        var chainLocalScale = chainPrefab.transform.localScale;
        this.chain.transform.localScale = isFacingRight ? chainLocalScale : 
                                    new Vector3(-chainLocalScale.x, chainLocalScale.y, 1);
    }

    private float GetAngleBetweenTargetAndTransform (){
        if (!target)
            return Player.Instance.IsFacingRight ? 45 : -45;

        var dir = target.position - (transform.position + offset);
        dir = target.transform.InverseTransformDirection(dir);
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private float CalculateHypotenusa (){
        var distance = Vector2.Distance(pullPoint, target.position);
        return Mathf.Sqrt(distance * distance + distance * distance);
    }

    private float PrePullSetup (){
        FaceTarget ();
        
        if (!target)
            return 80;
        
        pullPoint = transform.position + pullOffset;

        var hypotenusa = CalculateHypotenusa();

        Debug.DrawLine(pullPoint, target.position, Color.red, 3f);

        return hypotenusa;
    }

    private void FaceTarget (){
        float angle = GetAngleBetweenTargetAndTransform ();
        chain.transform.rotation = Quaternion.Euler(0,0, angle);
    }

    private void OnDrawGizmos() {
        var transformPosition = transform.position;
        Gizmos.DrawWireCube(transformPosition + point, size);
        pullPoint = transformPosition + pullOffset;
        Gizmos.DrawWireCube(pullPoint, pullPointSize);
        Gizmos.DrawWireSphere(transformPosition + offset, radius);
    }
}
