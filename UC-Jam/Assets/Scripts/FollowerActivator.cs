using UnityEngine;

public class FollowerActivator : MonoBehaviour
{
    public GameObject follower; // Assign the object to activate in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the "Player" tag
        {
            if (follower != null)
            {
                follower.SetActive(true); // Activate the follower
            }
            Destroy(gameObject); // Destroy this object
        }
    }
}
