using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    void Start()
    {
        
    }

  
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time+1f/attackRate;
            }
        }
      
    }

    void Attack()
    {
        //animator.SetTrigger("Attack");


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("hitting Enemy");
            enemy.GetComponent<EnemyHealth>().TakeDamage(40);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
