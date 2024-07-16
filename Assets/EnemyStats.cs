using System.Collections;
using UnityEngine;


public class EnemyStats : CharacterStats
{
    Enemy enemy;
    Rigidbody rb;

    private void Start()
    {
        GetReferences(); InitVariables();
    }

    private void Update()
    {
        if (isDead) { enemy.EnemyIsDead(); } else { return; }
    }

    public void GetReferences()
    {
        enemy = GetComponent<Enemy>();
        
    }

    public override void InitVariables()
    {   
        maxHealth = enemy.enemy.health;
        SetHealthTo(maxHealth);
    }

    public override void Die()
    {
        base.Die();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.AddForce(-transform.forward * 3, ForceMode.Impulse);
        
        StartCoroutine(DieTimer()); 
    }

    IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(8);
        rb.isKinematic = true;
        rb.detectCollisions = false;
        enemy.enabled = false;
        enabled = false;
    }

}
