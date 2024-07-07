using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public partial class Vehicle : MonoBehaviour, IInteractable
{
    Animator animator;
    Rigidbody rb;
    AnimationClip[] animationClips;
    [SerializeField] private Camera vehicleCamera;
    public PlayerCamera vehicleCam;
    [SerializeField] private Light[] lights;
    [SerializeField] private bool lightsOn;
    [SerializeField] private bool onVehicle = false;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canRide;
    [SerializeField] private float vehicleTorque = 200.0f;
    [SerializeField] private float vehicleSpeed = 0;
    [SerializeField] private float maxVehicleSpeed = 0;
    [SerializeField] private GameObject playerRef;
    float gravity = 1f;
    Vector3 direction = new Vector3();
    public Transform doorExit;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        HandleVehicleLights();

        if (canMove)
        {
            SimpleVehicleMovement();
            //HandleVehicleMovement();
        }


    }

    private void HandleVehicleLights()
    {
        if (lights != null) 
        {
            if (onVehicle)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    lightsOn = !lightsOn;
                }
            }

            if (lightsOn)
            {
                foreach (Light light in lights)
                {
                    light.enabled = true;
                }
            }
            else
            {
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
            }
        }
    }


    private void SimpleVehicleMovement()
    {
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");

        Vector3 movement = transform.forward * inputZ * vehicleSpeed;

        if (movement != Vector3.zero)
        {
            if( inputZ < 0)
                transform.Rotate(0,-inputX,0);
            else
                transform.Rotate(0, inputX, 0);
        }

        // Update the velocity of the Rigidbody
        rb.velocity = movement;
    }

    void HandleVehicleMovement()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            vehicleSpeed += 0.05f;
            direction += Vector3.forward;

            Debug.Log("Vehicle Moving forward");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            vehicleSpeed -= 0.05f;
            direction += Vector3.back;
            Debug.Log("Vehicle Moving backward");
        }
    }

    private void FixedUpdate()
    {
        

        direction.Normalize();

        Vector3 velocity = direction * vehicleSpeed;

        // Apply gravity to the velocity
        //velocity.y -= gravity;


        if (direction != Vector3.zero)
        {
            Debug.Log(direction);
            rb.AddForce(rb.velocity + velocity * vehicleTorque, ForceMode.VelocityChange);
        }
    }

    public void Interact()
    {
        return;
    }

    void GetOnVehicle()
    {
        
        onVehicle = !onVehicle;
        canMove = !canMove;

        vehicleCamera.enabled = true;
        vehicleCam.enabled = true;
        playerRef.SetActive(false);
        
    }

    public void GetOffVehicle()
    {
        onVehicle = !onVehicle;
        canMove = !canMove;

        vehicleCamera.enabled = false;

        Vector3 spawnPosition = transform.position + transform.right * -3;

        playerRef.transform.position = spawnPosition;
        playerRef.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        playerRef.SetActive(true);
    }

    public string GetInteractText()
    {
        string f;

        f = canRide ? $"Get on vehicle" : "";

        if (onVehicle)
        {
            f = "Get off vehicle";
        }
        
        return f;
    }

    public void InteractWithPlayer(Player1 player)
    {
        if (canRide)
        {
            if (onVehicle)
            {
                GetOffVehicle();
            }
            else
            {
                GetOnVehicle();
            }
        }
    }

}
