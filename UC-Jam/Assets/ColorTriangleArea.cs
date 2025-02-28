using UnityEngine;

public class ColorTriangleArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.triangleColorArea = true;
        }
    }

}
