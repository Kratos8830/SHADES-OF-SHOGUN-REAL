using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    private DoorBehaviour door;
    [SerializeField]
    private bool isDoorOpenSwitch;
    [SerializeField]
    private bool isDoorClosedSwitch;

    private float switchSizeY;
    private Vector3 switchUpPos;
    private Vector3 switchDownPos;
    public float switchSpeed = 8.0f;
    private float switchDelay = 0.2f;
    private bool ispressingSwitch = false;

    void Awake()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }
    void Start()
    {
        door = GameObject.Find("Door").GetComponent<DoorBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ispressingSwitch)
        {
            MoveSwitchDown();
        }

        else if (!ispressingSwitch)
        {
            MoveSwitchUp();
        }
    }

    public void MoveSwitchDown()
    {
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position,
            switchDownPos, switchSpeed * Time.deltaTime);
        }
    }

    public void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position,
            switchUpPos, switchSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ispressingSwitch = !ispressingSwitch;
            if (isDoorOpenSwitch && !door.isDoorOpen)
            {
                door.isDoorOpen = !door.isDoorOpen;
            }

            else if (isDoorClosedSwitch && door.isDoorOpen)
            {
                door.isDoorOpen = !door.isDoorOpen;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }

    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ispressingSwitch = false;
    }
}

