using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationsScript_ : MonoBehaviour
{
    protected Animator animator;
    protected void HandleMovement (float horizontal){
        if (horizontal > 0 && Input.GetKey(KeyCode.LeftShift)){
            animator.SetTrigger("Sprint");
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Run");
        }
        if (horizontal > 0 && Input.GetKeyUp(KeyCode.LeftShift)){
            animator.ResetTrigger("Sprint");
        }
        if (horizontal > 0 && Input.GetKey(KeyCode.LeftControl)){
            animator.SetTrigger("Walk");
            animator.ResetTrigger("Sprint");
            animator.ResetTrigger("Run");
        }
        if (horizontal > 0 && Input.GetKeyUp(KeyCode.LeftControl)){
            animator.ResetTrigger("Walk");
        }
        if (horizontal > 0 && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift)){
            animator.SetTrigger("Run");
        }
        if (horizontal == 0){
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Sprint");
        }
    }

    protected void HandleJump (){
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift)){
            animator.SetTrigger("Sprint Jump");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            animator.SetTrigger("Jump");
        }
    }

    protected void HandleCrouch (){
        if (Input.GetKeyDown(KeyCode.S))
            animator.SetBool("Crouch", true);
        if (Input.GetKeyUp(KeyCode.S))
            animator.SetBool("Crouch", false);
    }

    protected void HandlePush (){
        if (Input.GetKey(KeyCode.P))
            animator.SetBool("Pushing", true);
        if (Input.GetKeyUp(KeyCode.P))
            animator.SetBool("Pushing", false);
    }

    protected void HandleClimbing (float vertical, float horizontal){
        if (Input.GetKey(KeyCode.LeftShift) && horizontal == 0 && vertical > 0){
            animator.SetBool("Climbing", true);
            animator.SetTrigger("ClimbingUp");
        }
        if (Input.GetKey(KeyCode.LeftShift) && horizontal == 0 && vertical < 0){
            animator.SetBool("Climbing", true);
            animator.SetTrigger("ClimbingDown");
        }

        if (!Input.GetKey(KeyCode.LeftShift) && horizontal == 0){
            animator.SetBool("Climbing", false);
            animator.ResetTrigger("ClimbingUp");
            animator.ResetTrigger("ClimbingDown");
        }  
    }

    protected void HandleHanging (float vertical, float horizontal){
        if (vertical > 0 && Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("Hanging", true);
        if (vertical < 0 && Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("Hanging", false);
    }

    protected abstract void HandleAttack ();

    public void SetAttack () => animator.SetBool("Attack", true);
    public void ResetCharge () => animator.SetBool("Charging", false);
    
    public void ResetAttack () => animator.SetBool("Attack", false);

    protected void HandleBattlecry (){
        if (Input.GetKeyDown(KeyCode.L)){
            animator.SetTrigger("Battlecry");
        }
    }

    protected IEnumerator ChangeValueOvertime(float currentValue, float valueToLose, float duration)
    {
        float counter = 0;
        animator.SetFloat("ChargeFloat", 0);

        //Get the current life of the player
        float startValue = currentValue;

        //Calculate how much to lose
        float endValue = currentValue + valueToLose;

        //Stores the new player life
        float newValue = currentValue;
        float valueGain = currentValue;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            newValue = Mathf.Lerp(startValue, endValue, counter / duration);
            valueGain = newValue - valueGain;
            animator.SetFloat("ChargeFloat", animator.GetFloat("ChargeFloat")+valueGain);
            //Debug.Log($"{animator.GetFloat("ChargeFloat")} {valueGain}");
           // Debug.Log("Current Life: " + newPlayerLife+" healed: "+lifeGain);
            valueGain = newValue;
            yield return null;
        }
    }
}
