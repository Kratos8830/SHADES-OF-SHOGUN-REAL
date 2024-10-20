using UnityEngine;

public class TreeOpacityController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public SpriteRenderer treeRenderer;  // Reference to the tree's SpriteRenderer
    public float minDistance = 2.0f;  // Distance at which opacity is minimum
    public float maxDistance = 5.0f;  // Distance at which opacity is maximum
    public float minOpacity = 0.5f;  // Minimum opacity of the tree

    void Update()
    {
        // Calculate the horizontal (x-axis) distance between the player and the tree
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);

        // Calculate the opacity based on the distance
        float opacity = Mathf.InverseLerp(minDistance, maxDistance, horizontalDistance);
        opacity = Mathf.Clamp(opacity, minOpacity, 1.0f);  // Ensure opacity is within the min and max range

        // Apply the opacity to the tree's SpriteRenderer
        Color color = treeRenderer.color;
        color.a = opacity;
        treeRenderer.color = color;
    }
}
