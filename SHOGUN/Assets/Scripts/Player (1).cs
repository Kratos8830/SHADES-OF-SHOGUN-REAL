using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    //Main player

    public GameObject player;
    private Animator anim;
    // Float Values
    public float movementspeed = 5f;              // Ground movement speed
    public float jumpforce = 10f;                 // Jump force
    public float groundCheckRadius = 0.2f;        // Ground check radius
    public float dashAmount = 24f;                // Dash distance or speed
    public float DashTime = 0.2f;                 // Duration of dash
    public float DashCoolDown = 1f;               // Cooldown time before next dash
    public float Wallcheckdistance = 0.5f;        // Distance to check for walls
    public float wallSlidingSpeed = 1.5f;         // Speed while sliding down the wall
    public float movementForceInAir = 10f;        // Movement force in the air
    public float AirDragSpeed = 0.85f;            // Air drag speed multiplier

    // Transform Values
    public Transform Groundcheck;                 // Ground check position
    public Transform wallCheck;                   // Wall check position

    // LayerMask Values
    public LayerMask JumpableGround;              // Ground layers mask
    public LayerMask whatisWall;                  // Wall layers mask

    // Boolean Values
    public bool DoubleJump = false;               // Double jump availability
    public bool isGrounded;                       // Is the player grounded
    private bool canDash = true;                  // Can the player dash
    private bool isDashing;                       // Is the player currently dashing
    private bool isTouchingWall;                  // Is the player touching a wall
    private bool isWallSliding;                   // Is the player sliding on a wall
    private bool isFacingRight = true;            // Direction player is facing

    // Other Values
    public SpriteRenderer sprite;                 // Reference to the player's Sprite Renderer
    private float HorizontalInput;                // Player input for horizontal movement
    private float originalGravity;                // Store original gravity for dash



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return; // Exit Update early if currently dashing or wall jumping
        }

        HorizontalInput = Input.GetAxis("Horizontal");


        if (isGrounded)
        {
            rb.velocity = new Vector2(HorizontalInput * 7, rb.velocity.y);
        }
        else if (!isGrounded && !isWallSliding && HorizontalInput != 0)
        {
            Vector2 forcetoadd = new Vector2(movementForceInAir * HorizontalInput, 0);
            rb.AddForce(forcetoadd);

            if (Mathf.Abs(rb.velocity.x) > movementspeed)
            {
                rb.velocity = new Vector2(movementspeed * HorizontalInput, rb.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && HorizontalInput == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * AirDragSpeed, rb.velocity.y);
        }

        if (HorizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (HorizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        DashMechanics();
        Jump();
        CheckSurrounding();
        wallSliding();
        CheckIfWallSliding();

        anim.SetFloat("run", Mathf.Abs(HorizontalInput));

    }

    void DashMechanics()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);

                isGrounded = false;
                DoubleJump = true;
            }
            else if (DoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 1f);
                DoubleJump = false;
            }

        }
    }

    private void CheckSurrounding()
    {
        isGrounded = Physics2D.OverlapCircle(Groundcheck.position, groundCheckRadius, JumpableGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, Wallcheckdistance, whatisWall);
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            Debug.Log("walled");
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void wallSliding()
    {
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    void Flip()
    {
        if (!isWallSliding)
        {
            isFacingRight = !isFacingRight;
            player.transform.Rotate(0f, 180f, 0f);
            wallCheck.transform.Rotate(0f,180f, 0f);
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine the dash direction based on the player's facing direction
        float dashDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashAmount, 0f);

        yield return new WaitForSeconds(DashTime);


        // Stop the player's movement after the dash
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + Wallcheckdistance, wallCheck.position.y, wallCheck.position.z));
    }

}

