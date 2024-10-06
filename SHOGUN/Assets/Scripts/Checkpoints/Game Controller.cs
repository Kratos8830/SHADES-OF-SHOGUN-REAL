using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //restart position after health become 0
    Vector2 RestartStartLevelPos;
    Vector2 startPos;
    Vector2 CheckPos;
    private HealthManager hm;
    private PlayerController pc;

    void Start()
    {
        RestartStartLevelPos = transform.position;
        startPos = transform.position;
        hm=GameObject.Find("Health Manager").GetComponent<HealthManager>();
        pc=GameObject.Find("Player").GetComponent<PlayerController>();
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

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = CheckPos;

        
    }

    public void ZeroHealth()
    {
        transform.position = RestartStartLevelPos;

        if (hm.healthAmount < 1)
        {
            hm.healthAmount = 100;
            hm.healthFill.fillAmount = 1;
        }
    }

    
}
