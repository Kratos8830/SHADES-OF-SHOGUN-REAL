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
       StartCoroutine (Respawn());

        if (hm.healthAmount < 1)
        {
            pc.Damage();
            hm.healthAmount = 100;
            hm.healthFill.fillAmount = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            pc.Damage();
            StartCoroutine(Respawn());
        }

        else if (collision.tag == "Arrow")
        {
            hm.TakeDamage(5);
            pc.PlayHurtAnim();
            collision.gameObject.SetActive(false);
        }

        else if (collision.tag == "Saw")
        {
            hm.TakeDamage(5);
            pc.PlayHurtAnim();
        }
    }


}
