using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyBoat : MonoBehaviour
{
    public float floatHeight = 2.0f; // The height at which the object should float
    public float bounceDamp = 0.05f; // Damping to reduce bouncing
    public Vector3 buoyancyCentreOffset; // Offset for the buoyancy center
    public float waterDensity = 1.0f;


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
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("Collider component not found.");
            return;
        }

        Bounds bounds = collider.bounds;

        // Transform buoyancyCentreOffset into world space relative to boat's transform
        Vector3 actionPoint = transform.TransformPoint(buoyancyCentreOffset);

        // Visualize actionPoint in Scene view
        Debug.DrawLine(transform.position, actionPoint, Color.blue);

        // Calculate the water level dynamically using the water collider bounds
        float waterLevel = WaterHeight(bounds.center);
        
        // Visualize water level
        Debug.DrawLine(new Vector3(actionPoint.x - 1f, waterLevel, actionPoint.z), new Vector3(actionPoint.x + 1f, waterLevel, actionPoint.z), Color.green);

        // Calculate the depth of the object relative to the water surface
        float depth = waterLevel - actionPoint.y;

        rb.AddForceAtPosition(Physics.gravity, transform.position, ForceMode.Acceleration);

        // Apply buoyant force if the object is below the water surface
        if (depth > 0)
        {
            // Calculate the buoyant force
            float buoyantForce = Mathf.Clamp01(depth / floatHeight) * waterDensity * -Physics.gravity.y * rb.mass;

            // Apply the buoyant force at the action point
            
            rb.AddForceAtPosition(new Vector3(0, buoyantForce - rb.velocity.y * bounceDamp, 0), actionPoint - buoyancyCentreOffset, ForceMode.Acceleration);

            // Debug: Visualize applied force
            Debug.DrawRay(actionPoint, new Vector3(0, buoyantForce, 0), Color.yellow);
        }
    }

    float CalculateSubmergedVolume(Bounds bounds)
    {
        float submergedVolume = 0f;

        // Calculate the submerged volume based on water level and boat bounds
        float waterLevel = WaterHeight(bounds.center);
        float bottom = Mathf.Max(bounds.min.y, waterLevel - bounds.extents.y);
        float top = Mathf.Min(bounds.max.y, waterLevel + bounds.extents.y);

        if (top > bottom)
        {
            submergedVolume = bounds.size.x * (top - bottom) * bounds.size.z;
        }

        return submergedVolume;
    }

    // Example logic for WaterHeight: Replace this with your specific logic
    float WaterHeight(Vector3 position)
    {
        if (waterCollider != null)
        {
            return waterCollider.bounds.max.y;
        }
        else
        {
            Debug.LogWarning("Water collider is not assigned. Defaulting to y=0.");
            return 0f; // Default water level if no collider is assigned
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("Object entered water.");
            waterCollider = other;
            isInWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("Object exited water.");
            if (other == waterCollider)
            {
                isInWater = false;
                waterCollider = null;
            }
        }
    }

}
