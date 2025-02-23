using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowTest : MonoBehaviour
{
    public GameObject ObjectToFollow;
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
        if (queueTimer <= 0)
        {
            queueTimer = TimeOffset / Granularity;
            if (path.Count == Granularity)
                currentPathPosition = path.Dequeue();
            path.Enqueue(ObjectToFollow.transform.position);
        }
    }

    void Flip()
    {
        // Flip the follower's scale to match the player's facing direction
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}