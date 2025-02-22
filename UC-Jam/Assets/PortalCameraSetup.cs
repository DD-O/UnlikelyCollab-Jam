using UnityEngine;

public class PortalCameraSetup : MonoBehaviour
{
    public Camera portalCamera;
    public GameObject dimensionBox;

    void Update()
    {
        // Position the portal camera to match the dimension box's position
        portalCamera.transform.position = dimensionBox.transform.position;
        portalCamera.transform.rotation = dimensionBox.transform.rotation;

        // Adjust the camera's viewport so that it only renders inside the box.
        // Here we set the camera's frustum or viewport rectangle to match the dimension box.

        Vector3 boxMin = dimensionBox.GetComponent<Collider>().bounds.min;
        Vector3 boxMax = dimensionBox.GetComponent<Collider>().bounds.max;

        // Convert the world space bounding box to a camera-local space
        Vector3 boxSize = boxMax - boxMin;

        // Define the camera's frustum or viewport rectangle
        portalCamera.rect = new Rect(boxMin.x / boxSize.x, boxMin.y / boxSize.y, boxSize.x, boxSize.y);
    }
}
