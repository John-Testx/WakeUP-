using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraBehavior : MonoBehaviour
{
    public float rotationAmount;
    
    public float maxRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount * Mathf.Sin(Time.deltaTime) , 0);
        
        float rotationAngle = transform.parent.eulerAngles.y - transform.eulerAngles.y ;

        /*  Debug.Log(transform.eulerAngles.y);
            Debug.Log(transform.parent.eulerAngles.y);
            Debug.Log(rotationAngle);
        */
        
        if (rotationAngle <= -maxRotation || rotationAngle >= maxRotation) 
        {
            rotationAmount = -1 * rotationAmount;
        }
    }
}
