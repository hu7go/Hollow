using System.Collections;
using UnityEngine;

public class PlayerController : ChangeDirection
{
    Movement movement;
    AnimationController animController;
    BoxCollider2D mainCollider;
    PlayerWeapon weapon;
    UIManager uiManager;

    public bool playerTwo = false;

    [Space(20)]
    [SerializeField] private float stunDuration = .5f;
    private float stunned = 0f;
    [SerializeField] private Vector3 tmpTeleport;
    [SerializeField] private float teleportCooldown;

    [Header("Physics materials")]
    public PhysicsMaterial2D mainMaterial;
    public PhysicsMaterial2D blockMaterial;
    [Space(20)]

    public float attackCooldown = 1f;
    public bool isBlocking = false;

    [Space(20)]
    public GameObject blockSparks;
    public GameObject bloodEffect;
    [SerializeField] private GameObject emptyPotion;

    [HideInInspector]
    public int currentForce;

    private bool inRange = false;
    private float tmpDirection = .65f;
    private bool canTeleport = true;

    private PlayerStruct myPlayer;

    private int totalAttacks;
    private int totalAttacksHit;
    private int hitsBlocked;

    public Potion currentPotion;

    [SerializeField] private AudioSource aS;
    [SerializeField] private AudioClip fast;
    [SerializeField] private AudioClip slow;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip drink;

    public void Start()
    {
        mainCollider = GetComponent<BoxCollider2D>();
        movement = GetComponent<Movement>();
        animController = GetComponent<AnimationController>();
        uiManager = FindObjectOfType<UIManager>();
        stunned = stunDuration;

        weapon = GetComponentInChildren<PlayerWeapon>();

        PotionSprite();
    }

    public void Update()
    {
        if (movement.stunned)
        {
            stunned = stunned - 1 * Time.deltaTime;
        }
        if (stunned <= 0)
        {
            movement.stunned = false;
            stunned = stunDuration;
        }

        attackCooldown -= 1.5f * Time.deltaTime;

        if (attackCooldown < 0)
        {
            attackCooldown = 0;
        }

        CheckWhichDirection();

        ///Fast Attack
        if ((Input.GetMouseButtonDown(0) || (Input.GetAxis("Attack01") != 0)) && attackCooldown <= 0 && isBlocking == false)
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.clip = fast;
            aS.Play();

            attackCooldown = 1;
            animController.Attack01();
            StartCoroutine(AttackBox(.2f, true));

            totalAttacks++;
        }
        ///Heavy Attack
        if ((Input.GetMouseButtonDown(1) || (Input.GetAxis("Attack02") != 0)) && attackCooldown <= 0 && isBlocking == false)
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.clip = slow;
            aS.Play();

            attackCooldown = 1.5f;
            animController.Attack02();
            StartCoroutine(AttackBox(.4f, false));

