using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttack : MonoBehaviour
{
    public float attackCooldown = 0.5f;          // Cooldown between individual attacks
    public float comboResetTime = 1.5f;          // Time after which the combo will reset if no further clicks
    public float moveForwardAmount = 2f;         // Amount of force applied during the attack
    public int maxComboStep = 3;                 // The maximum number of combo steps
    public int attackDamage = 10;             // Damage dealt per attack
    public Collider2D hitboxCollider;            // Reference to the hitbox collider for detecting enemies

    private int comboStep = 0;                   // Tracks the current combo step (attack 1, 2, or 3)
    private bool canAttack = true;               // Whether the player can attack
    private float lastClickTime = 0f;            // Tracks the time of the last attack
    private Rigidbody2D rb;                      // Reference to the Rigidbody2D for applying velocity
    private Animator anim;                       // Reference to the Animator
    private List<Collider2D> enemiesInRange = new List<Collider2D>(); // List to track enemies inside the hitbox

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        // Get the Rigidbody2D component
        anim = GetComponent<Animator>();         // Get the Animator component

        // Ensure the hitbox collider is a trigger
        if (hitboxCollider != null)
        {
            hitboxCollider.isTrigger = true;
        }
    }

    void Update()
    {
        // Check if the mouse button (left click) is pressed and if attacking is allowed
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            HandleCombo();
        }

        // If enough time has passed since the last click, reset the combo
        if (Time.time - lastClickTime > comboResetTime && comboStep != 0)
        {
            ResetCombo();
        }
    }

    void HandleCombo()
    {
        // Track the time of the last attack
        lastClickTime = Time.time;

        // Advance the combo step up to the maximum allowed
        if (comboStep < maxComboStep)
        {
            comboStep++;
            PlayAttackAnimation(comboStep);    // Trigger the corresponding attack animation
            MoveForward();                     // Apply forward movement during the attack
            DealDamage();                      // Apply damage if enemies are in range
        }
        else if (comboStep == maxComboStep)
        {
            // If the player is on the final step, complete the combo and reset
            PlayAttackAnimation(comboStep);
            MoveForward();
            DealDamage();                      // Apply damage during the final attack
            ResetCombo();                      // Reset combo after finishing the last attack
        }

        // Temporarily disable attacking during the attack cooldown
        canAttack = false;
        Invoke(nameof(EnableAttack), attackCooldown);
    }

    void PlayAttackAnimation(int attackStep)
    {
        // Trigger the appropriate animation based on the combo step
        if (anim != null)
        {
            anim.SetTrigger("Attack" + attackStep); // Animation triggers: "Attack1", "Attack2", "Attack3"
        }
    }

    void MoveForward()
    {
        // Apply a small forward force during the attack
        Vector2 moveDirection = new Vector2(transform.localScale.x * moveForwardAmount, rb.velocity.y); // Moving based on player direction
        rb.velocity = moveDirection;  // Apply movement in the X direction
    }

    void DealDamage()
    {
        // Deal damage to all enemies inside the hitbox
        foreach (Collider2D enemy in enemiesInRange)
        {
            EnemyHealth enemyhealth = enemy.GetComponent<EnemyHealth>();
            if (enemyhealth != null)
            {
                enemyhealth.TakeDamage(attackDamage);  // Apply damage to the enemy
            }
        }
    }

    void EnableAttack()
    {
        // Re-enable attacking after cooldown
        canAttack = true;
    }

    void ResetCombo()
    {
        // Reset the combo back to the first step
        comboStep = 0;
        rb.velocity = Vector2.zero;            // Stop any residual movement after the combo
    }

    // Add enemies to the list when they enter the hitbox
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other);
        }
    }

    // Remove enemies from the list when they exit the hitbox
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other);
        }
    }

    // Visualize the hitbox collider in the editor
    void OnDrawGizmosSelected()
    {
        if (hitboxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitboxCollider.bounds.center, hitboxCollider.bounds.size);
        }
    }
}
