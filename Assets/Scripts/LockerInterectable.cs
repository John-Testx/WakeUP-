using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerInterectable : MonoBehaviour, IInteractable
{

    public bool opened;
    public GameObject hinge;

    public string GetInteractText()
    {
        string text = opened ? "Close Locker" : "Open Locker";
        return text;
    }

    
    public void Interact()
    {
        return;
    }

    public void InteractWithPlayer(Player1 player)
    {
        OpenLocker();
    }

    public void OpenLocker()
    {

        

        if (!opened)
        {
            opened = true;
            hinge.transform.Rotate(0, 90, 0);
            Debug.Log("Door opened");
        }

        else
        {
            opened = false;
            hinge.transform.Rotate(0, -90, 0);
            Debug.Log("Door closed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
