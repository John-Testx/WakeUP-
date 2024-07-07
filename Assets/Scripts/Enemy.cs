using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class Enemy : MonoBehaviour
{
    
    public LayerMask groundMask;
    public Transform groundCheck;

    public float sphereRadius = 0.3f;
    public bool isGround;

    Coroutine wanderCoroutine;

    public List<Transform> pathList;
    public Transform legs;
    Transform lastSeenPosition;
    public Transform currentRoute;
    public int currentRouteIndex = 0;
    public int previousRouteIndex = 0;
    Vector3 currentVelocity;
    Vector3 avoidanceDirection;
    Vector3 wanderDirection;

    [Header("EnemyState")]
    public bool isWalking;
    public bool chasing;
    public bool isWandering;
    public bool canSeePlayer;
    public bool obstruction;

    [Header("FunctionalOptions")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool followPath = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("EnemyFOV")]
    public float radius;
    [Range(0,360)]
    public float angle;
    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    
    Ray ray;
    Ray interactRay;
    Ray legsRay;
    
    public float enemySpeed = 0.7f;
    public float wanderSpeed = 0.7f;
    public float wanderElapsedTime = 0f;
    public float wanderTime = 10f;
    public float interactionDistance = 3f;
    public float jumpStrength;
    public float jumpCounter;
    public float maxObstacleHeight;
    public float avoidanceDistance = 1.5f;

    [Header("RigidBody")]
    Rigidbody rb;
    public float gravity = 1f;
    public float rbDrag;
    public float airDrag;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {   
        rb = transform.GetComponent<Rigidbody>();

        targetMask = LayerMask.GetMask("Target");
        obstructionMask = LayerMask.GetMask("Obstruction");
    }

    void Update()
    {
        isGround = CheckIfGrounded();
        EnemyInteraction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlDrag();
        
        FieldOfViewCheck();

        if (canSeePlayer) { 
            Debug.Log("The player has been spotted");
            chasing = true;
        }

        if (canMove)
        {
            if (chasing)
            {
                ChasePlayer();
            }

            if (isWandering && chasing != true)
            {
                Wander();
            }

            if (followPath)
            {
                if (pathList.Count > 0 && chasing != true)
                {
                    FollowPath();
                }
            }
        }
        
    }

    public void EnemyInteraction()
    {
        interactRay = new Ray(transform.position, transform.forward);

        //Debug.DrawRay(interactRay.origin, interactRay.direction * 1.2f);
        //Physics.Raycast(interactRay, out RaycastHit hitData, 4f);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
       
        foreach (Collider collider in colliders)
        {
             if (collider.TryGetComponent(out IInteractable interactable))
             {
                interactable.Interact();
             }
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                //float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, avoidanceDistance, obstructionMask))
                {
                     //FirstCheck version for enemy over player
                    
                        canSeePlayer = true;
                    
                    
                }
                else
                {
                    // Player is outside of the enemy's field of view
                    canSeePlayer = false;

                    //Debug.Log(avoidanceDirection);
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            
        }
    }
    
    void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, playerRef.transform.position);
        Vector3 directionToPlayer = playerRef.transform.position - transform.position;

        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        MoveEnemy(playerRef.transform);

        /*Debug.Log("Distance to player: " + distance);
        Debug.Log("Angle to player: " + angleToPlayer);
        */
        
         if (distance > 15)
        {
            
            chasing = false;
            Debug.Log("Lost Track of the player. Distance: " + distance);
            GetClosestPath();
        }
    }

    void FollowPath()
    {
        currentRoute = pathList[currentRouteIndex];

        MoveEnemy(currentRoute);

        float horizontalDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(currentRoute.position.x, 0, currentRoute.position.z));

        //Debug.Log(distanceToRoute);

        if (horizontalDistance < 0.1f)
        {
            GetClosestPath();
            //currentRouteIndex = (currentRouteIndex + 1) % pathList.Count;
        }
    }

    void Wander()
    {
        // Check if it's time to change direction
        if (Time.time >= wanderElapsedTime)
        {
            // Generate a new random direction
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            // Set the new direction and reset the timer
            wanderDirection = randomDirection;
            wanderElapsedTime = Time.time + Random.Range(5f, wanderTime);
        }

        // Move the enemy in the current wander direction
        MoveEnemy(wanderDirection);
    }


    void GetClosestPath()
    {
        float distance = 0;
        float distanceY = 0;
        
        float maxDistance = 20;
        float maxDistanceY = 4;

        int closestIndex = -1;
        int sameLevelIndex = 0;

        if (currentRouteIndex != previousRouteIndex) 
        { Debug.Log("Theyre different"); }

        for (int i = 0; i < pathList.Count; i++)
        {
            if (i == previousRouteIndex)
            {
                Debug.Log($"Found same point {i}");
                continue;
            }

            if (i != currentRouteIndex && i != previousRouteIndex)
            {
                distanceY = Mathf.Abs(transform.position.y - pathList[i].position.y);

                Vector3 enemyPosition = transform.position;
                Vector3 pathPosition = pathList[i].position;

                distance = Vector3.Distance(enemyPosition, pathPosition);

                if (distanceY < 0.1f) // You can adjust this threshold as needed
                {
                    
                    sameLevelIndex = i;
                }

                if (distance < maxDistance && distanceY < maxDistanceY)
                {

                    maxDistance = distance;
                    closestIndex = i;
                }
            }
        }

        Debug.Log($"Distance: {distance}, HeightDifference: {distanceY} ");

        float closestPointDistance = Vector3.Distance(transform.position, pathList[closestIndex].position);
        float sameLevelPointDistance = Vector3.Distance(transform.position, pathList[sameLevelIndex].position);
        
        //if the enemy closest position is too far away or in this case he needs to go down, he will first past through an index that is closest to him.
        if(closestPointDistance > 14)
        {
            closestIndex = sameLevelIndex;
        }

        Debug.Log($"Comparison enemy from closest :{closestPointDistance}");
        Debug.Log($"Comparison enemy from same :{sameLevelPointDistance}");

        if (closestIndex != -1)
        {
            Debug.Log($"Closest {closestIndex} Last: {previousRouteIndex}");
            previousRouteIndex = currentRouteIndex;
            currentRouteIndex = closestIndex;
            currentRoute = pathList[currentRouteIndex];
        }
    }

    public void CheckIfAbleToJump()
    {
        if (legs != null)
        {
            // Construct the ray using the legs position and forward direction
            Ray ray = new Ray(legs.position, transform.forward);
            RaycastHit hitData;
            Debug.DrawRay(ray.origin, ray.direction * 6f, Color.red); // Draw the ray for debugging

            if (isGround) { jumpCounter = 0; }

            // Perform raycasting to detect obstacles
            if (Physics.Raycast(ray, out hitData, 6f))
            {
                float height = Mathf.Abs(hitData.transform.position.y - hitData.point.y);
                if (hitData.distance < 2.4f && height < maxObstacleHeight && jumpCounter < 1)
                {
                    //Debug.Log("Preparing Jump");
                    jumpCounter++;
                    rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                    rb.AddForce(getCurrentVelocity(), ForceMode.Force);
                }
            }
            else
            {
                // Debug.Log("Raycast hit nothing."); // Log if the ray hits nothing
            }
        }
        else
        {
            Debug.LogWarning("Legs transform is not assigned."); // Log a warning if the legs transform is not assigned
        }
    }

    bool CheckIfGrounded()
    {
        bool groundedFromSphere = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        return groundedFromSphere;
    }

    void ControlDrag()
    {
        if (isGround)
        {
            rb.drag = rbDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
    

    void MoveEnemy(Transform currentRoute)
    {
        Vector3 direction = (currentRoute.position - transform.position).normalized;
        //direction.Normalize();
        direction.y = 0;


        if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, avoidanceDistance, obstructionMask))
        {
            obstruction = false;
            

        }
        else
        {
            obstruction = true;
            avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);

            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            //float angle = Vector3.Angle(direction, hit.normal);
            Debug.Log(angle);

            direction = direction + avoidanceDirection * angle;
            
            if (direction.magnitude > 0.001f)
            {
                direction.Normalize();
            }
        }

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 5f);

        velocity = direction * enemySpeed;

        velocity.y -= gravity;

        currentVelocity = velocity;

        // Use rb.MovePosition for more accurate physics-based movement
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        CheckIfAbleToJump();
    }

    void MoveEnemy(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        Vector3 velocity;

        if (isWandering)
        {
            velocity = direction * wanderSpeed;
        }
        else
        {
            // Calculate the velocity using the direction and speed
            velocity = direction * enemySpeed;
        }
        // Apply gravity to the velocity
        

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 5f);

        Debug.Log(velocity);

        // Move the enemy using the Rigidbody's MovePosition method
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public Vector3 getCurrentVelocity() { return currentVelocity; }

    Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {

        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360f, radius);

        // Draw the field of view angle
        Vector3 viewAngle01 = DirectionFromAngle(-angle / 2, false);
        Vector3 viewAngle02 = DirectionFromAngle(angle / 2, false);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + viewAngle02 * radius);

        if (canSeePlayer)
            {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawLine(transform.transform.position, playerRef.transform.position);
            }

    }



    /*private void OnDrawGizmos()
    {
        // Visualize velocity vector
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }*/
}
