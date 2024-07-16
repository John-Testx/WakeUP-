using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Rigidbody rb;
    public float depthBeforeSubmerged = 2.0f; 
    public float displacementAmount = 0.05f;

    private void FixedUpdate()
    {
        if (transform.position.y < 0)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
