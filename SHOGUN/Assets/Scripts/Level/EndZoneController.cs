using UnityEngine;

public class EndZoneController : MonoBehaviour
{
    public float autoMoveSpeed = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetAutoMove(true);
                playerController.rb.velocity = new Vector2(autoMoveSpeed, 0); // Set rightward movement
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetAutoMove(false); // Allow control again
            }
        }
    }



}
