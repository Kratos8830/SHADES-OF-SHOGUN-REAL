using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    public Transform player, enemy;
    public float disbtwplayerenemy = 12.2f;
    public Animator anim;
    private bool isFacingRight = true;
    public EnemyShoot es;
    public PlayerController pc;
    public EnemyHealth eh;



    void Start()
    {
        GameObject arrowEnemyObject = GameObject.Find("Arrow Enemy");
        if(arrowEnemyObject != null)
        {
            anim = GameObject.Find("Arrow Enemy").GetComponentInChildren<Animator>();
            es = GameObject.Find("Arrow Enemy").GetComponentInChildren<EnemyShoot>();
        }
        else
        {
            Debug.LogError("Arrow Enemy object not found in the scene.");
        }

        pc = GameObject.Find("Player").GetComponent<PlayerController>();

        if (anim == null || es == null || pc == null)
        {
            Debug.LogWarning("Some components could not be initialized.");
        }
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
        if (Mathf.Abs(distance) < disbtwplayerenemy && pc.movementInputDirection >= 0)
        {
            anim.SetTrigger("attack");
            if (eh.ismyEnemyDied == false)
            {
                es.Shoot(!isFacingRight);
            }
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

    public void StopArrowShoot()
    {
        es.enabled = false; 
    }


}
