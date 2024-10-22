using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackJod : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int[] attackDamages = { 10, 15, 20 }; // Different damage values for each combo attack

    public float attackRate = 2f; // Attacks per second
    private float nextAttackTime = 0f; // Time when the next attack is allowed

    private int currentComboIndex = 0; // Current combo index
    private bool isAttacking = false; // Tracks if an attack is in progress
    private bool attackQueued = false; // Indicates if the next attack is queued

    // Reference to PlayerController for facing direction
    public PlayerController playerController;

    // Reference to the slash effect prefab
    public GameObject slashEffectPrefab;

    // Coroutine reference for forward movement
    private Coroutine forwardMovementCoroutine;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (isAttacking)
                {
                    attackQueued = true; // Queue the next attack if an attack is already in progress
                }
                else
                {
                    PerformAttack();
                }
            }
        }
    }

    void PerformAttack()
    {
        nextAttackTime = Time.time + 1f / attackRate; // Set the time for the next attack
        isAttacking = true; // Mark that an attack is in progress

        // Stop player movement while attacking
        playerController.canMove = false;

        // Play the attack animation based on the current combo index
        switch (currentComboIndex)
        {
            case 0:
                animator.SetTrigger("Attack1");
                break;
            case 1:
                animator.SetTrigger("Attack2");
                break;
            case 2:
                animator.SetTrigger("Attack3");
                break;
        }

        // Increment the combo index
        currentComboIndex = (currentComboIndex + 1) % attackDamages.Length; // Loop back to Attack1 after Attack3
    }

    // Method to be called from an animation event
    public void MoveForwardDuringAttack()
    {
        // Stop any existing forward movement coroutine to avoid conflicts
        if (forwardMovementCoroutine != null)
        {
            StopCoroutine(forwardMovementCoroutine);
        }

        // Start the forward movement coroutine
        forwardMovementCoroutine = StartCoroutine(MoveForward());
    }

    // Coroutine to move the player forward during the attack
    private IEnumerator MoveForward()
    {
        float moveDuration = 0.1f; // Duration of the forward movement
        float speed = 3f; // Adjust this value to control how fast the player moves forward

        Vector2 forwardDirection = playerController.isFacingRight ? Vector2.right : Vector2.left;

        float elapsed = 0f;

        // Move forward for the specified duration
        while (elapsed < moveDuration)
        {
            // Move the player forward in the facing direction
            playerController.transform.position += (Vector3)(forwardDirection * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    // Function to deal damage and knockback to enemies
    public void DealDamage()
    {
        // Detect enemies within the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy: " + enemy.name);

            // Apply damage to the enemy's health script
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Calculate the knockback direction based on the player's facing direction
                Vector2 knockbackDirection = playerController.isFacingRight ? Vector2.right : Vector2.left;

                // Pass both the damage and the knockback direction to the TakeDamage method
                enemyHealth.TakeDamage(attackDamages[currentComboIndex], knockbackDirection);
            }
        }
    }


    public void SpawnSlashEffect()
    {
        // Instantiate the slash effect at the attack point position with the correct rotation
        GameObject slashEffect = Instantiate(slashEffectPrefab, attackPoint.position, Quaternion.identity);

        // Optional: Destroy the slash effect after a certain duration
        Destroy(slashEffect, 0.5f); // Adjust this duration to how long you want the slash effect to last
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false; // Mark that the current attack has ended

        // Allow movement again
        playerController.canMove = true;

        // Stop the forward movement coroutine if it's still running
        if (forwardMovementCoroutine != null)
        {
            StopCoroutine(forwardMovementCoroutine);
            forwardMovementCoroutine = null;
        }

        // If an attack was queued, perform it immediately
        if (attackQueued)
        {
            PerformAttack();
            attackQueued = false; // Reset attack queue
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
