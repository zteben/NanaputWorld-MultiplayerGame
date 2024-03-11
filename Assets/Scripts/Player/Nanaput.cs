using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Nanaput : MonoBehaviour
{
    public PhotonView photonView;
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject cam;
    
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float moveSpeed;
    public float jumpForce;
    public float fallMultiplier;
    protected bool canDoubleJump;


    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            cam.SetActive(true);
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
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
                StartCoroutine(DelayDoubleJump(0.2f));
            }

            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        if (IsGrounded())
        {
            canDoubleJump = true;
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

