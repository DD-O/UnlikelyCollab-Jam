using UnityEngine;

public class ColorStarArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.starColorArea = true;
        }
    }

}
