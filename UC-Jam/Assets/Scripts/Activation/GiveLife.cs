using UnityEngine;
using System.Collections;

public class GiveLife : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float colorChangeSpeed = 5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();

        // Store the original color from the sprite
        originalColor = spriteRenderer.color;

        // Set initial color (no change in the shader yet, will be handled by the script)
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", new Color(0f, originalColor.g, originalColor.b, originalColor.a)); // Start at original color
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }

    private void Start()
    {
        StartCoroutine(FadeToGray(colorChangeSpeed)); // Duration of the fade from original color to gray
    }

    private IEnumerator FadeToGray(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            float newDesaturation = Mathf.Lerp(0f, 1f, t); // Interpolating from original color to grayscale

            // Update the red channel (_Color.r) to control the transition
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", new Color(newDesaturation, originalColor.g, originalColor.b, originalColor.a));
            spriteRenderer.SetPropertyBlock(propertyBlock);

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure fully gray at the end
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", new Color(1f, originalColor.g, originalColor.b, originalColor.a)); // Fully gray
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
