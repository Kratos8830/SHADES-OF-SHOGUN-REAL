using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;           // Damage dealt to the enemy
    public float attackCooldown = 0.5f;        // Time between attacks
    public Collider2D attackTrigger;           // The trigger collider representing the attack area
    public LayerMask enemyLayer;               // Layer to detect enemies
    private float attackTimer = 0f;            // Timer to track cooldown
    private bool canAttack = true;

    private void Start()
    {
        attackTrigger.enabled = false;         // Disable the trigger at start
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;         // Countdown the cooldown

        if (Input.GetButtonDown("Fire1") && attackTimer <= 0f)
        {
            Attack();                          // Perform attack
            attackTimer = attackCooldown;      // Reset cooldown
        }
    }

    private void Attack()
    {
        canAttack = true;
        attackTrigger.enabled = true;          // Enable the attack trigger when attacking
        Debug.Log("Player attacking!");

        // Optional: Play attack animation here if using an animator

        Invoke("DisableAttackTrigger", 0.1f);  // Disable the trigger after a short time to avoid multiple hits
    }

    private void DisableAttackTrigger()
    {
        attackTrigger.enabled = false;         // Disable the attack trigger after the attack is performed
        canAttack = false;
    }

    // Detect when the enemy enters the attack range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canAttack && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Apply damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy hit by player!");

                canAttack = false;  // Prevent applying damage multiple times in the same attack
                DisableAttackTrigger();  // Immediately disable the attack trigger
            }
        }
    }
}
