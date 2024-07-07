using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementInteractable : MonoBehaviour , IInteractable
{
    public enum SupplementType
    {
        Protein = 0,
        Creatine = 1,
        EnergyDrink= 2,
        Vitamin= 3,
        Trenbolone = 4,
        Steroids= 5,
    }

    public SupplementType Type;

    public string GetInteractText()
    {
        return $"Grab {Type}";
    }

    public void Interact()
    {
        gameObject.SetActive(false);
        return;
    }

    void ReactionType() {
        
        switch (Type)
        {
            case SupplementType.Protein:
                break;
            case SupplementType.Creatine:
                break;
            case SupplementType.EnergyDrink:
                break;
            case SupplementType.Vitamin:
                break;
            case SupplementType.Steroids:
                break;

        }
        return;
    }

    public void InteractWithPlayer(Player1 player)
    {
        return;
    }

    
}
