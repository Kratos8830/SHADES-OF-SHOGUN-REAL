using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health Settings")]
    public int maxHealth = 100; // Maximum health of the boss
    private int currentHealth;

    [Header("Effects")]
    public GameObject deathEffect; // Effect to spawn when boss dies
    public GameObject hitEffect; // Effect to spawn when boss takes damage

    [Header("Animations")]
    private Animator animator; // Animator for boss animations

    [Header("Sound Effects")]
    public AudioClip hitSound; // Sound for taking damage
    public AudioClip deathSound; // Sound for death
    private AudioSource audioSource;

    [Header("healthBar")]
    public Image healthBar;

    private bool isStunned = false; // Indicates if the boss is stunned

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        animator = GetComponent<Animator>(); // Reference to Animator
        audioSource = GetComponent<AudioSource>(); // Reference to AudioSource
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0 || isStunned) return; // Prevent further actions if boss is already dead or stunned

        currentHealth -= damage; // Reduce health
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        Debug.Log($"Boss took {damage} damage. Current health: {currentHealth}");

        // Play hit effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Trigger hurt animation
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // Check if health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Stun(float duration)
    {
        if (isStunned) return; // Avoid repeated stuns

        isStunned = true;

        // Trigger stun animation if available
        if (animator != null)
        {
            animator.SetTrigger("Stun");
        }

        // Start the stun recovery process
        Invoke(nameof(RecoverFromStun), duration);
    }

    private void RecoverFromStun()
    {
        isStunned = false;

        // Trigger recovery animation if available
        if (animator != null)
        {
            animator.SetTrigger("Recover");
        }
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);

        // Play death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable the boss after a short delay to allow the animation to play
        Destroy(gameObject, 2f);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
