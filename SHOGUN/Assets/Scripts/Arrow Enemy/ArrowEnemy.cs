using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    public Transform player, enemy;
    public float disbtwplayerenemy=12.2f;
    private Animator anim;
    private bool isFacingRight=true;
    private EnemyShoot es;
    private PlayerController pc;

    void Start()
    {
        anim = GameObject.Find("Arrow Enemy").GetComponentInChildren<Animator>();
        es= GameObject.Find("Arrow Enemy").GetComponentInChildren<EnemyShoot>();
        pc=GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CalulateDistance();
    }

    void Flip()
    {
       
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void CalulateDistance()
    {
        float distance = player.position.x - enemy.position.x;
        FlipTowardsPlayer();
        if (Mathf.Abs(distance) < disbtwplayerenemy && pc.movementInputDirection>=0)
        {
            anim.SetTrigger("attack");
            es.Shoot(!isFacingRight);
        }
    }

    void FlipTowardsPlayer()
    {
        // Flip towards the player's position
        if (player != null)
        {
            if (player.position.x < transform.position.x && !isFacingRight)
            {
                Flip();
            }
            else if (player.position.x > transform.position.x && isFacingRight)
            {
                Flip();
            }
        }
    }
}
