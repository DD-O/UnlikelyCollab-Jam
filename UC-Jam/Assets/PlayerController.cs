using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public KeyCode toggleKey = KeyCode.E;
    private Rigidbody2D rb;
    private bool isInCreepyMode = false;
    public GameObject dimensionBox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MovePlayer();
        ToggleDimensionBox();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    private void ToggleDimensionBox()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isInCreepyMode = !isInCreepyMode;
            dimensionBox.SetActive(isInCreepyMode);
        }
    }
}
