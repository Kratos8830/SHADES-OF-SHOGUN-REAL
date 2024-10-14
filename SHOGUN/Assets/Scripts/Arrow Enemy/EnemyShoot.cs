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
    private void Update()
    {
        shootTimer += Time.deltaTime;

        if(shootTimer >= timeBtwAttacks)
        {
            shootTimer = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        arrowRB=Instantiate(arrowPrefab,transform.position,Quaternion.identity);

        arrowRB.velocity =  arrowRB.transform.right * -arrowSpeed ;

        Destroy(arrowRB.gameObject, arrowDestroyTime);
    }
}
