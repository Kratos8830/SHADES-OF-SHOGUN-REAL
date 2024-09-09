using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject myEnemy;
    public Animator animator;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;  
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
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy dies");
        animator.SetBool("isDead", true);
        Destroy(rb);
        //myEnemy.GetComponent<Collider2D>().enabled = false;
        this.enabled = true;
        GetComponent<EnemyAttack>().enabled = false;
        
    }

}
