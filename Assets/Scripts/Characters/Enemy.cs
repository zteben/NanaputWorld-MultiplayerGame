using UnityEngine;

public class Enemy : Nanaput
{
    //public Transform targetPlayer;
    //public float detectionRange = 5f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        cam = null;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    protected override void Update()
    {
        HandleAIMovement();
        HandleAIAttack();
    }

    protected override void FixedUpdate()
    {
        ProcessAIMovement();
        ProcessAIAttack();
    }

    protected void HandleAIMovement() {}

    protected void ProcessAIMovement() {}

    protected void HandleAIAttack() {}

    protected void ProcessAIAttack() {}
}
