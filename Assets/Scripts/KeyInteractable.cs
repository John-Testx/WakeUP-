using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyInteractable : MonoBehaviour
{
    [SerializeField] private int code;
    string keyName;
    int keyLevel;

    public int GetCode()
    {
        return code;
    }

    

    
}
