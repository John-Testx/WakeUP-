using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    
    public void InteractWithPlayer(Player1 player);
    public void Interact();
    public string GetInteractText();
}
