using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geisha : MonoBehaviour
{
    public bool canDash=false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            //Debug.Log("canDash true");
            canDash = true;
        }
    }
}
