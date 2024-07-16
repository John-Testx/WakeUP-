using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlenderInteractable : MonoBehaviour, IInteractable
{
    public static bool opened;
    public bool reachedMaxAmount;
    //public int maxAmount;
    public GameObject blenderLid;
    public bool isBlenderLid;
    

    public string GetInteractText()
    {
        if ( isBlenderLid )
        {
            if (!opened) { return "Open Blender"; }
            else { return "Put Lid"; }
        }

        else if (!reachedMaxAmount && opened)
        {
            return "Add Ingredient";
        }

        if (reachedMaxAmount && !opened) { return "Turn On"; }


        else { return ""; }
        
    }

    public void Interact()
    {
        return;
    }

    public void InteractWithPlayer(Player1 player)
    {
        CheckBlender();
    }

    void CheckBlender() 
    { 
        if (isBlenderLid )
        {
            if(!opened) 
            {
                opened = true;
                blenderLid.transform.localPosition = new Vector3 (-1, -0.5f, -1);
            }
            else 
            {
                opened = false;
                blenderLid.transform.localPosition = new Vector3(0, 0, 1.144f);
            }
            
        }



    }
}
