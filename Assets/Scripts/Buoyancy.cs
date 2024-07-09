using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float floatHeight = 2.0f; // Desired floating height
    public float bounceDamp = 0.05f; // Damping to reduce bobbing
    public float waterDensity = 1.0f; // Density of the water

    private Rigidbody rb;
    private Collider waterCollider;
    private bool isInWater = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isInWater && waterCollider != null)
        {
            ApplyBuoyancy();
        }
    }

    void ApplyBuoyancy()
    {
        // Calculate the water level dynamically using the water collider bounds
        float waterLevel = waterCollider.bounds.max.y;
        float depth = waterLevel - transform.position.y;

        if (depth > 0)
        {
            // Calculate the buoyant force
            float buoyantForce = Mathf.Clamp01(depth / floatHeight) * waterDensity * Physics.gravity.y * -rb.mass;

            // Apply the force to the Rigidbody
            rb.AddForce(new Vector3(0, buoyantForce - rb.velocity.y * bounceDamp, 0), ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Object entered water.");
            waterCollider = other;
            isInWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Object exited water.");
            if (other == waterCollider)
            {
                isInWater = false;
                waterCollider = null;
            }
        }
    }
}
