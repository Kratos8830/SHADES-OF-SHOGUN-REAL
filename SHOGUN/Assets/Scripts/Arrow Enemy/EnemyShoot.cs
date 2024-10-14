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

    public void Shoot(bool isFacingRight)
    {
        if (shootTimer >= timeBtwAttacks)
        {
            shootTimer = 0;
            Rigidbody2D arrowRB = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            float direction = isFacingRight ? 1f : -1f;
            arrowRB.velocity = new Vector2(direction * arrowSpeed, 0f);

            //for changing arrow rotation depending upon is Player facing right 
            Vector3 arrowScale = arrowRB.transform.localScale;
            arrowScale.x = isFacingRight ? Mathf.Abs(arrowScale.x) : -Mathf.Abs(arrowScale.x);
            arrowRB.transform.localScale = arrowScale;

            Destroy(arrowRB.gameObject, arrowDestroyTime);
        }
    }
}
