using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;

public class ObjectInteractable1 : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public Inventory inventory;
    [SerializeField] private string objectName;
    [SerializeField] private int keyCode; 
    public bool pickable;
    bool canGrab = true;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
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
        else {
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

            canGrab = inventory.AddItem(gameObject);
            Debug.Log("Object Grabbed");
        }
    }
}
