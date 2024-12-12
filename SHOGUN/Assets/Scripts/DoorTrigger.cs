using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject DoorPos;
    public GameObject Door;
    private Vector3 doorOpenPos;

    private void Awake()
    {
       Door.transform.position= DoorPos.transform.position;
        doorOpenPos = new Vector3(DoorPos.transform.position.x, DoorPos.transform.position.y + 5f, DoorPos.transform.position.z);
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
