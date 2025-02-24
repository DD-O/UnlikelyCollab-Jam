using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Variable to store the follower's previous position
    private Vector3 previousPosition;

    void Start()
    {
        // Initialize references
        groundCheckSides = GetComponent<HuddleCheck>();
        playerController = ObjectToFollow.GetComponent<PlayerController>();
        playerRb = ObjectToFollow.GetComponent<Rigidbody2D>();
        playerAnimator = ObjectToFollow.GetComponent<Animator>();
        followerAnimator = GetComponent<Animator>();

        // Initialize previous position
        previousPosition = transform.position;
    }

    void Update()
    {
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

        // Calculate the follower's movement by comparing positions
        Vector3 positionDelta = transform.position - previousPosition;

        // Update the previous position for the next frame
        previousPosition = transform.position;

        // Sync animations with the calculated movement
        SyncAnimations(positionDelta);
    }

    void FixedUpdate()
    {
        if (ObjectToFollow == null)
            return;

        queueTimer -= Time.fixedDeltaTime;

        bool followerIsGrounded = IsFollowerGrounded(); // Use raycast to check if player is grounded

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

    private void SyncAnimations(Vector3 positionDelta)
    {
        // Syncing the Jumping/Falling animations with a delay
        bool playerIsJumping = playerAnimator.GetBool("isJump_Anim");
        bool playerIsFalling = playerAnimator.GetBool("isFalling_Anim");
        bool playerIsGrounded = playerController.isGrounded;
        bool followerIsGrounded = IsFollowerGrounded();

        // Syncing the Running Animation, but only if the follower is grounded
        if (followerIsGrounded)
        {
            // Check if the horizontal velocity is significantly greater than 0.01 or less than -0.01
            if (Mathf.Abs(playerRb.linearVelocity.x) > 0.01f)
            {
                followerAnimator.SetFloat("isRun_Anim", Mathf.Abs(playerRb.linearVelocity.x)); // Pass speed to follower
            }
            else
            {
                followerAnimator.SetFloat("isRun_Anim", 0f); // Stop running animation if no significant movement
            }
        }
        else
        {
            followerAnimator.SetFloat("isRun_Anim", 0f); // Reset run animation if not grounded
        }

        // Handle Jumping animation with delay
        if (playerIsJumping && !playerIsGrounded)
        {
            StartCoroutine(TriggerAnimationWithDelay("isJump_Anim", animationDelay));
        }
        else if (!playerIsJumping && playerIsGrounded)
        {
            followerAnimator.SetBool("isJump_Anim", false); // Reset jump animation
        }

        // Handle falling animation with delay
        if (playerIsFalling && !playerIsGrounded)
        {
            StartCoroutine(TriggerAnimationWithDelay("isFalling_Anim", animationDelay));
        }
        else if (!playerIsFalling && playerIsGrounded)
        {
            followerAnimator.SetBool("isFalling_Anim", false); // Reset falling animation
        }

        // Follower idle animation when grounded and not moving horizontally
        if (followerIsGrounded && Mathf.Abs(positionDelta.x) < 0.1f) // Use positionDelta.x instead of velocity
        {
            followerAnimator.SetBool("isIdle_Anim", true);  // Call idle animation
        }
        else
        {
            followerAnimator.SetBool("isIdle_Anim", false); // Reset idle animation when moving
        }
    }


    // Coroutine to delay triggering an animation
    private IEnumerator TriggerAnimationWithDelay(string animName, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay
        followerAnimator.SetBool(animName, true);
    }

    // Function to check if the player is grounded using raycast
    private bool IsFollowerGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(followerGroundCheck.transform.position, Vector2.down, raycastDistance, groundLayer);
        return hit.collider != null; // Returns true if the raycast hits something on the ground layer
    }
}
