using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : AnimationsScript_
{
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
        if (Input.GetKeyDown(KeyCode.J)){
            animator.SetBool("Attack", true);
        }
        if (Input.GetKey(KeyCode.J) && animator.GetBool("Attack")){
            animator.SetBool("Aiming", true);
        }
        if (Input.GetKeyUp(KeyCode.J) && animator.GetBool("Aiming")){
            animator.SetBool("Aiming", false);
        }

        if (Input.GetKeyDown(KeyCode.I)){
            animator.SetBool("Attack", true);
            animator.SetBool("Charging", true);
        }
    }

}
