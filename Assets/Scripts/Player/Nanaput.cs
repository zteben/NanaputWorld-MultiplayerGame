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
    protected bool canJump;
    protected bool canDoubleJump;


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
                Debug.LogError("boundObject not found.");
        }
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            CheckInput();
        }
    }

    protected virtual void CheckInput()
    {
        Move();
        Jump();
    }

    protected void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        anim.SetBool("isRunning", move != 0);

        if (move < 0)
        {
            photonView.RPC("FlipSR", RpcTarget.All, true);
        }
        else if (move > 0)
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
        if (IsGrounded() && rb.velocity.y == 0)
        {
            canJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() && canJump)
            {
                canJump = false;
                StartCoroutine(DelayDoubleJump(0.2f));
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            else if (canDoubleJump || canJump)
            {
                canJump = false;
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        else if (rb.velocity.y < 0)
        {
            rb.velocity -= fallMultiplier * Time.deltaTime * new Vector2(0, -Physics2D.gravity.y);
        }
    }

    protected bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(25f, 1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    protected IEnumerator DelayDoubleJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDoubleJump = true;
    }
}

