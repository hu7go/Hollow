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
    public GameObject jumpParticles;

    private float privateSpeed = 0;
    private float x = 0;
    private float buttonCooldown = .5f;
    private int buttonCount = 0;
    private bool doubleJump = true;
    private float initJumpThrust;

    private float dashForce;

    [HideInInspector]
    public bool stunned = false;

    Rigidbody2D rb2D;
    AnimationController animController;
    PlayerFeet playerFeet;
    PlayerController playerController;
    WaterController waterController;
    WallJump wj;

    private int totalJumps;

    private IEnumerator coroutine;

    public bool canWallJump = false;
    private bool hasWallJumped = false;

    public bool canDoubleJump = false;
    private bool hasDoubleJumped = false;

    [SerializeField] private AudioSource aSWalk;
    [SerializeField] private AudioSource aSJump;
    private float soundCD;

    public void Start()
    {
        aSWalk = GetComponent<AudioSource>();
        initJumpThrust = jumpThrust;
        privateSpeed = speed;
        animController = GetComponent<AnimationController>();
        rb2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerFeet = GetComponentInChildren<PlayerFeet>();
        waterController = GetComponent<WaterController>();
        wj = GetComponentInChildren<WallJump>();
    }

    void Update()
    {
        if (!playerFeet.grounded && wj.canWallJump)
        {
            canWallJump = true;
        }
        else
        {
            hasWallJumped = false;
            canWallJump = false;
        }

        if (playerFeet.grounded)
        {
            hasDoubleJumped = false;
            canDoubleJump = false;
        }
        else
        {
            canDoubleJump = true;
        }

        if (!playerController.isBlocking && stunned == false)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("JumpPad")))
            {
                //Normal Jump!
                if (playerFeet.grounded)
                {
                    Jump();
                    return;
                }

                //Wall Jump!
                if (!playerFeet.grounded && canWallJump && !hasWallJumped)
                {
                    hasWallJumped = true;
                    Jump();
                    return; 
                }

                //Double Jump!
                if (!playerFeet.grounded && canDoubleJump && !hasDoubleJumped)
                {
                    hasDoubleJumped = true;
                    var tmp = Instantiate(jumpParticles, transform.position, transform.rotation);
                    Destroy(tmp, 1);
                    Jump();
                    return;
                }
            }
        }
    }

    public void Jump()
    {
        aSJump.pitch = Random.Range(1f, 1.5f);
        aSJump.Play();

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(transform.up * jumpThrust);

        totalJumps++;
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

        FootSteps();

        x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        
        Vector2 movement = new Vector2(x * speed, rb2D.velocity.y);

        rb2D.velocity = movement;

        //DashLogic();
        animController.AnimationHandler(x);
    }

    public int TotalJumps ()
    {
        return totalJumps;
    }

    //Sound for footsteps
    public void FootSteps ()
    {
        if (playerFeet.grounded == false)
            return;

        if (rb2D.velocity.sqrMagnitude == 0)
        {
            return;
        }

        soundCD -= .1f;

        if (soundCD <= 0)
        {
            aSWalk.pitch = Random.Range(0.6f, 0.9f);
            aSWalk.Play();

            soundCD = 2f;
        }
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

        if (Input.GetKeyDown(KeyCode.G) && !playerController.isBlocking && playerController.attackCooldown <= 0)
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
