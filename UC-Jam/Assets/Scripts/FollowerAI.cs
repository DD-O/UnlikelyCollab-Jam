using System.Collections.Generic;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followDelay = 0.5f; // Delay time in seconds
    public float followDistance = 1.5f; // Minimum distance to keep from the player
    public float moveSpeed = 5f; // Movement speed of the follower

    private Queue<Vector2> positionHistory = new Queue<Vector2>();
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("RecordPosition", 0f, followDelay / 10f); // Record positions at a steady rate
    }

    void Update()
    {
        if (positionHistory.Count > 0)
        {
            Vector2 targetPosition = positionHistory.Peek();
            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance > followDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                UpdateSpriteDirection(targetPosition);
            }

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                positionHistory.Dequeue(); // Remove old positions once reached
            }
        }
    }

    void RecordPosition()
    {
        if (player != null)
        {
            positionHistory.Enqueue(player.position);
        }
    }

    void UpdateSpriteDirection(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }
}