            totalAttacks++;
        }
        if ((Input.GetKey(KeyCode.Q) || Input.GetButton("Block")) && attackCooldown <= 0)
        {
            mainCollider.sharedMaterial = blockMaterial;
            isBlocking = true;
            animController.BlockAnim(isBlocking);
        }
        if ((Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Block")) && attackCooldown <= 0)
        {
            mainCollider.sharedMaterial = mainMaterial;
            isBlocking = false;
            animController.BlockAnim(isBlocking);
        }

        if (Input.GetKey(KeyCode.F))
        {
            Debug.DrawLine(transform.position + new Vector3(0,0.1f,0), Camera.main.ScreenToWorldPoint(new Vector3((Input.mousePosition.x), (Input.mousePosition.y), 1)), Color.red);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (canTeleport)
                Teleport();
        }

        //Use a potion
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Potion"))
        {
            if (currentPotion != null)
            {
                GameObject tmpPotion;
                tmpPotion = Instantiate(currentPotion.emptyPotion, transform.position + new Vector3(0,.7f,0), transform.rotation);
                tmpPotion.GetComponent<Rigidbody2D>().AddForce(new Vector2(100 * transform.localScale.normalized.x, 100));
                tmpPotion.GetComponent<Rigidbody2D>().AddTorque(Random.Range(3, 7));
                currentPotion.effect.Effect(this);

                PotionSprite();
            }
        }
    }

    private IEnumerator AttackBox(float timeToAttack, bool tmpLightAttack)
    {
        yield return new WaitForSeconds(timeToAttack);

        //Create a linecast that checks if there is a enemy there
        var tmp = Physics2D.Linecast(transform.position + new Vector3(0, 0.3f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y + 0.3f) , 1 << LayerMask.NameToLayer("Enemy"));
        var tmp2 = Physics2D.Linecast(transform.position + new Vector3(0, 0.4f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y + 0.7f), 1 << LayerMask.NameToLayer("Enemy"));
        var tmp3 = Physics2D.Linecast(transform.position + new Vector3(0, 0.2f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y - 0.1f), 1 << LayerMask.NameToLayer("Enemy"));

        Debug.DrawLine(transform.position + new Vector3(0, 0.3f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y + 0.3f), Color.red);
        Debug.DrawLine(transform.position + new Vector3(0, 0.4f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y + 0.7f), Color.red);
        Debug.DrawLine(transform.position + new Vector3(0, 0.2f, 0), new Vector2(transform.position.x + tmpDirection, transform.position.y - 0.1f), Color.red);

        //If the raycast hits a enemy
        if (tmp || tmp2 || tmp3)
        {
            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.clip = hit;
            aS.Play();

            totalAttacksHit++;
            weapon.Attack(tmp.transform.gameObject, tmpLightAttack);
        }
    }

    private void CheckWhichDirection()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
        {
            if (transform.localScale.x < 0)
            {
                ChangeingDirection();
            }
            tmpDirection = .65f;
            currentForce = 1;
            weapon.force = weapon.currentWeapon.damageForce;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            if (transform.localScale.x > 0)
            {
                ChangeingDirection();
            }
            tmpDirection = -0.65f;
            currentForce = -1;
            weapon.force = -1 * weapon.currentWeapon.damageForce;
        }
    }

    //public void StopAttack(int whichAttack)
    //{
    //    if (whichAttack == 1)
    //    {
    //        attackOneHB.SetActive(false);
    //    }
    //    if (whichAttack == 2)
    //    {
    //        attackTwoHB.SetActive(false);
    //    }
    //}

    public void KnockBack(bool blocked, float knockbackForce, bool left = true)
    {
        int tmpNumber;
        if (left)
            tmpNumber = 1;
        else
            tmpNumber = -1;

        if (blocked)
        {
            GameObject tmpEffect = Instantiate(blockSparks, transform.position + new Vector3(.5f, .5f), transform.rotation, transform);
            Destroy(tmpEffect, 1f);
            hitsBlocked++;
        }
        else
        {
            GameObject tmpEffect = Instantiate(bloodEffect, transform.position + new Vector3(.5f, .5f), transform.rotation, transform);
            Destroy(tmpEffect, 1f);
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(tmpNumber* (-2f * knockbackForce),(1f * knockbackForce)), ForceMode2D.Impulse);

        movement.stunned = true;
    }

    private void Teleport()
    {

        if (Physics2D.Raycast(transform.position + new Vector3(0, 0.1f, 0), Camera.main.ScreenToWorldPoint(new Vector3((Input.mousePosition.x), (Input.mousePosition.y), 1))).rigidbody.tag == "Ground")
        {
            //return;
        }

        canTeleport = false;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3((Input.mousePosition.x), (Input.mousePosition.y), 1));
        StartCoroutine(TeleportCooldown());
    }

    IEnumerator TeleportCooldown ()
    {
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    public void StartScene ()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GetCurrentLevel() > 1)
            {
                PlayerStruct tmpPlayer = GameManager.Instance.LoadPlayer();

                myPlayer = tmpPlayer;

                //myPlayer.currentArmor = tmpPlayer.currentArmor;
                //myPlayer.currentWeapon = tmpPlayer.currentWeapon;
                //myPlayer.currentHealth = tmpPlayer.currentHealth;
                //myPlayer.currentMoney = tmpPlayer.currentMoney;
                //myPlayer.currentPotion = tmpPlayer.currentPotion;

                LoadPlayerStats();
            }
        }
    }

    private void LoadPlayerStats ()
    {
        GetComponentInChildren<PlayerArmor>().currentArmor = myPlayer.currentArmor;
        GetComponentInChildren<PlayerWeapon>().currentWeapon = myPlayer.currentWeapon;
        GetComponentInChildren<PlayerHealth>().SetCurrentHealth(myPlayer.currentHealth);
        GetComponentInChildren<PlayerArmor>().LoadArmor(myPlayer.currentArmorValue);
        GetComponentInChildren<CoinHandler>().SetMoney(myPlayer.currentMoney);
        currentPotion = myPlayer.currentPotion;
    }

    public void QuitScene ()
    {
        if (GameManager.Instance != null)
        {
            myPlayer.currentArmor = GetComponentInChildren<PlayerArmor>().currentArmor;
            myPlayer.currentWeapon = weapon.currentWeapon;
            myPlayer.currentHealth = GetComponentInChildren<PlayerHealth>().GetCurrentHealth();
            myPlayer.currentMoney = GetComponentInChildren<CoinHandler>().GetCurrentMoney();
            myPlayer.currentArmorValue = GetComponentInChildren<PlayerArmor>().SaveArmor();
            myPlayer.currentPotion = currentPotion;

            GameManager.Instance.SavePlayer(myPlayer);
        }
    }

    public int TotalAttacks ()
    {
        return totalAttacks;
    }

    public int AttacksHit ()
    {
        return totalAttacksHit;
    }

    public int HitsBlocked ()
    {
        return hitsBlocked;
    }

    public void UsedPotion()
    {
        aS.volume = 0.8f;
        aS.pitch = Random.Range(0.8f, 1.2f);
        aS.clip = drink;
        aS.Play();
        aS.volume = 0.4f;

        currentPotion = null;
    }

    public void PotionSprite ()
    {
        if (currentPotion == null)
        {
            uiManager.UpdatePotion(null);
            return;
        }

        uiManager.UpdatePotion(currentPotion.image);
    }
}

