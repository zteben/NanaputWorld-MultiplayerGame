using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public abstract class Nanaput : MonoBehaviour
{
    public PhotonView photonView;
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject cam;
    private PolygonCollider2D boundCollider;

    public CapsuleCollider2D groundCheckCollider;
    public LayerMask groundLayer;

    public float moveSpeed;
    public float jumpForce;
    public float fallMultiplier;
    public float downForceMultiplier;

    public HealthBar healthBar;
    public float maxHealth;
    public float currentHealth;

    protected bool canJump;
    protected bool canDoubleJump;

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool downPressed;
    private float downHoldTime = 0f;
    private ContactFilter2D dropFilter;


    /*
    #############################################
    -----------------INITIALIZE------------------
    #############################################
    */
    protected virtual void Awake()
    {
        canJump = false;
        canDoubleJump = false;
    }

    protected virtual void Start()
    {
        SetCamera();
        SetHealth();
        dropFilter.SetLayerMask(groundLayer);
        dropFilter.useTriggers = false;
    }

    protected void SetCamera()
    {
        if (photonView.IsMine)
        {
            cam.SetActive(true);

            GameObject boundObject = GameObject.Find("CameraBound");
            if (boundObject != null)
            {
                boundCollider = boundObject.GetComponent<PolygonCollider2D>();
                CinemachineConfiner2D confiner = cam.GetComponent<CinemachineConfiner2D>();
                confiner.m_BoundingShape2D = boundCollider;
            }
            else
            {
                Debug.LogError("CameraBound not found.");
            }
        }
    }

    protected void SetHealth()
    {
        if (photonView.IsMine)
            healthBar.ChangeColor(new Color32(76, 217, 100, 255));
        else
            healthBar.ChangeColor(new Color32(102, 178, 255, 255));
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }


    /*
    #############################################
    -------------------UPDATE--------------------
    #############################################
    */  
    protected virtual void Update()
    {
        if (!photonView.IsMine) return;

        HandleMovementInput();
        HandleAttackInput();
    }

    protected virtual void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        ProcessMovement();
        ProcessAttack();
    }


    /*
    #############################################
    -------------------MOVEMENT------------------
    #############################################
    */
    protected void HandleMovementInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        downPressed = Input.GetKey(KeyCode.DownArrow);

        downHoldTime = downPressed ? downHoldTime + Time.deltaTime : 0f;
    }

    protected void ProcessMovement()
    {
        Move();
        Jump();
        Down();
    }

    protected void Move()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        anim.SetBool("isRunning", moveInput.x != 0);

        if (moveInput.x < 0)
        {
            photonView.RPC("FlipSR", RpcTarget.All, true);
        }
        else if (moveInput.x > 0)
        {
            photonView.RPC("FlipSR", RpcTarget.All, false);
        }
    }

    [PunRPC]
    protected void FlipSR(bool flip)
    {
        sr.flipX = flip;
    }

    protected void Jump()
    {
        if (IsGrounded() && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            canJump = true;
            canDoubleJump = true;
        }

        if (jumpPressed)
        {
            if (IsGrounded() && canJump)
            {
                canJump = false;
                StartCoroutine(DelayDoubleJump(0.15f));
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (canDoubleJump)
            {
                canJump = false;
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            jumpPressed = false;
        }
    }

    protected void Down()
    {
        // Fall faster
        if (!IsGrounded())
        {
            if (downPressed)
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -20f);
                }
                rb.velocity += Vector2.up * Physics2D.gravity.y * (downForceMultiplier - 1);
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
            }
            downHoldTime = 0f;
        }
        // Drop through platform
        else
        {
            if (downPressed && downHoldTime > 0.3f)
            {
                Collider2D[] platforms = new Collider2D[5];
                int hitCount = groundCheckCollider.OverlapCollider(dropFilter, platforms);

                for (int i = 0; i < hitCount; i++)
                {
                    if (platforms[i] != null && platforms[i].CompareTag("Floating Platform"))
                    {
                    StartCoroutine(TemporarilyDisableCollision(platforms[i]));
                    break;
                    }
                }
                downHoldTime = 0f;
            }
                
        }
    }

    protected bool IsGrounded()
    {
        return groundCheckCollider.IsTouchingLayers(groundLayer);
    }

    protected IEnumerator DelayDoubleJump(float delay)
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(delay);
        canDoubleJump = true;
    }

    protected IEnumerator TemporarilyDisableCollision(Collider2D platform)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platform, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platform, false);
    }


    /*
    #############################################
    --------------------ATTACK-------------------
    #############################################
    */
    protected virtual void HandleAttackInput() {}

    protected virtual void ProcessAttack() {}

    [PunRPC]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            // You can add death logic here
            Debug.Log("A player has been eliminated.");
        }
    }

}
