using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinInteractable : MonoBehaviour, IInteractable
{
    public bool opened;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Interact()
    {
        return;   
    }

    void OpenCabin() {

        //Debug.Log("Interact");
        //Vector3 z = transform.forward;
        //opened = !opened;

        //Debug.Log(z);


        if (!opened)
            StartCoroutine(opening());
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.38f);
        else
            StartCoroutine(closing());
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.38f);
    }

    public string GetInteractText()
    {
        return "Open/Close Cabin";
    }

    IEnumerator opening()
    {
        print("you are opening the door");
        animator.Play("OpenCabin");
        opened = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing()
    {
        print("you are closing the door");
        animator.Play("CloseCabin");
        opened = false;
        yield return new WaitForSeconds(.5f);
    }

    public void InteractWithPlayer(Player1 player)
    {
        OpenCabin();
    }
}
