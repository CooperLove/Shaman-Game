using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TigerAttacks : AnimalAttacks
{
    Animator animator;
    private Player player;

    [Header("First Attack variables")]
    
    [SerializeField] CinemachineVirtualCamera cam;

    [Header("Medium Attack variables")]
    //[SerializeField] private bool canUseMediumAttack;
    [SerializeField] public GameObject[] shards;
    [SerializeField] public Vector2[] angles;
    [SerializeField] public Vector3 shardsInitialPoint;
    [SerializeField] public float startOffest = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] public float shard_X_Offset = 0;
    [SerializeField] public float shards_Dist_Offset = 0;
    [SerializeField] public float poseInterval = 0;
    [SerializeField] public float createdShardInterval = 0;
    [SerializeField] public float shardDestroyTimer = 0;
    [SerializeField] private LayerMask wallLayer = 0;

    [Header("Third Attack variables")]
    
    
    [Header("Fourth Attack variables")]
    //[SerializeField] private GameObject auraPrefab = null;
    [SerializeField] private Transform auraPoint = null;
    [SerializeField] private float auraDebuffArea = 0;

    private void Start() {
        animator = GetComponent<Animator>();
        canUseBasicAttack     = true;
        canUseFirstSpell    = true;
        canUseSecondSpell = true;
        //canUseMediumAttack = true;
        canUseThirdSpell   = true;
        player = Player.Instance;
    }
    
    private void Update() {
        
    }

    private void OnDrawGizmosSelected() {
        Vector3 v = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        Gizmos.DrawRay(v, Vector3.right * shards_Dist_Offset);
        if (auraPoint != null)
            Gizmos.DrawWireSphere(auraPoint.position, auraDebuffArea);
    }

}
