using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NanaputOld : MonoBehaviour
{
    public PhotonView photonView;
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject cam;
    /*
    public float groundCheckX;
    public float groundCheckY;
    public LayerMask groundLayer;
    public Transform groundTransform;
    */

    public float moveSpeed;
    public float jumpForce;

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
        /*
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Debug.Log("jump!");
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        }*/
    }

    [PunRPC]
    private void FlipSR(bool flip)
    {
        sr.flipX = flip;
    }

    /*

    public bool isGrounded()
    {
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckY, groundLayer) || 
            Physics2D.Raycast((Vector2)groundTransform.position - Vector2.right * groundCheckX, Vector2.down, groundCheckY, groundLayer) || 
            Physics2D.Raycast((Vector2)groundTransform.position + Vector2.right * groundCheckX, Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground check rays in the scene view.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundTransform.position, groundTransform.position + Vector3.down * groundCheckY);

        // Left edge ray.
        Gizmos.DrawLine(groundTransform.position - Vector3.right * groundCheckX, groundTransform.position - Vector3.right * groundCheckX + Vector3.down * groundCheckY);

        // Right edge ray.
        Gizmos.DrawLine(groundTransform.position + Vector3.right * groundCheckX, groundTransform.position + Vector3.right * groundCheckX + Vector3.down * groundCheckY);
    }
    */

}

