using UnityEngine;
using System.Collections.Generic;

public class TriggerEnding : MonoBehaviour
{
    public GameObject endCutscene;
    public List<GameObject> followers;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.SetActive(false); // Deactivate Player
            endCutscene.SetActive(true);

            // Deactivate each object in the followers list without a loop
            if (followers.Count > 0) followers[0].SetActive(false); // Deactivate first follower
            if (followers.Count > 1) followers[1].SetActive(false); // Deactivate second follower
            if (followers.Count > 2) followers[2].SetActive(false); // Deactivate third follower
            if (followers.Count > 3) followers[3].SetActive(false); // Deactivate fourth follower
        }
    }
}
