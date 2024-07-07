using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeObjectiveMessage(string message)
    {
        if (text2 != null)
        {   
            text2.SetText(message);
            text2.enabled = true;
        }
    }

    public void ShowInteractMessage(bool show, string message)
    {
        text.SetText(message);
        text.enabled= show;
    }

}
