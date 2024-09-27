using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    

    public float dashAmount = 24f;                // Dash distance or speed
    public float DashTime = 0.2f;                 // Duration of dash
    public float DashCoolDown = 1f;               // Cooldown time before next dash
    public float distanceBetweenImages;
    private bool canDash = true;                  // Can the player dash
    private bool isDashing;                       // Is the player currently dashing
    private float originalGravity;                // Store original gravity for dash
    private float lastImageXpos;

    void Start()
    {
        
    }

  
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {


            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("attack");
                DashMechanics();
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("attack2");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void DashMechanics()
    {
       
            StartCoroutine(Dash());
            //anim.SetBool("dash", true);

            //After Image Shit
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AI_Pool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine the dash direction based on the player's facing direction
        float dashDirection = GetComponent<Player>().isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashAmount, 0f);

        //After Image Shit
        AI_Pool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

        yield return new WaitForSeconds(DashTime);
        //anim.SetBool("dash", false);

        // Stop the player's movement after the dash
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
    }

    void Attack()
    {
     


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        foreach(Collider2D enemy in hitEnemies)
        {
           
            Debug.Log("hitting enemy");
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }


    }

  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
