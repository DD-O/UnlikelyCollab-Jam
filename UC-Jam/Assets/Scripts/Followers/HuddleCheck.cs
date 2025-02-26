using UnityEngine;

public class HuddleCheck : MonoBehaviour
{
    public Transform pointL;
    public Transform pointR;
    public float raycastDistance = 1.0f;
    public LayerMask groundLayer;

    private bool groundedL;
    private bool groundedR;

    public bool GroundedL => groundedL;
    public bool GroundedR => groundedR;

    void FixedUpdate()
    {
        // Perform 2D raycasts and store results
        groundedL = Physics2D.Raycast(pointL.position, Vector2.down, raycastDistance, groundLayer);
        groundedR = Physics2D.Raycast(pointR.position, Vector2.down, raycastDistance, groundLayer);

        // Debugging: Visualize the rays in Scene View
        //Debug.DrawRay(pointL.position, Vector2.down * raycastDistance, groundedL ? Color.green : Color.red);
        //Debug.DrawRay(pointR.position, Vector2.down * raycastDistance, groundedR ? Color.green : Color.red);

    }
}
