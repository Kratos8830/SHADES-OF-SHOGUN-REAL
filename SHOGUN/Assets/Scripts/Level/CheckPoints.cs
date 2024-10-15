using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private GameController gameController;
    void Start()
    {
        gameController=GameObject.Find("Player").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            gameController.UpdateCheckPos(transform.position);
        }
    }
}
