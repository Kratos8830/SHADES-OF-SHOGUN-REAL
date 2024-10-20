using UnityEngine;
using TMPro;

public class TipController : MonoBehaviour
{
    public TextMeshProUGUI tipText;      // Reference to the TextMeshPro object (for tips)
    public SpriteRenderer objectToFade;  // Reference to the object that will fade (e.g., Sprite)
    public Transform player;             // Reference to the player object
    public float detectionRadius = 5f;   // Radius to detect the player
    public float fadeSpeed = 2f;         // Speed of the fade effect

    private bool playerInRange = false;  // To track if the player is in range
    private Color originalTipColor;      // Store the original text color
    private Color originalObjectColor;   // Store the original object color (for Sprite)

    void Start()
    {
        // Get and store the original colors
        if (tipText != null)
            originalTipColor = tipText.color;

        if (objectToFade != null)
            originalObjectColor = objectToFade.color;

        // Initially set both the text and object to fully transparent (hidden)
        SetOpacity(0f);
    }

    void Update()
    {
        // Calculate the distance between the player and the tip/object
        float distance = Vector2.Distance(player.position, transform.position);

        // Check if the player is within the detection radius
        if (distance <= detectionRadius)
        {
            // Player is close enough, fade in both the tip and the object
            playerInRange = true;
        }
        else
        {
            // Player is too far, fade out both the tip and the object
            playerInRange = false;
        }

        // Perform the fade effect for both the text and the object
        FadeElements();
    }

    // Function to fade both the TextMeshPro text and the sprite object
    void FadeElements()
    {
        float targetAlpha = playerInRange ? 1f : 0f;  // Full opacity if in range, else transparent

        // Fade the TextMeshPro text
        if (tipText != null)
        {
            Color color = tipText.color;
            color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
            tipText.color = color;
        }

        // Fade the sprite object
        if (objectToFade != null)
        {
            Color color = objectToFade.color;
            color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
            objectToFade.color = color;
        }
    }

    // Function to set the opacity for both the text and object instantly (used in Start)
    void SetOpacity(float alpha)
    {
        // Set opacity for the TextMeshPro text
        if (tipText != null)
        {
            Color color = tipText.color;
            color.a = alpha;
            tipText.color = color;
        }

        // Set opacity for the sprite object
        if (objectToFade != null)
        {
            Color color = objectToFade.color;
            color.a = alpha;
            objectToFade.color = color;
        }
    }
}
