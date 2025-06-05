using UnityEngine;
using Photon.Pun;

public class Barza : Nanaput
{
    private bool attackPressed = false;
    public LayerMask attackableLayer;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            attackPressed = true;
        }
    }

    protected override void ProcessAttack()
    {
        if (!attackPressed) return;

        float attackRange = 10f;
        float damageAmount = 100f;

        Vector2 attackPosition = (Vector2)transform.position + (sr.flipX ? Vector2.left : Vector2.right) * attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange, attackableLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Nanaput>(out Nanaput enemy))
            {
                if (enemy.photonView != null)
                {
                    enemy.photonView.RPC("TakeDamage", RpcTarget.All, damageAmount);
                }
            }
        }

        attackPressed = false;
    }



}
