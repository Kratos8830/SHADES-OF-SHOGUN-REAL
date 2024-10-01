using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject myEnemy;
    public Rigidbody2D rb;
    public Animator animator;
    void Start()
    {
        currentHealth = maxHealth;  
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            animator.SetTrigger("dead");
            animator.SetBool("isDead", true);
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died");

        
        myEnemy.GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<EnemyHealth>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        Destroy(rb);
        this.enabled = true;
    }

}
