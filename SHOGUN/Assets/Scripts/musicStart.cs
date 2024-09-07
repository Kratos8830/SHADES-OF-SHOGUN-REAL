using UnityEngine;

public class musicStart : MonoBehaviour
{
    public AudioSource audioSource;   // The AudioSource component to play the audio
    public AudioClip Clip;        // The AudioClip to play when the player enters the trigger
    public AudioClip stopClip;        // Optional: Another clip to play when stopping, if needed

    // Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Music"))
        {
            // Play the assigned audio clip
            audioSource.clip = Clip;
            audioSource.Play();
            Debug.Log("music");
        }
    }

    // Trigger Exit
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag("Music"))
        {
            // Stop the audio or play another clip if needed
            audioSource.Stop();

           
        }
    }
}
