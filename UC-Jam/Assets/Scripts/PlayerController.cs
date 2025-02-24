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

    private Rigidbody2D rb;
    private bool facingRight = true;
    public bool isGrounded;

    public Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if player is on the ground
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = Vector2.up * jumpVelocity;
            animator.SetBool("isJump_Anim", true); // Play jump animation
            animator.SetBool("isFalling_Anim", false);
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
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
