using UnityEngine;

public class Skeleton : EnemyStats, IEnemyType
{
    Animator animController;
    //Get the animator
    public override void Start()
    {
        base.Start();
        animController = GetComponent<Animator>();
    }

    //All of these functions are for different animations for this specific enemy
    public void TakeDamage ()
    {
        if (health <= 0)
        {
            DieAnim();
            return;
        }
        animController.SetTrigger("TakeDamage");
    }

    public void DieAnim ()
    {
        animController.SetTrigger("Die");
    }

    public void Walking()
    {
        animController.SetBool("Walking", true);
    }

    public void StopWalking()
    {
        animController.SetBool("Walking", false);
    }

    public void Attacking()
    {
        animController.SetTrigger("Attack");
    }

    public void Die ()
    {

    }
}
