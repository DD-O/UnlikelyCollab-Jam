using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public float coyoteTime = 0.2f;  // Duration of coyote time (time after leaving the ground to still jump)

    private Rigidbody2D rb;
    private bool facingRight = true;
    public bool isGrounded;
    private float coyoteTimeCounter;  // Timer for coyote time

    public Animator animator;

    // Audio
    private AudioSource sfxSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sfxSource = transform.Find("SFX").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if player is on the ground
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // If grounded, reset coyote time counter
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            // Otherwise, start counting down coyote time if the player is in the air
            if (!wasGrounded)  // Only decrement the counter when we leave the ground
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }

        // Handle movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Running Animation
        animator.SetFloat("isRun_Anim", Mathf.Abs(moveInput));

        // Handle flipping
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Handle jumping with coyote time
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f)  // Allow jumping if grounded or within coyote time
            {
                rb.linearVelocity = Vector2.up * jumpVelocity;
                animator.SetBool("isJump_Anim", true);  // Play jump animation
                animator.SetBool("isFalling_Anim", false);

                // Jump SFX
                SoundManager.Instance.PlaySound("Jump", sfxSource);
            }
        }

        // Handle jumping physics for better jump feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Handle animations for jumping & falling
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0) // Moving up
            {
                animator.SetBool("isJump_Anim", true);
                animator.SetBool("isFalling_Anim", false);
            }
            else if (rb.linearVelocity.y < 0) // Moving down
            {
                animator.SetBool("isJump_Anim", false);
                animator.SetBool("isFalling_Anim", true);
            }
        }
        else // When grounded, reset animations
        {
            animator.SetBool("isJump_Anim", false);
            animator.SetBool("isFalling_Anim", false);

            // Check if the player just landed (was in the air and now is grounded)
            if (!wasGrounded && isGrounded)
            {
                // Play landing sound effect
                //SoundManager.Instance.PlaySound("Landing", sfxSource);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
