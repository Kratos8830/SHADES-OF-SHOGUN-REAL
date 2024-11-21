
using System.Collections;
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

    private EnemyAI enemyAI; // Reference to EnemyAI script
    public ArrowEnemy arrowenemy;
    public bool ismyEnemyDied = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Get reference to EnemyAI component
        enemyAI = GetComponent<EnemyAI>();

    }



    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");
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
            Die();
        }
    }

    public IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        float elapsed = 0f;

        animator.SetTrigger("hurt");
        myEnemy.GetComponent<ArrowEnemy>().enabled = false;
        StartCoroutine(isHitOff());

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
        // Set the "isDead" animator flag and trigger the "dead" animation
        animator.SetTrigger("dead");
        animator.SetBool("isDead", true);

        // Instantiate blood particles on death
        if (bloodParticlePrefab != null)
        {
            Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
        }

        // Disable enemy components and collider
        if (myEnemy != null)
        {
            myEnemy.GetComponent<Collider2D>().enabled = false;

        }

        // Set isDead in EnemyAI and disable it
        if (enemyAI != null)
        {
            enemyAI.isDead = true;
            enemyAI.enabled = false; // Disable the EnemyAI script
        }

        // Start coroutine to wait for the animation to finish, then destroy the GameObject
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the enemy GameObject
        Destroy(myEnemy);
        ismyEnemyDied = true;
    }

    IEnumerator isHitOff()
    {
        yield return new WaitForSeconds(2.5f);
        myEnemy.GetComponent<ArrowEnemy>().enabled = true;
    }

}
