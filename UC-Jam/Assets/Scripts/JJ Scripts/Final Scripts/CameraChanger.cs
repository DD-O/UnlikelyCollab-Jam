using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public Camera mainCamera;
    public JJCamera scriptCamera;

    public float previousSize;
    public Vector3 previousOffset;

    public float targetSize;
    public Vector3 targetOffset;

    public float changeSpeed;

    public float changeTime = 0.0f;
    public bool beginChange = false;

    void FixedUpdate()
    {
        if (beginChange) {
            // Change Size
            mainCamera.orthographicSize = Mathf.Lerp(previousSize, targetSize, changeTime);
            
            // Change Offset
            scriptCamera.offset = Vector3.Lerp(previousOffset, targetOffset, changeTime);

            changeTime += changeSpeed * Time.deltaTime;
        }

        if (changeTime > 1.0f) {
            beginChange = false;
            changeTime = 0.0f;
        }
    }
}
