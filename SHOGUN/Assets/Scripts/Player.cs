

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;


    // Float Values
    public float movementspeed = 5f;              // Ground movement speed
    public float jumpforce = 10f;                 // Jump force
    public float groundCheckRadius = 0.2f;        // Ground check radius
    public float movementForceInAir = 10f;        // Movement force in the air
    public float AirDragSpeed = 0.85f;            // Air drag speed multiplier

    // Wall Mechanics
    public float Wallcheckdistance = 0.5f;        // Distance to check for walls
    public float wallSlidingSpeed = 1.5f;         // Speed while sliding down the wall
    public float wallJumpForceX = 25f;            // Wall jump force (horizontal)
    public float wallJumpForceY = 13f;            // Wall jump force (vertical)
    public float wallJumpDirection = 1f;          // Wall jump direction multiplier

    // Dash Mechanics
    public float dashAmount = 24f;                // Dash distance or speed
    public float DashTime = 0.2f;                 // Duration of dash
    public float DashCoolDown = 1f;               // Cooldown time before next dash
    public float distanceBetweenImages;
    private bool canDash = true;                  // Can the player dash
    private bool isDashing;                       // Is the player currently dashing
    private float originalGravity;                // Store original gravity for dash
    private float lastImageXpos;

    // Transform Values
    public Transform Groundcheck;                 // Ground check position
    public Transform wallCheck;                   // Wall check position

    // LayerMask Values
    public LayerMask JumpableGround;              // Ground layers mask
    public LayerMask whatisWall;                  // Wall layers mask

    // Boolean Values
    public bool DoubleJump = false;               // Double jump availability
    public bool isGrounded;                       // Is the player grounded
    private bool isTouchingWall;                  // Is the player touching a wall
    private bool isWallSliding;                   // Is the player sliding on a wall
    private bool isFacingRight = true;            // Direction player is facing

    // Variable jump height
    public float variableJumpheightmultiplier = 0.5f;

    // Object pooling (e.g., for afterimages during dashing)
    public float afterImageSpawnRate = 0.05f;     // How often to spawn afterimages

    // Input Values
    private float HorizontalInput;                // Player input for horizontal movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponentInChildren<SpriteRenderer>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing)
        {
            return; // Exit Update early if currently dashing
        }

        HorizontalInput = Input.GetAxis("Horizontal");

        // Handle Movement
        HandleMovement();

        // Handle Jumping
        Jump();

        // Check if player is touching wall or grounded
        CheckSurrounding();

        // Handle Wall Sliding
        CheckIfWallSliding();
        WallSliding();

        // Handle Wall Jump
        WallJump();

        // Handle Dashing
        DashMechanics();

        // Flip player sprite when changing direction
        if (HorizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (HorizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        anim.SetFloat("run", Mathf.Abs(HorizontalInput));
        UpdateAnimations();
    }

    // --- Movement Mechanics ---
    void HandleMovement()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(HorizontalInput * movementspeed, rb.velocity.y);
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
    }

    private void UpdateAnimations()
    {
        //anim.SetFloat("isWalking", HorizontalInput);
        //anim.SetBool("isGrounded", isGrounded);
        //anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    // --- Dash Mechanics ---
    void DashMechanics()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            //anim.SetBool("dash", true);

            //After Image Shit
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AI_Pool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
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

        //After Image Shit
        AI_Pool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

        yield return new WaitForSeconds(DashTime);
        //anim.SetBool("dash", false);

        // Stop the player's movement after the dash
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
    }

    // --- Jump Mechanics ---
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                anim.SetBool("jump", true);
                isGrounded = false;
                DoubleJump = true;
                //StartCoroutine(StopDoubleJumpAnim());
            }

            else if (DoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 1f);
                anim.SetBool("doublejump", true);
                StartCoroutine(StopDoubleJumpAnim());
                DoubleJump = false;
            }
        }

        // Variable jump height control
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpheightmultiplier);
        }
    }

    // --- Wall Mechanics ---
    void WallJump()
    {
        if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpDirection = isFacingRight ? -1f : 1f;
            rb.velocity = new Vector2(jumpDirection * wallJumpForceX, wallJumpForceY);
            isWallSliding = false;

            // Flip player after wall jump if needed
            if ((isFacingRight && jumpDirection < 0) || (!isFacingRight && jumpDirection > 0))
            {
                Flip();
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
            anim.SetBool("wallslide", true);
        }
        else
        {
            anim.SetBool("wallslide", false);
            isWallSliding = false;
        }
    }

    private void WallSliding()
    {
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    // --- Other Functions ---
    void Flip()
    {
        if (!isWallSliding)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            wallCheck.transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator StopDoubleJumpAnim()
    {
        yield return new WaitForSeconds(1.3f);
        anim.SetBool("doublejump", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + Wallcheckdistance, wallCheck.position.y, wallCheck.position.z));
    }
}
