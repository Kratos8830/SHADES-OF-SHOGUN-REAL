using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isKeyTaken = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            isKeyTaken = true;
            //do scene transition from another script
            Debug.Log("Key is Taken now you change scene");
            Destroy(this.gameObject);
        }
    }
}
