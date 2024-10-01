using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;
    Vector2 CheckPos;

    void Start()
    {
        startPos = transform.position;
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Trap")
        {
            StartCoroutine(Respawn());
        }
    }

    public void UpdateCheckPos(Vector2 pos)
    {
        CheckPos = pos;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = CheckPos;
    }
}
