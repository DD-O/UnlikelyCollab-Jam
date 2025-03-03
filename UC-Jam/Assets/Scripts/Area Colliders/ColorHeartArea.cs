using UnityEngine;

public class ColorHeartArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.heartColorArea = true;
        }
    }

}
