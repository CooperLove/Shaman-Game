using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript_Jaguar : AnimationsScript_
{
    [SerializeField] private TigerAttacks player;
    [SerializeField] private bool canDash;
    [SerializeField] private float dashTimer;
    [SerializeField] private float chargeDur;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical   = Input.GetAxis("Vertical");

        animator.SetFloat("Pos X", horizontal);
        animator.SetFloat("Pos Y", vertical);

        HandleMovement(horizontal);
        HandleAttack();
        HandleJump();
        HandleCrouch();
        HandlePush ();
        HandleClimbing(vertical, horizontal);
        HandleHanging(vertical, horizontal);
        HandleBattlecry();
    }

    protected override void HandleAttack (){
        
    }
    
}
