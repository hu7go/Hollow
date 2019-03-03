using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public bool normalAttack = true;

    public Weapon currentWeapon;
    playerController playerController;

    public float force;

    private int myDamage;

    public void Start()
    {
        if (normalAttack)
            myDamage = currentWeapon.damage;
        else
            myDamage = currentWeapon.heavyAttackDamage;

        playerController = GetComponentInParent<playerController>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        int testForce = playerController.currentForce;

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(myDamage, testForce);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, force));
        }
    }
}
