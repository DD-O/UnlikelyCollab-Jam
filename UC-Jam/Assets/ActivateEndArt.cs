using UnityEngine;

public class ActivateEndArt : MonoBehaviour
{
    public GameObject objectToActivate; // Assign in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the Player enters the collider
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true); // Activate the referenced GameObject
            }
        }
    }
}
