using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject Door;
    public Vector3 doorClosedPos;
    public Vector3 doorOpenPos;

    private void Awake()
    {
        Door.transform.position = doorClosedPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance.HasAllScrolls())
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("You need more scrolls to open the door!");
            }
        }
    }

    private void OpenDoor()
    {
        Door.transform.position = doorOpenPos;
    }

}
