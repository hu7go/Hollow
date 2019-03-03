using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public bool goingRight = true;
    [HideInInspector]
    public bool isDashing = false;
    public float speed = 5f;
    public float jumpThrust = 10f;

    private float privateSpeed = 0;
    private float x = 0;
    private float buttonCooldown = .5f;
    private int buttonCount = 0;

    private float dashForce;

    [HideInInspector]
    public bool stunned = false;

    Rigidbody2D rb2D;
    AnimationController animController;
    PlayerFeet playerFeet;
    playerController playerController;

    private IEnumerator coroutine;

    public void Start()
    {
        privateSpeed = speed;
        animController = GetComponent<AnimationController>();
        rb2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<playerController>();
        playerFeet = GetComponentInChildren<PlayerFeet>();
    }

    void FixedUpdate ()
    {
        if (!playerController.isBlocking && stunned == false)
        {
            Moving();
        }
	}

    private void Moving()
    {
        if (animController.isAttacking && playerFeet.grounded || playerController.isBlocking)
        {
            speed = 0;
        }
        else
        {
            speed = privateSpeed;
        }

        x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        
        Vector2 movement = new Vector2(x * speed, rb2D.velocity.y);

        rb2D.velocity = movement;

        if (Input.GetKeyDown(KeyCode.Space) && playerFeet.grounded == true)
        {
            Jump();
        }

        //DashLogic();
        animController.AnimationHandler(x);
    }

    public void Jump ()
    {
        rb2D.AddForce(transform.up * jumpThrust);
    }

    /*
    public void DashLogic ()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            dashForce = 250;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            dashForce = -250;
        }

        if (Input.GetKeyDown(KeyCode.F) && !playerController.isBlocking && playerController.attackCooldown <= 0)
        {
            Dash(dashForce);
        }

        //if (buttonCooldown > 0)
        //{
        //    buttonCooldown -= 1 * Time.deltaTime;
        //}
        //else
        //{
        //    buttonCount = 0;
        //}
    }

    private void Dash(float forceAmount)
    {
        animController.DashAnim();
        DashAnimStart();
        rb2D.AddForce(new Vector2(forceAmount * 10, 0));
    }

    public void DashAnimStart ()
    {
        isDashing = true;
        coroutine = StopDashing();
        StartCoroutine(coroutine);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }
    */
}
