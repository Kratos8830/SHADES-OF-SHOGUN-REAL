using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    protected Transform pointA, pointB;
    protected Vector3 CurrenTarget;
    [SerializeField]
    private float speed = 5.0f;
    private bool isfacingRight;
    private Animator anim;
    //for attack
    public Transform player;
    public Transform enemy;
    public EnemyShoot es;



    void Start()
    {
       anim=GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            return;
        }
        Movement();
        Attack();
    }

    void Flip()
    {
        
        isfacingRight = !isfacingRight;

        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Movement()
    {
        if (transform.position == pointA.position)
        {
            //Debug.Log("PointA");
            CurrenTarget = pointB.position;
            if (!isfacingRight)
            {
                Flip();
            }
            anim.SetTrigger("idle");

        }


        else if (transform.position == pointB.position)
        {
            CurrenTarget = pointA.position;
            if (isfacingRight)
            {
                Flip();
            }
            anim.SetTrigger("idle");

        }

        transform.position = Vector3.MoveTowards(transform.position, CurrenTarget, speed * Time.deltaTime);

        
    }

    void Attack()
    {
        float distance = player.position.x - enemy.position.x;

        if (Mathf.Abs(distance) < 15.0f)
        {
            anim.SetBool("inCombat", true);
            if (distance > 0 && !isfacingRight)
            {
                Flip();
            }
            // Check if the player is to the left of the enemy and the enemy is facing right
            else if (distance < 0 && isfacingRight)
            {
                Flip();
            }
        }

        else
        {
            anim.SetBool("inCombat", false);
        }
    }


}
