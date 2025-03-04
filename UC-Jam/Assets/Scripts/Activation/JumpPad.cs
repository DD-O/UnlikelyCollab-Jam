using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounciness = 20f;
    private float bounceCooldown = 0.1f; // Short cooldown to prevent multiple bounces
    private float lastBounceTime = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float currentTime = Time.time;

                // Check if enough time has passed since the last bounce
                if (currentTime - lastBounceTime > bounceCooldown)
                {
                    Debug.Log("BOUNCE");

                    // Reset vertical velocity to prevent stacking forces
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                    // Apply bounce force
                    rb.AddForce(Vector2.up * bounciness, ForceMode2D.Impulse);

                    // Update bounce time
                    lastBounceTime = currentTime;
                }
            }
        }
    }
}
