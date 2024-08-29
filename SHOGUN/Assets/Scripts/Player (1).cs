using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    
    //especially for Shoot script gets access of Sprite Rendered of Player
    public SpriteRenderer sprite;  
    private float HorizontalInput;
    public float jumpforce = 10.0f;
    public bool isGrounded ;
    public LayerMask JumpableGround;
    public bool DoubleJump = false;
    private bool isFacingRight = true;
    public Transform Groundcheck;
    public float groundCheckRadius;
    public float movementspeed = 5;
    

  

    // For Dash
    private bool canDash = true;
    private bool isDashing;
    public float dashAmount = 24f;
    private float DashTime = 0.2f;
    private float DashCoolDown = 1f;
    private float originalGravity;

    // for walljump

    private bool isTouchingWall;
    public Transform wallCheck;
    public float Wallcheckdistance;
    public LayerMask whatisWall;

    private bool isWallSliding;
    public float wallSlidingSpeed;

    //in Air
    public float movementForceInAir;
    public float AirDragSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        sprite = GetComponentInChildren<SpriteRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing )
        {
            return; // Exit Update early if currently dashing or wall jumping
        }

        HorizontalInput = Input.GetAxis("Horizontal");

        
        if (isGrounded)
        {
            rb.velocity = new Vector2(HorizontalInput * 7, rb.velocity.y);
        }
        else if(!isGrounded && !isWallSliding && HorizontalInput !=0)
        {
            Vector2 forcetoadd = new Vector2(movementForceInAir * HorizontalInput, 0);
            rb.AddForce(forcetoadd);

            if(Mathf.Abs(rb.velocity.x)> movementspeed)
            {
                rb.velocity = new Vector2 (movementspeed * HorizontalInput, rb.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && HorizontalInput ==0)
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
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.7f);
                DoubleJump = false;
            }
           
        }
    }

   private void CheckSurrounding()
   {
        isGrounded = Physics2D.OverlapCircle(Groundcheck.position, groundCheckRadius, JumpableGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right,Wallcheckdistance,whatisWall);
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
            if(rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2 (rb.velocity.x,-wallSlidingSpeed);
            }
        }
    }

    void Flip()
    {
        if (!isWallSliding)
        {

            Vector3 currentScale = gameObject.transform.localScale;
            currentScale.x *= -1;
            gameObject.transform.localScale = currentScale;
            isFacingRight = !isFacingRight;
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
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + Wallcheckdistance,wallCheck.position.y,wallCheck.position.z));
    }






}
