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

    public float moveSpeed;
    public float jumpForce;

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            cam.SetActive(true);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            CheckInput();
        }
    }

    protected virtual void CheckInput()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move == -1)
        {
            photonView.RPC("FlipSR", RpcTarget.All, true);
            anim.SetBool("isRunning", true);
        }
        else if (move == 1)
        {
            photonView.RPC("FlipSR", RpcTarget.All, false);
            anim.SetBool("isRunning", true);
        }

        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    [PunRPC]
    protected void FlipSR(bool flip)
    {
        sr.flipX = flip;
    }
}

