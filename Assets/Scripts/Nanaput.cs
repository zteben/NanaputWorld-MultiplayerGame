using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Nanaput : MonoBehaviour
{
    public PhotonView photonView;
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject cam;

    public float MoveSpeed;
    public float JumpForce;
    public bool isGrounded = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            cam.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * MoveSpeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            photonView.RPC("FlipSR", RpcTarget.All, true);
            anim.SetBool("isRunning", true);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
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
    private void FlipSR(bool flip)
    {
        sr.flipX = flip;
    }
}

