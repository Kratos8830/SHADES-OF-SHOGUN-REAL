using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ninjastar : MonoBehaviour
{
    [SerializeField] private float speed = 6;
    [SerializeField] private Rigidbody2D rb;
    private bool hit;
    public int damage = 40;
    private float direction;
   public GameObject boss;


    void Start()
    {
        rb.velocity = transform.right * speed;
       
    }

    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
       
        BossHealth bossHealth = HitInfo.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
