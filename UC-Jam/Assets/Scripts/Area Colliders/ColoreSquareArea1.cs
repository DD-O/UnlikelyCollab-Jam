using UnityEngine;

public class ColorSquareArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.squareColorArea = true;
        }
    }

}
