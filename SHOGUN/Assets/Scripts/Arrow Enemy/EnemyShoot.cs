using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D arrowPrefab;
    [SerializeField]
    private float timeBtwAttacks = 2f;
    [SerializeField]
    private float arrowSpeed;
    [SerializeField]
    private float arrowDestroyTime = 1f;
    private float shootTimer;
    private Rigidbody2D arrowRB;
    
   
    void Update()
    {
        shootTimer += Time.deltaTime;
        
    }

    public void Shoot(bool FacingRight)
    {
        if (shootTimer >= timeBtwAttacks)
        {
            shootTimer = 0;
            Rigidbody2D arrowRB = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            float direction = FacingRight ? 1f : -1f;
            // Flip the arrow's local scale if the player is facing right
            if (direction == 1f)
            {
                arrowRB.transform.localScale = new Vector3(2f, 2f, 2f); 
            }
            else
            {
                arrowRB.transform.localScale = new Vector3(-2f, 2f, 2f); 
            }
            arrowRB.velocity = new Vector2(direction * arrowSpeed, 0f);
            Destroy(arrowRB.gameObject, arrowDestroyTime);
        }

    }

  
}
