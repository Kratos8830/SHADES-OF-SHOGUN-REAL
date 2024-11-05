using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelNameImageEffect : MonoBehaviour
{
    public Image levelNameImage;       // Reference to the UI Image component
    public float fadeInDuration = 1f;  // Duration of the fade-in effect
    public float stayDuration = 2f;    // Duration to keep image visible before fading out
    public float fadeOutDuration = 1f; // Duration of the fade-out effect
    public RectTransform maskTransform; // Reference to the RectMask2D mask (or Mask)

    private Vector2 originalMaskSize;

    void Start()
    {
        // Store the initial size of the mask
        if (maskTransform != null)
            originalMaskSize = maskTransform.sizeDelta;

        // Start with the image fully transparent
        SetImageAlpha(0f);

        // Start the slash fade-in effect
        StartCoroutine(SlashFadeInEffect());
    }

    IEnumerator SlashFadeInEffect()
    {
        // Slash fade-in: gradually reveal the mask size to uncover the image diagonally
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeInDuration;

            // Simulate a diagonal reveal by adjusting the mask size
            maskTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 0), originalMaskSize, progress);

            // Fade in the image alpha
            SetImageAlpha(progress);
            yield return null;
        }

        // Ensure image is fully visible
        SetImageAlpha(1f);

        // Keep the image visible for a set duration
        yield return new WaitForSeconds(stayDuration);

        // Fade-out effect
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float progress = 1 - (elapsed / fadeOutDuration);
            SetImageAlpha(progress);
            yield return null;
        }

        // Fully hide the image after fade-out
        SetImageAlpha(0f);
    }

    // Set the alpha of the image
    private void SetImageAlpha(float alpha)
    {
        Color color = levelNameImage.color;
        color.a = alpha;
        levelNameImage.color = color;
    }
}
