using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    public GameObject player;
    


    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (animator != null)
        {
            if (Input.GetMouseButtonDown(0)) //
            {
                StartCoroutine(Attack());
            }
        }

    }

    IEnumerator Attack()
    {
        animator.Play("Attack1");
        yield return new WaitForSeconds(.8f);
        

    }

}
