using System.Collections.Generic;
using UnityEngine;

public class FollowTest : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public GameObject HuddleTargetLEFT;
    public GameObject HuddleTargetRIGHT;
    public bool huddleLeft;
    public float TimeOffset;
    public int Granularity;

    public float InterpolationSpeed;
    public float speedReductionValue;

    public float flipDelay; // Delay in seconds before flipping (customizable)

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
    private bool speedReduced = false;

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

        bool playerisGrounded = playerController.isGrounded;

        if (groundCheckSides != null)
        {
            groundedL = groundCheckSides.GroundedL;
            groundedR = groundCheckSides.GroundedR;

            // If room on left but not right, follow HuddleTargetLEFT
            if (groundedL && !groundedR && playerisGrounded)
            {
                if (HuddleTargetLEFT != null)
                {
                    ApplySpeedReduction();  // Refactored method
                    path.Enqueue(HuddleTargetLEFT.transform.position);
                }
            }
            // If room on right but not left, follow HuddleTargetRIGHT
            else if (groundedR && !groundedL && playerisGrounded)
            {
                if (HuddleTargetRIGHT != null)
                {
                    ApplySpeedReduction();  // Refactored method
                    path.Enqueue(HuddleTargetRIGHT.transform.position);
                }
            }
            // If there is room on both sides
            else if (groundedR && groundedL)
            {
                if (huddleLeft)
                {
                    ApplySpeedReduction();  // Refactored method
                    path.Enqueue(HuddleTargetLEFT.transform.position);
                }
                else
                {
                    ApplySpeedReduction();  // Refactored method
                    path.Enqueue(HuddleTargetRIGHT.transform.position);
                }
            }
            else
            {
                ApplySpeedReduction();  // Refactored method
                path.Enqueue(ObjectToFollow.transform.position);
            }
        }
        else
        {
            ApplySpeedReduction();  // Refactored method
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


    void Flip()
    {
        // Flip the follower's scale to match the player's facing direction
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
