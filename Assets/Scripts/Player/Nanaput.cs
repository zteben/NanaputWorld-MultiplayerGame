using System.Collections;
using UnityEngine;
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

    public Transform groundCheck;
    public LayerMask groundLayer;

    public float moveSpeed;
    public float jumpForce;
    public float fallMultiplier;
    public float downForceMultiplier;

    protected bool canJump;
    protected bool canDoubleJump;

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool downPressed;

    protected virtual void Awake()
    {
        canJump = false;
        canDoubleJump = false;
    }

    protected virtual void Start()
    {
        SetCamera();
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

    protected virtual void Update()
    {
        if (!photonView.IsMine) return;

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        downPressed = Input.GetKey(KeyCode.DownArrow);
    }

    protected virtual void FixedUpdate()
    {
        if (!photonView.IsMine) return;

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
        if (!IsGrounded())
        {
            if (downPressed)
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                rb.velocity += Vector2.up * Physics2D.gravity.y * (downForceMultiplier - 1);
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
            }
        }
    }

    protected bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 0.2f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    protected IEnumerator DelayDoubleJump(float delay)
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(delay);
        canDoubleJump = true;
    }
}
