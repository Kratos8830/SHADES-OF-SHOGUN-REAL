using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject myEnemy;
    public Rigidbody2D rb;
    public Animator animator;

    public float knockbackDuration = 0.2f; // Duration of the knockback
    public float knockbackForce = 5f; // Adjust this value to control knockback strength

    public GameObject bloodParticlePrefab; // Reference to the blood particle prefab

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        //animator.SetTrigger("hurt");
        Debug.Log("Hurting enemy: " + currentHealth);

        // Instantiate blood particles when hurt
        if (bloodParticlePrefab != null)
        {
            Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
        }

        // Start knockback coroutine
        StartCoroutine(ApplyKnockback(knockbackDirection));

        if (currentHealth <= 0)
        {
            animator.SetTrigger("dead");
            animator.SetBool("isDead", true);
            Die();
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        float elapsed = 0f;

        // Store original velocity for the enemy
        Vector2 originalVelocity = rb.velocity;

        while (elapsed < knockbackDuration)
        {
            // Apply knockback force in the specified direction
            rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, originalVelocity.y);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset the velocity after the knockback
        rb.velocity = new Vector2(originalVelocity.x, originalVelocity.y);
    }

    public void Die()
    {
        // Instantiate blood particles on death
        if (bloodParticlePrefab != null)
        {
            Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
        }

        // Play death animation, particle effects, etc.
        Debug.Log("Enemy died!");

        // Disable enemy components and collider
        myEnemy.GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<EnemyHealth>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        Destroy(rb);
        this.enabled = false; // Disable this component
    }
}
