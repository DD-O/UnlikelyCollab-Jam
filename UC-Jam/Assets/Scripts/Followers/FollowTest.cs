using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowTest : MonoBehaviour
{
    public GameObject ObjectToFollow;  // The player object
    public GameObject HuddleTargetLEFT; // Huddle target for left side
    public GameObject HuddleTargetRIGHT; // Huddle target for right side
    public bool huddleLeft;  // Whether to huddle on the left
    public float TimeOffset; // Time offset for path updates
    public int Granularity; // How many updates per timeOffset
    public float InterpolationSpeed; // Speed of following
    public float speedReductionValue; // Speed reduction for huddling

    public float flipDelay; // Delay before flipping direction

    private Queue<Vector3> path = new Queue<Vector3>();  // Path for follower to follow
    private float queueTimer; // Timer to manage path updates
    private Vector3 currentPathPosition; // Current position in the path
    private bool isFacingRight = true; // Whether the follower is facing right
    private bool targetFacingRight; // Whether the player is facing right
    private float flipTimer; // Timer for flip delay

    private HuddleCheck groundCheckSides; // Ground checking for left/right huddling
    private PlayerController playerController; // PlayerController reference to check for grounded state
    private Rigidbody2D playerRb; // Player's Rigidbody2D to check movement

    private bool groundedL;
    private bool groundedR;
    private bool speedReduced = false; // Whether the speed reduction has been applied

    // Animator references for both player and follower
    private Animator playerAnimator;
    private Animator followerAnimator;

    // Delay parameters
    public float animationDelay = 0.2f; // Delay before follower animation starts
    public float raycastDistance = 1f; // How far to check for grounding using the raycast
    public LayerMask groundLayer; // Layer mask for the ground

    // New reference for the follower's ground check position (empty GameObject)
    public GameObject followerGroundCheck;

    // Rigidbody2D reference for the follower's own movement
    private Rigidbody2D followerRb;

    // Store the previous position for velocity calculation
    private Vector3 previousPosition;

    //Fix animmations for platform logic
    private bool onSpecialPlatform = false;

    void Start()
    {
        // Initialize references
        groundCheckSides = GetComponent<HuddleCheck>();
        playerController = ObjectToFollow.GetComponentInParent<PlayerController>(); // Get from parent
        playerRb = ObjectToFollow.GetComponentInParent<Rigidbody2D>(); // Get from parent
        playerAnimator = ObjectToFollow.GetComponentInParent<Animator>(); // Get from parent
        followerAnimator = GetComponent<Animator>(); // Follower's own animator
        followerRb = GetComponent<Rigidbody2D>(); // Follower's Rigidbody2D for movement

        // Set the previous position to the starting position
        previousPosition = transform.position;
    }

    void Update()
    {
        // Manually calculate velocity
        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        // Interpolate position
        if (currentPathPosition != Vector3.zero)
            transform.position += (currentPathPosition - transform.position) * Time.deltaTime * InterpolationSpeed;

        // Check if the ObjectToFollow (the player) has flipped
        if (ObjectToFollow != null)
        {
            bool targetIsFacingRight = ObjectToFollow.transform.lossyScale.x > 0;

            // If the player's facing direction changes, start the flip timer
            if (targetIsFacingRight != targetFacingRight)
            {
                flipTimer = flipDelay;
                targetFacingRight = targetIsFacingRight;
            }
        }

        // Handle flip delay timer
        if (flipTimer > 0)
        {
            flipTimer -= Time.deltaTime;
        }
        else if (flipTimer <= 0 && targetFacingRight != isFacingRight)
        {
            Flip();
        }

        // Sync animations with the follower's velocity
        SyncAnimations(velocity);

        Debug.Log("On special platform: " + onSpecialPlatform);

    }

    void FixedUpdate()
    {
        if (ObjectToFollow == null)
            return;

        queueTimer -= Time.fixedDeltaTime;

        bool followerIsGrounded = IsFollowerGrounded(); // Use raycast to check if follower is grounded

        if (groundCheckSides != null)
        {
            groundedL = groundCheckSides.GroundedL;
            groundedR = groundCheckSides.GroundedR;

            // Handling huddling logic
            if (groundedL && !groundedR && !IsPlayerMoving())
            {
                if (HuddleTargetLEFT != null)
                {
                    ApplySpeedReduction();
                    path.Enqueue(HuddleTargetLEFT.transform.position);
                }
            }
            else if (groundedR && !groundedL && !IsPlayerMoving())
            {
                if (HuddleTargetRIGHT != null)
                {
                    ApplySpeedReduction();
                    path.Enqueue(HuddleTargetRIGHT.transform.position);
                }
            }
            else if (groundedR && groundedL && !IsPlayerMoving())
            {
                if (huddleLeft)
                {
                    ApplySpeedReduction();
                    path.Enqueue(HuddleTargetLEFT.transform.position);
                }
                else
                {
                    ApplySpeedReduction();
                    path.Enqueue(HuddleTargetRIGHT.transform.position);
                }
            }
            else
            {
                ApplySpeedReduction();
                path.Enqueue(ObjectToFollow.transform.position);
            }
        }
        else
        {
            ApplySpeedReduction();
            path.Enqueue(ObjectToFollow.transform.position);
        }

        // Time management for path updates
        if (queueTimer <= 0)
        {
            queueTimer = TimeOffset / Granularity;
            if (path.Count == Granularity)
                currentPathPosition = path.Dequeue();
        }


    }

    private void ApplySpeedReduction()
    {
        if (!speedReduced)
        {
            InterpolationSpeed -= speedReductionValue;
            speedReduced = true;
        }
    }

    private bool IsPlayerMoving()
    {
        if (playerRb == null)
            return false;

        return playerRb.linearVelocity != Vector2.zero; // Return true if player is moving
    }

    private void Flip()
    {
        // Flip the follower's scale to match the player's facing direction
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void SyncAnimations(Vector3 velocity)
    {
        // First, check if the follower is grounded
        bool followerIsGrounded = IsFollowerGrounded();

        // Handle Running Animation
        if (followerIsGrounded)
        {

            if (Mathf.Abs(velocity.x) > 0.1f && !onSpecialPlatform)  // Significant movement
            {
                // Trigger running animation based on horizontal velocity
                followerAnimator.SetFloat("isRun_Anim", Mathf.Abs(velocity.x));
            }
            else
            {
                // Stop running animation if no movement
                followerAnimator.SetFloat("isRun_Anim", 0f);
            }

            // Handle Idle Animation (when on ground and not moving)
            if (Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f)
            {
                // Trigger idle animation if the follower is grounded but not moving
                if (!followerAnimator.GetBool("isIdle_Anim")) // Avoid redundant calls
                {
                    followerAnimator.SetBool("isIdle_Anim", true);
                }
            }
            else
            {
                // Stop idle animation if there's movement
                if (followerAnimator.GetBool("isIdle_Anim"))
                {
                    followerAnimator.SetBool("isIdle_Anim", false);
                }
            }
        }
        else
        {
            // Reset run animation if not grounded
            followerAnimator.SetFloat("isRun_Anim", 0f);
        }

        // Handle Jump Animation (based on upward velocity)
        if (!followerIsGrounded && velocity.y > 0.1f && !onSpecialPlatform)
        {
            if (!followerAnimator.GetBool("isJump_Anim")) // Avoid redundant calls
            {
                followerAnimator.SetBool("isJump_Anim", true);
            }
        }
        else
        {
            if (followerAnimator.GetBool("isJump_Anim"))
            {
                followerAnimator.SetBool("isJump_Anim", false);
            }
        }

        // Handle Falling Animation (based on downward velocity)
        if (!followerIsGrounded && velocity.y < -0.1f && !onSpecialPlatform)
        {
            if (!followerAnimator.GetBool("isFalling_Anim")) // Avoid redundant calls
            {
                followerAnimator.SetBool("isFalling_Anim", true);
            }
        }
        else
        {
            // Only stop falling animation if we're grounded or no longer falling
            if (followerAnimator.GetBool("isFalling_Anim"))
            {
                followerAnimator.SetBool("isFalling_Anim", false);
            }

            // Ensure idle animation is triggered when we land
            if (followerIsGrounded && Mathf.Abs(velocity.x) < 0.1f)
            {

                // Trigger idle animation if the follower is grounded and not moving
                if (!followerAnimator.GetBool("isIdle_Anim"))
                {
                    followerAnimator.SetBool("isIdle_Anim", true);
                }
            }
        }
    }



    // Function to check if the follower is grounded using raycast
    private bool IsFollowerGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(followerGroundCheck.transform.position, Vector2.down, raycastDistance, groundLayer);
        return hit.collider != null; // Returns true if the raycast hits something on the ground layer
    }

    // Detect when the follower enters the special platform
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpecialPlatform"))
        {
            onSpecialPlatform = true;
        }
    }

    // Detect when the follower exits the special platform
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SpecialPlatform"))
        {
            onSpecialPlatform = false;
        }
    }





}
