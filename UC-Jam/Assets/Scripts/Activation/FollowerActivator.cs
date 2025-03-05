using UnityEngine;
using System.Collections;  // For using Coroutines

public class FollowerActivator : MonoBehaviour
{
    public GameObject follower; // Assign the object to activate in the Inspector
    public GroupBox groupBox; // JJ Script Reference to Add Follower to the Group Perceptive Box
    public float delayTime = 2f; // Time to wait before activating the follower

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the "Player" tag
        {
            if (follower != null)
            {
                // Start the coroutine to wait before performing actions
                StartCoroutine(ActivateFollowerAfterDelay());
            }

            
        }
    }

    // Coroutine to wait for the delay before executing the rest of the code
    private IEnumerator ActivateFollowerAfterDelay()
    {
        yield return new WaitForSeconds(delayTime); // Wait for the specified delay time

        follower.SetActive(true); // Activate the follower
        MusicManager.Instance.TriggerNextSong();
        groupBox.followerGroup.Add(follower); // Add Follower to Group Box
        if (groupBox.followerGroup.Count > 1) // If there's more than just Square in the Group...
        {
            groupBox.padding = 5; // Give the Group Perception Box Padding!
        }
        Destroy(gameObject); // Destroy this object
    }
}
