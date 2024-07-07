using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanInteractable : MonoBehaviour, IInteractable, IITask
{
    private bool cleaned;

    public string GetInteractText()
    {
        return "Clean";
    }

    public void Interact()
    {
        return;
    }

    public bool getClean()
    {
        return cleaned;
    }

    public bool IsDone()
    {
        if (cleaned) { return true; }
        return false;
    }

    public void InteractWithPlayer(Player1 player)
    {
        cleaned = true;
        transform.gameObject.SetActive(false);
    }
}
