using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : CharacterStats
{
    Animator animator;
    public GameObject player;
    bool isCovered;
    [SerializeField] float protectionFactor;

    void Start()
    {
        InitVariables();
    }

    // Update is called once per frame
    void Update()
    {

        if (animator != null)
        {
            if (Input.GetMouseButtonDown(1)) //
            {
                StartCoroutine(Cover());
            }
        }

    }

    public override void Die()
    {
        base.Die();
        transform.gameObject.SetActive(false);
    }

    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage)
    {
        if (isCovered)
        {
            damage = (int)(damage /protectionFactor);
        }

        base.TakeDamage(damage);
    }

    IEnumerator Cover()
    {
        if(!isCovered)
        {
            animator.Play("Cover");
            isCovered = true;
        }
        else 
        {
            animator.Play("Uncover");
            isCovered = false;
        }

        yield return new WaitForSeconds(.8f);
    }
}
