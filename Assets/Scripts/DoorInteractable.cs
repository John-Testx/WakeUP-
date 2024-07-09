using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DoorInteractable : MonoBehaviour, IInteractable {

    public Inventory inventory;
    public GameObject player;
    public Animator animator;
    string text;
    public int doorType;
    public bool opened;
    public bool isLocked;
    [SerializeField] private int keyCode;


    void Start()
    {
        animator = GetComponent<Animator>();
        inventory = FindObjectOfType<Inventory>();
    }


    public void Interact()
    {
        if (animator != null) { CheckDoor(); }
    }

    void CheckDoor()
    {   
        if (!isLocked) { OpenDoor(); }
        
        else
        {
            //Debug.Log("DoorLocked");
            if (inventory.GetKey(keyCode) == keyCode)
            {
                UnlockDoor();
            }
            else { setText("Door is Locked"); }
        }
    }

    public void UnlockDoor() 
    {
        isLocked = false;
    }

    public void OpenDoor()
    {

        GameObject hinge = transform.parent.gameObject;

        if (!opened)
        {
            StartCoroutine(opening());
            /*opened = true;
            hinge.transform.Rotate(0, -90, 0);
            Debug.Log("Door opened");
            */
        }

        else
        {
            StartCoroutine(closing());
            /*
            opened = false;
            hinge.transform.Rotate(0, 90, 0);
            Debug.Log("Door closed");
            */
        }
    }

    public string GetInteractText()
    {
        text = opened ? "Close Door" : "Open Door";
        return text;
    }

    void setText( string newText)
    {
        text = newText;
    }


    IEnumerator opening()
    {
        print("you are opening the door");
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        if (doorType == 2 ) { animator.Play("OpeningDoor2"); }
        else {animator.Play("Opening 1");}
        opened = true;
        yield return new WaitForSeconds(.5f);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
    }

    IEnumerator closing()
    {
        print("you are closing the door");
        if (doorType == 2) { animator.Play("ClosingDoor2"); }
        else { animator.Play("Closing 1"); }
        opened = false;
        yield return new WaitForSeconds(.5f);
    }

    public void InteractWithPlayer(Player1 player)
    {
        if (animator != null) { CheckDoor(); }
    }

    
}
