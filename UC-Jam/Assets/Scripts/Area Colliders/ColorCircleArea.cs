using UnityEngine;

public class ColorCircleArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.circleColorArea = true;
        }
    }

}
