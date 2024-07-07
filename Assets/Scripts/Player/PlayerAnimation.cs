using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public bool walking;
    public bool idle;
    public bool jumping;
    public bool running;
    public int climbing;
    public bool sitting;
    public bool inspecting;

    private Coroutine idleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            if (idle && idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(IdleState());
            }
            else if (!idle && idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            else if (climbing != 0)
            {   
                // Do something when climbing
                switch (climbing)
                {
                    case 1:
                        StartCoroutine(ClimbLow());
                        break;
                    case 2:
                        StartCoroutine(ClimbHigh());
                        break;
                }
            }
            else if (walking)
            {
                // Do something when walking

                animator.Play("Rig_walk");

                //StartCoroutine(Walk());
            }
            else if (running)
            {
                animator.Play("Rig_run");
            }
            else if (sitting)
            {
                // Do something when sitting
                StartCoroutine(Sit());
            }
            else if (inspecting)
            {
                // Do something when inspecting
                StartCoroutine(Inspect());
            }
            else if (jumping)
            {
                // Do something when jumping
                StartCoroutine(Jump());
            }
            else
            {
                // Default behavior when no animation state is active
            }
        }

    }

    /*IEnumerator Walk()
    {
        animator.SetBool("Walking", walking);
        
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Walking", !walking);
    }*/

    IEnumerator Jump()
    {
        animator.Play("Rig_jump_start");
        jumping = false;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator IdleState()
    {
        if (idle)
        {
            int idleNum = Random.Range(1, 4);
            string idleName = string.Format("Rig_Idle{0}", idleNum);
            print(idleName);
            animator.Play(idleName);
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator ClimbHigh()
    {
        print("you are climbing");

        animator.Play("Rig_climb_high");
        climbing = 0;
        yield return new WaitForSeconds(.5f);
    }
    IEnumerator ClimbLow()
    {
        print("you are climbing");

        animator.Play("Rig_climb_low");
        climbing = 0;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator Sit()
    {
        print("you are sitting");

        animator.Play("Rig_sit_idle");
        sitting = false;
        yield return new WaitForSeconds(.5f);
    }
    IEnumerator Inspect()
    {
        print("you are climbing");

        animator.Play("Rig_inspect_ground_start");
        inspecting = false;
        yield return new WaitForSeconds(.5f);
    }

}
