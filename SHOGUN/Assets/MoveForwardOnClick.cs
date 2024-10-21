using UnityEngine;

public class MoveForwardOnClick : MonoBehaviour
{
    public float moveForwardAmount = 1f;         // Amount of force applied on click
    public float movementCooldown = 0.5f;       // Cooldown between movements

    private bool canMove = true;                 // Whether the player can move
    public Rigidbody2D rb;                      // Reference to the Rigidbody2D

    void Start()
    {
       
    }

    void Update()
    {
        // Check if the mouse button (left click) is pressed and if movement is allowed
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            MoveForward();   // Move the player forward
            canMove = false; // Disable further movement until cooldown ends
            Invoke(nameof(ResetMove), movementCooldown);  // Reset movement after cooldown
        }
    }

    void MoveForward()
    {
        // Move the player a little forward based on the direction they are facing
        Vector2 moveDirection = new Vector2(transform.localScale.x * moveForwardAmount, rb.velocity.y); // Maintain Y-axis velocity
        rb.velocity = moveDirection;  // Apply movement in the X direction
    }

    void ResetMove()
    {
        // Re-enable movement after the cooldown
        canMove = true;
    }
}
