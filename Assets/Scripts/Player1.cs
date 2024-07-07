using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    Vector3 direction;
    Vector3 climbDirection;
    public Rigidbody rb;
    PlayerAnimation playerAnimation;
    public bool isGround;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public float descentForceMultiplier = 3f;
    public float climbForceMultiplier = 5f;
    public float slopeForce = 10f;
    public float slopeRayLength = 0.5f;
    public float air = 0.4f;
    public float rbDrag = 6f;
    public float airdrag = 3f;
    float slopeAngle;
    float slopeDirection;
    float hm;
    float vm;

    [Header("FunctionalOptions")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useAnimation = true;

    [Header("PlayerState")]
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool onSlope;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isSprinting = false;

    private bool wasWalkingLastFrame = false;
    private float maxYvelocity;
    [SerializeField] private float maxFallVelocity;


    [Header("Movement")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float CurrentSpeed;
    [SerializeField] private float crouchSpeed = 6f;
    [SerializeField] private float movementSpeed = 6f;

    public float jumpStrength;

    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] concreteClips = default;
    [SerializeField] private AudioClip[] tilesClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer =0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier: isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        CurrentSpeed = movementSpeed;
        rb.freezeRotation = true;

        if (playerAnimation != null)
        {
            useAnimation = true;
        }
        else { useAnimation = false; }

    }

    // Update is called once per frame
    void Update()
    {
        isGround = CheckIfGrounded();
        PInput();
        
        if (canCrouch)
        {
            Crouch();
        }
        if (canSprint)
        {
            Sprint();
        }
        if (canJump)
        {
            Jump();
        }
        if (useFootsteps)
        {
            HandleFootsteps();
        }

        if (useAnimation)
        {
            ChangePlayerAnimation();
        }
    }

    private void HandleFootsteps()
    {
        if (isGround == false) return;
        if (direction == Vector3.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(transform.position,Vector3.down, out RaycastHit hitInfo, 3)) 
            {
                switch(hitInfo.collider.tag) 
                {
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/CONCRETE":
                        footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                        break;
                    case "Footsteps/METAL":
                        footstepAudioSource.PlayOneShot(metalClips[Random.Range(0, metalClips.Length - 1)]);
                        break;
                    case "Footsteps/GRASS":
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    case "Footsteps/TILES":
                        footstepAudioSource.PlayOneShot(tilesClips[Random.Range(0, tilesClips.Length - 1)]);
                        break;
                }
            }
            footstepTimer = GetCurrentOffset;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
        ControlDrag();
    }
    
    void ChangePlayerAnimation()
    {
        if (wasWalkingLastFrame)
        {
            playerAnimation.idle = false;

            if (isSprinting)
            {
                playerAnimation.running = true;
                playerAnimation.walking = false;
            }
            else
            {
                playerAnimation.walking = true;
                playerAnimation.running = false;
            }
        }
        else if (!wasWalkingLastFrame)
        {
            playerAnimation.idle = true;
            playerAnimation.walking = false;
            playerAnimation.running = false;
        }

        // Store walking state for next frame
        wasWalkingLastFrame = direction != Vector3.zero;
    }


    bool CheckIfGrounded()
    {
        // Check if the player is grounded using a sphere cast and raycast
        bool groundedFromSphere = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        

        return groundedFromSphere ;
    }

    void Jump()
    {
        if (isJumping && rb.velocity.y == 0)
        {
            isJumping = false;
            if (maxYvelocity < maxFallVelocity)
            {
                Debug.Log("You died");
            }

        }
        else if (isJumping)
        {
            if (rb.velocity.y < maxYvelocity)
            {
                maxYvelocity = rb.velocity.y;
            }
        }

        if (isGround & Input.GetKeyDown(KeyCode.Space) & !isJumping )
        {
            isJumping = true;
            maxYvelocity = 0;
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            playerAnimation.jumping = true;
        }

        
        
    }

    bool CheckForSlope() {

        if (isGround)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeRayLength, groundMask))
            {
                if (hit.normal != Vector3.up)
                {   
                    // Calculate the slope angle
                    slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    
                    slopeDirection = Vector3.Angle(hit.normal, direction) - 90;
                    
                    //Debug.Log(slopeDirection);
                    //Debug.Log(transform.forward);

                    // Calculate the climb direction
                    climbDirection = Vector3.Cross(hit.normal, -transform.right);
                    climbDirection.Normalize();
                    climbDirection= direction + climbDirection;
                    

                    //Debug.Log(hit.normal);
                    /*Debug.Log(slopeAngle);
                    Debug.Log(climbDirection);*/
                    return true;
                }
            }
                    
        }
        return false;
    }
    void ClimbSlope()
    {
        // Apply force to climb the slope or reduce speed on descending slope
        if (slopeAngle <= 30f)
        {
            if (slopeDirection > 0)
            {
                
                rb.AddForce(direction.normalized * slopeForce * (CurrentSpeed * 1.2f / climbForceMultiplier) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
            else if (slopeDirection < 0)
            {
                rb.AddForce(direction.normalized * slopeForce * (CurrentSpeed * 0.5f / climbForceMultiplier) * Time.fixedDeltaTime, ForceMode.VelocityChange);

            }
        }
        else
        {
            

        }
    }


    void Crouch()
    {
        

        if (Input.GetKeyDown(KeyCode.C))
        {

            if (!isCrouching)
            {
                //Debug.Log("Bodoque se agacho");
                isCrouching = true;
                CurrentSpeed = crouchSpeed;
                Vector3 scaleChange = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
                transform.localScale -= scaleChange;

            }
            else
            {
                CurrentSpeed = movementSpeed;
                isCrouching = false;
                Vector3 scaleChange = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
                transform.localScale = scaleChange;
            }

            //El vector de scale en transform es como una magnitud por lo que si  
            //al vector de scale le resto con otro vector, el "scale" transform disminuye     
        }
    }

    void MovePlayer()
    {
        if (isGround)
        {
            onSlope = CheckForSlope();
            
            if (onSlope) { ClimbSlope(); }

            else 
            {
                
                rb.AddForce(direction.normalized * CurrentSpeed * speedMultiplier, ForceMode.Acceleration);
                
            }
        }
        else
        {
            rb.AddForce(direction.normalized * CurrentSpeed * air, ForceMode.Acceleration);
        }
    }

    void Sprint()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            CurrentSpeed = isCrouching ? crouchSpeed * 2 : movementSpeed * 2;
            Debug.Log("Sprinting started. Sprinting: " + isSprinting);
        }

        // Check if Left Shift key is released this frame
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            CurrentSpeed = isCrouching ? crouchSpeed : movementSpeed ;
            Debug.Log("Sprinting stopped. Sprinting: " + isSprinting);
        }

    }

    void ControlDrag()
    {

        if (isGround)
        {
            rb.drag = rbDrag;
            
        }
        else
        {
            rb.drag = airdrag;
        }
    }

    void PInput()
    {
        hm = Input.GetAxisRaw("Horizontal");
        vm = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vm * speedMultiplier + transform.right * hm * speedMultiplier;
    }
}
