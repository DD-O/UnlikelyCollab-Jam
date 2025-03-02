using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private Camera mainCamera;
    private JJCamera scriptCamera;
    private CameraChanger cameraChanger;

    public float targetSize;
    public Vector3 targetOffset;

    public float changeSpeed;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false; // Disable Debug Texture
        mainCamera = GetComponentInParent<CameraChanger>().mainCamera;
        scriptCamera = GetComponentInParent<CameraChanger>().scriptCamera;
        cameraChanger = GetComponentInParent<CameraChanger>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if (cameraChanger.beginChange == false) {
            cameraChanger.beginChange = true;
            cameraChanger.changeTime = 0.0f;

            cameraChanger.previousSize = mainCamera.orthographicSize;
            cameraChanger.previousOffset = scriptCamera.offset;

            cameraChanger.targetSize = targetSize;
            cameraChanger.targetOffset = targetOffset;
            cameraChanger.changeSpeed = changeSpeed;
        //}
    }
}
