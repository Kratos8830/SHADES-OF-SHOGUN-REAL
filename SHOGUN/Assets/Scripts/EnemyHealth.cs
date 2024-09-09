using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject myEnemy;
    void Start()
    {
        currentHealth = maxHealth;  
    }


    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Margaya Maderchod");

        myEnemy.GetComponent<Collider2D>().enabled = false;
        this.enabled = true;
    }

}
