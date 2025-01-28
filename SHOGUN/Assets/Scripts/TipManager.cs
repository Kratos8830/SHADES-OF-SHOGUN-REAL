using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    public Transform player;               // Reference to the player transform
    public Transform regionCenter;         // Center of the region where the tip is displayed
    public CanvasGroup tipCanvasGroup;     // CanvasGroup containing both text and image
    public float fadeSpeed = 2f;           // Speed of fading in and out
    public float range = 5f;               // Range within which the tip is visible

    private void Start()
    {
        // Ensure the CanvasGroup's alpha is initially set to 0 (invisible)
        tipCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        // Calculate the distance between the player and the region center
        float distance = Vector3.Distance(player.position, regionCenter.position);

        // Check if the player is within range
        if (distance <= range)
        {
            // Fade in the tip
            tipCanvasGroup.alpha = Mathf.MoveTowards(tipCanvasGroup.alpha, 1, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Fade out the tip
            tipCanvasGroup.alpha = Mathf.MoveTowards(tipCanvasGroup.alpha, 0, fadeSpeed * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(regionCenter.position, range);
        Gizmos.color = Color.red;
    }
}
