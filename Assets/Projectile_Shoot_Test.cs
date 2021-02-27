using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Shoot_Test : MonoBehaviour
{
    public enum SpaceDirection
    {
        up, down, right, left
    }
    
    public float startDelay = 0.0f;
    [SerializeField] private bool destroyOnHit = false;
    [Tooltip("Has it hit something ?")] public bool hit = false;

    [Header("Destroy with timer")]
    [Tooltip("Should destroy after some time ?")] public bool shouldDestroy = false;
    public float destroyTimer = 0.0f;
    public float velocity = 0.0f;
    
    [Header("Direction")]
    [SerializeField]private bool chooseDirectionBasedOnPlayerDirection = false;
    [SerializeField] private SpaceDirection direction = SpaceDirection.right;
    private Vector3 playerDir = Vector3.down;
    [SerializeField] private Space space = Space.World;
    private bool readyToGo = false;
    // Start is called before the first frame update
    void Start()
    {
        if (shouldDestroy)
            Destroy(gameObject, destroyTimer);

        if (chooseDirectionBasedOnPlayerDirection)
            playerDir = Player.Instance.IsFacingRight ? Vector3.right : Vector3.left;

        if (startDelay == 0)
            readyToGo = true;
        else
            StartCoroutine (StartDelay (startDelay));
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!readyToGo) return;
        
        var dir = playerDir;
        if (!chooseDirectionBasedOnPlayerDirection){
            switch (direction)
            {
                case SpaceDirection.up:
                    dir = Vector3.up; break;
                case SpaceDirection.down:
                    dir = Vector3.down; break;
                case SpaceDirection.right:
                    dir = Vector3.right; break;
                case SpaceDirection.left:
                    dir = Vector3.left; break;
            }
        }
        transform.Translate(dir * (velocity * Time.deltaTime), space);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) 
            return;
        
        var player = Player.Instance;
        var pi = Player.Instance.playerInfo;

        var enemy = other.transform.GetComponent<Enemy>();
        enemy?.TakeDamage(pi.CalculatePhysicalDamage());

        hit = true;
        
        if (destroyOnHit && !shouldDestroy)
            Destroy(gameObject);
    }

    private IEnumerator StartDelay (float delay) {
        yield return new WaitForSeconds (delay);
        readyToGo = true;
    }
}
