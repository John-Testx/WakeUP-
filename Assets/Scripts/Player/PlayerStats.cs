using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player1 player1;

    private void Start()
    {
        InitVariables();
        CheckHealth(); 
    }


    private void Update()
    {
        if (isDead) { player1.PlayerIsDead(); } else { return; }
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
    }

    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        player1 = GetComponent<Player1>();
    }

    public override void Die()
    {
        base.Die();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.AddForce(-transform.forward * 3,ForceMode.Impulse);
    }
}
