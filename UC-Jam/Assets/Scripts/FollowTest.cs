using System.Collections.Generic;
using UnityEngine;

public class FollowTest : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public GameObject HuddleTargetLEFT;
    public GameObject HuddleTargetRIGHT;
    public bool huddleLeft;
    public float TimeOffset = 1;
    public int Granularity = 10;

    public float InterpolationSpeed = 10;

    public float flipDelay = 0.5f; // Delay in seconds before flipping (customizable)

    private Queue<Vector3> path = new Queue<Vector3>();
    private float queueTimer;

    private Vector3 currentPathPosition;

    private bool isFacingRight = true; // Track the facing direction of the follower
    private bool targetFacingRight; // Track the target's facing direction
    private float flipTimer; // Timer for the flip delay

    private HuddleCheck groundCheckSides; // Reference to HuddleCheck script on the same GameObject

    // Reference to the PlayerController script to access isGrounded
    private PlayerController playerController;

    // Cached grounded values for optimization
    private bool groundedL;
    private bool groundedR;

    protected void Start()
    {
        // Cache reference to HuddleCheck and PlayerController
        groundCheckSides = GetComponent<HuddleCheck>();
        playerController = ObjectToFollow.GetComponent<PlayerController>();
    }

    protected void Update()
    {
        if (currentPathPosition != Vector3.zero)
            transform.position += (currentPathPosition - transform.position) * Time.deltaTime * InterpolationSpeed;

        // Check if the ObjectToFollow (the player) has flipped
        if (ObjectToFollow != null)
        {
            // Use worldScale to check the player's facing direction
            bool targetIsFacingRight = ObjectToFollow.transform.lossyScale.x > 0;

            // If the target's facing direction changes, start the flip timer
            if (targetIsFacingRight != targetFacingRight)
            {
                flipTimer = flipDelay; // Reset the timer to the flip delay
                targetFacingRight = targetIsFacingRight;
            }
        }

        // If the flip timer is active, count it down
        if (flipTimer > 0)
        {
            flipTimer -= Time.deltaTime; // Decrease timer by elapsed time
        }
        else if (flipTimer <= 0 && targetFacingRight != isFacingRight)
        {
            Flip(); // Flip the follower after the delay
        }


    }

    protected void FixedUpdate()
    {
        if (ObjectToFollow == null)
            return;

        queueTimer -= Time.fixedDeltaTime;

        // Check if the player is grounded using the isGrounded value from PlayerController
        bool playerisGrounded = playerController.isGrounded;

        // Only check if groundCheckSides is assigned and GroundedL or GroundedR are needed
        if (groundCheckSides != null)
        {
            // Cache the grounded values
            groundedL = groundCheckSides.GroundedL;
            groundedR = groundCheckSides.GroundedR;

            // If room on left but not right, follow HuddleTargetLEFT
            if (groundedL && !groundedR && playerisGrounded)
            {
                if (HuddleTargetLEFT != null) // Make sure HuddleTargetLEFT is assigned
                {
                    // Update path to follow HuddleTargetLEFT's position
                    path.Enqueue(HuddleTargetLEFT.transform.position);
                }
            }
            // If room on right but not left, follow HuddleTargetRIGHT
            else if (groundedR && !groundedL && playerisGrounded)
            {
                if (HuddleTargetRIGHT != null) // Make sure HuddleTargetRIGHT is assigned
                {
                    // Update path to follow HuddleTargetRIGHT's position
                    path.Enqueue(HuddleTargetRIGHT.transform.position);
                }
            }
            // If there is room on both sides
            else if (groundedR && groundedL)
            {
                if (huddleLeft) { path.Enqueue(HuddleTargetLEFT.transform.position); } //if huddleLeft is true go left
                else { path.Enqueue(HuddleTargetRIGHT.transform.position); } // or go right if false
            }
            else // If no room, continue stacking on player
            {
                path.Enqueue(ObjectToFollow.transform.position);
            }
        }
        else
        {
            // If groundCheckSides is not assigned, default to following ObjectToFollow
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

    void Flip()
    {
        // Flip the follower's scale to match the player's facing direction
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
