using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.0f);  // Adjust the x and y speed to scroll horizontally or vertically
    private Material material;
    private Vector2 offset;

    void Start()
    {
        // Get the Material component of the sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    void Update()
    {
        // Calculate the new offset based on the scroll speed and time
        offset += scrollSpeed * Time.deltaTime;

        // Apply the offset to the material’s "_MainTex" texture
        if (material != null)
        {
            material.mainTextureOffset = offset;
        }
    }
}
