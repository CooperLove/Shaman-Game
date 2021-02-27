using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsScript : MonoBehaviour
{
    public Animator animator;
    public bool attacking = false;
    public float weight = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(0, weight);
    }

    // Update is called once per frame
    public void OnAttackEnter (int attack){
        OnRunExit ();
        animator.SetInteger("Attack", attack);
        attacking = true;
    }
    public void OnAttackExit (){
        OnRunExit ();
        animator.SetInteger("Attack", -1);
        attacking = false;
    }
    public void OnRunEnter (float h){
        animator.SetFloat ("Horizontal", h);
    }
    public void OnRunExit (){
        animator.SetFloat ("Horizontal", 0);
    }
    public void OnJumpEnter (){
        animator.SetBool("OnGround", false);
        animator.SetTrigger("Jump");
    }
    public void OnJumpExit (){
        animator.SetBool("OnGround", true);
        animator.ResetTrigger("Jump");
    }

    public void OnEnterRoll (){
        animator.SetTrigger("Roll");
    }
    public void OnExitRoll (){
        animator.ResetTrigger("Roll");
    }
}
