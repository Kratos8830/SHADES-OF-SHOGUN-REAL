using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
 
    //especially for Shoot script gets access of Sprite Rendered of Player
    public SpriteRenderer sprite;  
    private float HorizontalInput;
    public float jumpforce = 3.0f;
    public bool Grounded = true;
    public LayerMask JumpableGround;
    public bool DoubleJump = false;

    // For Wall Slide
    public bool isWallSliding = false;
    public float WallSlidingSpeed = 2.0f;

    // For Wall Jump
    public float wallJumpForce = 16.0f;
    private bool isWallJumping;

    // For Dash
    private bool canDash = true;
    private bool isDashing;
    public float dashAmount = 16f;
    private float DashTime = 0.2f;
    private float DashCoolDown = 1f;
    private TrailRenderer trail;
    private float originalGravity;

    // For Checking Walls
    [SerializeField]
    private LayerMask WallLayer;
    [SerializeField]
    private Transform WallCheck;

    private bool isFacingRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
        sprite = GetComponentInChildren<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || isWallJumping)
        {
            return; // Exit Update early if currently dashing or wall jumping
        }
        if(!isWallJumping) 
        {
            HorizontalInput = Input.GetAxis("Horizontal");

        }

        rb.velocity = new Vector2(HorizontalInput * 7, rb.velocity.y);

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
        GroundCheck();
       
        WallSlide();
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
            if (Grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                
                Grounded = false;
                DoubleJump = true;
            }
            else if (DoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.7f);
                DoubleJump = false;
            }
            else if (isWallSliding)
            {
                WallJump();
            }
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector3.down, 1.5f, JumpableGround);
       
        if (hitinfo.collider != null)
        {
            Grounded = true;
        
        }
    }

    void Flip()
    {
        // Flip the character by inverting the x scale
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine the dash direction based on the player's facing direction
        float dashDirection = !isFacingRight ? -1f : 1f;
        rb.velocity = new Vector2(dashDirection * dashAmount, 0f);

        trail.emitting = true;
        yield return new WaitForSeconds(DashTime);
        trail.emitting = false;

        // Stop the player's movement after the dash
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.2f, WallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !Grounded && HorizontalInput != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        isWallJumping = true;

        // Determine the jump direction based on the wall's side
        float jumpDirection = sprite.flipX ? 1f : -1f;

        // Apply the jump force with some horizontal velocity
        rb.velocity = new Vector2(jumpDirection * wallJumpForce, jumpforce);

        // Flip the player's sprite
        sprite.flipX = jumpDirection == 1f ? false : true;

        // Prevent further wall jumps for a short duration
        StartCoroutine(ResetWallJump());
    }

    private IEnumerator ResetWallJump()
    {
        yield return new WaitForSeconds(0.2f);
        isWallJumping = false;
    }
}
