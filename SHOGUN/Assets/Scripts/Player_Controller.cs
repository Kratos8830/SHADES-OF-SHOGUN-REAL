
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // --- Variables ---

    // General Movement Variables
    public float movementInputDirection;
    public bool isFacingRight = true;
    private bool isWalking;
    public bool canMove = true;
    private bool canFlip = true;
    private int facingDirection = 1;

    public float movementSpeed = 10.0f;

    // Jumping Mechanics Variables
    private float jumpTimer;
    private int amountOfJumpsLeft;
    private bool canNormalJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;

    public int amountOfJumps = 1;
    public float jumpForce = 16.0f;
    public float jumpTimerSet = 0.15f;
    public float variableJumpHeightMultiplier = 0.5f;

    // Wall Mechanics Variables
    private float turnTimer;
    private float wallJumpTimer;
    private int lastWallJumpDirection;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canWallJump;
    private bool hasWallJumped;

    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float wallHopForce;
    public float wallJumpForce;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    // Dashing Mechanics Variables
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private bool isDashing;

    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    // Ground Mechanics Variables
    private bool isGrounded;

    public float groundCheckRadius;

    // Component References
    Rigidbody2D platform;
    public Rigidbody2D rb;
    private Animator anim;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;

    //Audio
    private bool isRunningSoundPlaying = false;
    public AudioSource runningSound;

    //Post Processing
    public ParticleSystem dust;
    private HealthManager hm;


    public int lives = 2;
    public GameObject restartMenu;
    private PlayerController pc;
    private bool isAutoMoving = false;

    //npc reference 
    public Izo iz;



    // --- Start & Update ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!Pause_Menu.isPaused)
        {
            if (!isAutoMoving)  // Prevent regular input when auto-moving
            {
                CheckInput();
                CheckMovementDirection();
            }

            UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckJump();
            CheckDash();
        }

        if (Pause_Menu.isPaused)
        { 
            runningSound.Stop();
        }
    }


    private void FixedUpdate()
    {
        if (isAutoMoving)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);  // Auto move to the right
        }
        else
        {
            ApplyMovement();
        }
        CheckSurroundings();
        MovingPlatform();
    }

    // --- AutoMove Mecahnics which enable at Endzone (Trial) --- 

    public void ResetPlayerState()
    {
        // Stop dash and jump mechanics
        isDashing = false;
        isAttemptingToJump = false;
        rb.velocity = Vector2.zero; // Reset velocity

        // Reset animation parameters
        anim.SetBool("isWalking", false);
        anim.SetBool("isGrounded", true);
        anim.SetBool("isWallSliding", false);
        //anim.ResetTrigger("dash"); // Optional: Ensure any dash trigger is reset if used
    }

    public void SetAutoMove(bool autoMove)
    {
        if (autoMove)
        {
            ResetPlayerState();  // Reset player state upon entering auto-move
        }

        isAutoMoving = autoMove;
        canMove = !autoMove;
        canFlip = !autoMove;

        // Force rightward movement and run animation when auto-moving
        rb.velocity = new Vector2(autoMove ? movementSpeed : 0, 0);
        anim.SetBool("isWalking", autoMove);
    }

    // ------------------------------------------------------------------------------------


    // --- General Movement Mechanics ---
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        isWalking = Mathf.Abs(rb.velocity.x) >= 0.01f;
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.95f, rb.velocity.y);  // Air drag applied
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }

        if (isWallSliding && rb.velocity.y < -wallSlideSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);

            if (isGrounded)
            {
                dust.Play();
            }
        }
    }

    // --- Jumping Mechanics ---
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        canWallJump = isTouchingWall;
        canNormalJump = amountOfJumpsLeft > 0;
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;

            dust.Play();
            // Play jumping sound
            SoundManager.Instance.PlaySound2D("JumpStart");
            SoundManager.Instance.PlaySound2D("Jump");
            
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    // --- Wall Mechanics ---
    private void CheckIfWallSliding()
    {
        //if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        //{
        //    isWallSliding = true;
        //}
        //else
        //{
        //    isWallSliding = false;
        //}

        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }

    // --- Dashing Mechanics ---
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                // During dash, disable movement and flipping but keep running
                canMove = false;
                canFlip = false;

                rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f); // Apply dash velocity
                dashTimeLeft -= Time.deltaTime;

                // Keep spawning dash images if using trail effect
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AI_Pool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                // Dash finished, allow movement again
                isDashing = false;
                canMove = true;
                canFlip = true;

                // Stop running animation only if no input, otherwise continue running
                if (Mathf.Abs(movementInputDirection) < 0.01f)
                {
                    anim.SetBool("isWalking", false); // Transition back to idle if no input
                }
                else if (isGrounded)
                {
                    anim.SetBool("isWalking", true); // Continue running if player is moving
                }
            }
        }
    }


    private void AttemptToDash()
{
    if (Time.time >= (lastDash + dashCoolDown))
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        if (isGrounded)
        {
            anim.SetBool("isWalking", true); // Force running animation

            dust.Play(); // Dust effect for visual feedback
        }

        SoundManager.Instance.PlaySound2D("Dash");

        AI_Pool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
}




    // --- Ground Mechanics ---
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    // --- Input Handling ---
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else if (isTouchingWall && isWallSliding && movementInputDirection == 0)
            {
                WallJump();
            }
            else
            {
                isAttemptingToJump = true;
                jumpTimer = jumpTimerSet;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && iz.canDash == true)
        {
            AttemptToDash();
        }
    }

    private void UpdateAnimations()
    {
        //not changed code for automove here

        // Force running animation if dashing, regardless of movement
        if (isDashing)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Update the grounded, walking, and wall sliding states
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.velocity.y);
            anim.SetBool("isWallSliding", isWallSliding);

            // Only play running animation if player is actively walking (input direction) and not just due to platform movement
            if (Mathf.Abs(movementInputDirection) > 0.01f && isGrounded)
            {
                anim.SetBool("isWalking", true);  // Running animation
            }
            else
            {
                anim.SetBool("isWalking", false);  // Idle animation
            }
        }

        // Handle running sound based on player input and grounded state
        if (anim.GetBool("isWalking") && !runningSound.isPlaying)
        {
            runningSound.Play();
        }
        else if (!anim.GetBool("isWalking") || !isGrounded)
        {
            runningSound.Stop();
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
            platform = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
            platform = null;
            rb.gravityScale = 3f;
        }
    }

    void MovingPlatform()
    {
        if (platform)
        {
            // Only apply the platform's velocity when the player is not moving (idle on the platform)
            if (Mathf.Abs(movementInputDirection) < 0.01f)
            {
                rb.velocity = new Vector2(platform.velocity.x, rb.velocity.y);
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    public void PlayHurtAnim()
    {
        anim.SetTrigger("hurt");
    }

    public void Damage()
    {
        lives--;

        if(lives <1)
        {
            restartMenu.SetActive(true);
            pc.enabled = false;
        }
    }
}



