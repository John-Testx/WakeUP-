using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable2 : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    Inventory1 inventory1;
    [SerializeField] private Item item;
    [SerializeField] private string objectName;
    [SerializeField] private int keyCode;
    public Item Item => item;
    public bool pickable;
    bool canGrab = true;

    void Start()
    {
        inventory1 = FindObjectOfType<Inventory1>();
    }

    public string GetInteractText()
    {
        if (objectName != "")
        {
            return "Grab " + objectName;
        }
        else if (!canGrab)
        {
            return "Can't grab any more items";
        }
        else
        {
            return "Grab Item";

        }
    }

    public void Interact()
    {

        return;
    }

    public int GetKeyCode()
    {
        return keyCode;
    }

    public void InteractWithPlayer(Player1 player)
    {
        if (pickable)
        {
            inventory1.GrabItem(gameObject);
            //Debug.Log("Object Grabbed");
        }
    }
}
