using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sprite;
    private float HorizontalInput;
    public float jumpforce = 6.0f;
    public bool Grounded = true;
    public LayerMask JumpableGround;
    public bool DoubleJump = false;


    private bool WallSliding;
    private float WallslidingSpeed = 5f;
    [SerializeField] private Transform wallcheck;
    [SerializeField] private LayerMask wallLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        HorizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(HorizontalInput * 7, rb.velocity.y);

        Jump();
        GroundCheck();
        Flip();
        Wallslide();
    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Grounded && DoubleJump != true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                Grounded = false;
                DoubleJump = true;

            }
            else if (DoubleJump == true && Grounded != false)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.7f);
                DoubleJump = false;
            }
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector3.down, 1.5f, JumpableGround);

        if (hitinfo.collider != null)
        {
            Grounded = true;
            Debug.Log("grounded");

        }
    }
    private bool isWalled()
    {
       return Physics2D.OverlapCircle(wallcheck.position, 0.2f, wallLayer);
        
    }
    private void Wallslide()
    {
        if(isWalled() && Grounded != true && HorizontalInput > 0)
        {
            WallSliding=true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-WallslidingSpeed,float.MaxValue));
            Debug.Log("walled");
        }
        else
        {
            WallSliding = false;
        }
    }

    void Flip()
    {
        if (HorizontalInput > 0)
        {
            sprite.flipX = false;
        }
        else if (HorizontalInput < 0)
        {
            sprite.flipX = true;
        }
    }

}
