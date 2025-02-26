using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows
    public Vector3 offset;  // Offset of the camera from the player

    private Vector3 velocity = Vector3.zero; // Used for smooth damp

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate target position
            Vector3 targetPosition = player.position + offset;

            // Ensure the camera stays at z = -10 (for 2D view)
            targetPosition.z = -10;

            // Smoothly move the camera towards the desired position using SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
        }
    }
}
