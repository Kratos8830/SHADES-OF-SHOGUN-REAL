using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool isDoorOpen = false;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    public float doorspeed = 10f;

    void Awake()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x,transform.position.y + 5f,transform.position.z);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(isDoorOpen) 
        {
            OpenDoor();
        }

        else if(!isDoorOpen)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if(transform.position != doorOpenPos) 
        {
            transform.position = Vector3.MoveTowards(transform.position,
            doorOpenPos,doorspeed*Time.deltaTime);
        }
    }

    public void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                doorClosedPos, doorspeed * Time.deltaTime);
        }
    }
}
