using System.Collections.Generic;
using UnityEngine;

public class TriggerEnding : MonoBehaviour
{
    public GameObject endCutscene;
    public List<GameObject> followers;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.SetActive(false); // Deactivate Player
            endCutscene.SetActive(true);
        }
    }
}
