using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkingBehaviour : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 2.5f;
    float distance = 0.0f;
    public Player player;
    private Rigidbody2D rb;
    Vector2 target = Vector2.zero;
    Vector2 newPos = Vector2.zero;
    private Boss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) 
            player = Player.Instance;
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        boss = animator.gameObject.GetComponent<Boss>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.growing)
            return;
        distance = Vector2.Distance(player.transform.position, rb.position);
        if (distance <= attackRange && !boss.IsAttacking){
            animator.SetInteger("Attack", 0);
            return;
        }
        animator.SetInteger("Attack", -1);
        target = new Vector2(player.transform.position.x, rb.position.y);
        newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        //Debug.Log($"Distance: {distance}");
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
