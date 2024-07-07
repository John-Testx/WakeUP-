using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectInteractable : MonoBehaviour
{
    public string objectType;
    public bool opened;
    public bool unlocked;
    [SerializeField] private int code;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCode()
    {
        return code;
    }

    public void UseDoorInteractable(Object item, int key)
    {
        if (item != null)
        {
            if (!unlocked & key != 0)
            {
                unlocked = true;
            }
            
            GameObject hinge = item.GameObject().transform.parent.gameObject;

            if (!opened)
            {
                opened = true;
                hinge.transform.Rotate(0, -90, 0);
                Debug.Log("Door opened");
            }

            else
            {
                opened = false;
                hinge.transform.Rotate(0, 90, 0);
                Debug.Log("Door closed");
            }

        }
    }


    public string GetObjectType()
    {
        return objectType;
    }

    public void Interact()
    {
        return;
    }

    public string GetInteractText()
    {
        return "";
    }
}
