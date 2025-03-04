using UnityEngine;

public class FollowerActivator : MonoBehaviour
{
    public GameObject follower; // Assign the object to activate in the Inspector
    public GroupBox groupBox; // JJ Script Reference to Add Follower to the Group Perceptive Box

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the "Player" tag
        {
            if (follower != null)
            {
                follower.SetActive(true); // Activate the follower
                MusicManager.Instance.TriggerNextSong();
                groupBox.followerGroup.Add(follower); // JJ Add Follower to Group Box
                if (groupBox.followerGroup.Count > 1) { // JJ If there's more than just Square in the Group...
                    groupBox.padding = 5; // JJ ...Then Give the Group Perception Box Padding!
                }

            }
            Destroy(gameObject); // Destroy this object
        }
    }
}
