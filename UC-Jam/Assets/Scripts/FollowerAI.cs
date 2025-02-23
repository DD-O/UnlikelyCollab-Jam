using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followSpeed = 3f; // Speed at which the follower moves towards the player
    public float spreadDistance = 1f; // Distance the follower will spread out when the player stops
    public LayerMask groundLayer; // Ground layer mask for detecting if grounded

    public Transform groundCheckPlayerLeft; // Empty GameObject for ground check on the left side of the player
    public Transform groundCheckPlayerRight; // Empty GameObject for ground check on the right side of the player

    private Vector3 targetPosition; // The position the follower is moving toward
    private bool isGrounded; // To check if the follower is on the ground
    private bool spreadLeft = true; // Whether the follower spreads left or right based on ground checks

    void Update()
    {
        // Check if the follower is grounded
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);

        // Check the ground status on the player's left and right sides
        bool leftGrounded = Physics2D.Raycast(groundCheckPlayerLeft.position, Vector2.down, 1f, groundLayer);
        bool rightGrounded = Physics2D.Raycast(groundCheckPlayerRight.position, Vector2.down, 1f, groundLayer);

        // Set the spread direction based on the ground checks
        if (leftGrounded && rightGrounded)
        {
            spreadLeft = true; // Spread left if both sides have ground
        }
        else if (leftGrounded && !rightGrounded)
        {
            spreadLeft = true; // Spread left if only the left side has ground
        }
        else if (!leftGrounded && rightGrounded)
        {
            spreadLeft = false; // Spread right if only the right side has ground
        }
        else
        {
            spreadLeft = false; // No spread if neither side has ground
        }

        // If player is moving, follow their position
        if (player != null)
        {
            // If player is moving horizontally, move toward the player horizontally and vertically
            if (player.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0.1f)
            {
                targetPosition = player.position;
            }
            // If player isn't moving, apply horizontal spread distance while matching vertical position
            else
            {
                // Apply spread to the horizontal direction based on the spreadLeft condition
                float spreadDirection = spreadLeft ? -1f : 1f;

                // Set the target position to match the player's vertical position and apply horizontal spread
                targetPosition = new Vector3(player.position.x + spreadDirection * spreadDistance, player.position.y, transform.position.z);
            }

            // Smoothly move to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        // If the follower isn't grounded but the player is, quickly move to the player
        if (!isGrounded && player.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0.1f)
        {
            targetPosition = player.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
