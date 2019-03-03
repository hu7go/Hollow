using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Movement movementScript;
    PlayerController playerController;
    PlayerFeet playerFeet;

    private IEnumerator coroutine;
    [HideInInspector]
    public bool isAttacking = false;

    void Awake ()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementScript = GetComponent<Movement>();
        playerController = GetComponent<PlayerController>();
        playerFeet = GetComponentInChildren<PlayerFeet>();
    }

    public void AnimationHandler(float direction)
    {
        if (playerFeet.grounded == false)
        {
            animator.SetBool("OnGround", false);
        }
        else
        {
            animator.SetBool("OnGround", true);
        }

        if (direction == 0)
        {
            animator.SetBool("Moving", false);
        }

        //if (direction < 0 || direction > 0)
        //{
        //    animator.SetBool("Moving", true);
        //}
        if (direction < 0)
        {
            movementScript.goingRight = false;
            animator.SetBool("Moving", true);
        }

        if (direction > 0)
        {
            movementScript.goingRight = true;
            animator.SetBool("Moving", true);
        }
    }

    public void DashAnim ()
    {
        animator.SetBool("Dashing", true);
        coroutine = Dash();
        StartCoroutine(coroutine);
    }

    public void Attack01 ()
    {
        animator.SetBool("Attack01", true);
        isAttacking = true;
        coroutine = StopAttack(1, .75f);
        StartCoroutine(coroutine);
    }

    public void Attack02 ()
    {
        animator.SetBool("Attack02", true);
        isAttacking = true;
        coroutine = StopAttack(2, 1f);
        StartCoroutine(coroutine);
    }

    private IEnumerator StopAttack(int whichAttack, float animSpeed)
    {
        yield return new WaitForSeconds((animSpeed / 2));
        animator.SetBool("Attack01", false);
        animator.SetBool("Attack02", false);
        isAttacking = false;
        //playerController.StopAttack(whichAttack);
    }

    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Dashing", false);
    }

    public void BlockAnim (bool isBlocking)
    {
        animator.SetBool("Blocking", isBlocking);


        //TODO: make this work (or something like it)
        //animator.SetFloat("Blocking", 1);
    }
}
