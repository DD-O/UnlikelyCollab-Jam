using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowExperiment : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public float followSpeed = 5f;
    public float gravityScale = 2f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator followerAnimator;
    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float interpolationSpeed = 10f; // How smoothly the follower interpolates to the player's position
    public float granularity = 0.1f; // How often we record positions (time interval)
    private Queue<Vector3> recordedPositions = new Queue<Vector3>(); // Stores recorded player positions
    private float timer = 0f; // Timer to track the granularity for recording

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        followerAnimator = GetComponent<Animator>();

        // Ensure follower ignores collisions with the player
        Collider2D playerCollider = ObjectToFollow.GetComponent<Collider2D>();
        Collider2D followerCollider = GetComponent<Collider2D>();
        if (playerCollider != null && followerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, followerCollider);
        }

        // Dynamically assign ObjectToFollow if it's not set
        if (ObjectToFollow == null)
        {
            ObjectToFollow = transform.parent.gameObject; // Assuming the follower's parent is the player
        }
    }

    void Update()
    {
        FollowTarget();
        UpdateAnimations();
    }

    void FollowTarget()
    {
        if (ObjectToFollow == null) return;

        // Update the timer with the time that has passed since the last frame
        timer += Time.deltaTime;

        // Record the player's position at intervals defined by the granularity
        if (timer >= granularity)
        {
            recordedPositions.Enqueue(ObjectToFollow.transform.position); // Record current player position
            timer = 0f; // Reset the timer
        }

        // Keep the recorded positions queue from growing indefinitely
        if (recordedPositions.Count > Mathf.RoundToInt(1f / granularity) * 10) // Limit the history length (~10 seconds)
        {
            recordedPositions.Dequeue(); // Remove the oldest position
        }

        // If there are recorded positions, move the follower towards the first recorded position
        if (recordedPositions.Count > 0)
        {
            Vector3 targetPosition = recordedPositions.Peek(); // Get the first recorded position
            transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationSpeed * Time.deltaTime); // Interpolate movement

            // Remove the position from the queue if the follower is close enough to it
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                recordedPositions.Dequeue();
            }
        }
    }

    void UpdateAnimations()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Update the Animator parameters based on conditions
        followerAnimator.SetBool("isGrounded", isGrounded);
        followerAnimator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
        followerAnimator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y > 0.1f);
        followerAnimator.SetBool("isFalling", !isGrounded && rb.linearVelocity.y < -0.1f);
        followerAnimator.SetBool("isIdle", isGrounded && Mathf.Abs(rb.linearVelocity.x) < 0.1f);
    }
}
