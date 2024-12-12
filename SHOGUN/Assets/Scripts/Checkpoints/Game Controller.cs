using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public bool DashUnlocked = false;


    //restart position after health become 0
    Vector2 RestartStartLevelPos;
    Vector2 startPos;
    Vector2 CheckPos;
    private HealthManager hm;
    private PlayerController pc;

    //for scrolls logic
    public int scrollsCollected = 0;
    private int totalScrollsRequired = 3;
    public TMP_Text currentScrolls;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

       
    }



    void Start()
    {
        RestartStartLevelPos = transform.position;
        startPos = transform.position;
        hm = GameObject.Find("Health Manager").GetComponent<HealthManager>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();

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
        StartCoroutine(Respawn());

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

        else if (collision.tag == "Lightning")
        {

            Debug.Log("Damage");
            hm.TakeDamage(15);
            pc.PlayHurtAnim();
            if (hm.healthAmount < 1)
            {
                pc.Damage();
                hm.healthAmount = 100;
                hm.healthFill.fillAmount = 1;

                if (pc.lives > 1)
                {
                    StartCoroutine(Respawn());
                }

            }
        }

        else if (collision.tag == "Arrow")
        {
            hm.TakeDamage(5);
            pc.PlayHurtAnim();
            collision.gameObject.SetActive(false);

            if (hm.healthAmount < 1)
            {
                pc.Damage();
                hm.healthAmount = 100;
                hm.healthFill.fillAmount = 1;
                StartCoroutine(Respawn());

                if (pc.lives > 1)
                {
                    StartCoroutine(Respawn());
                }
            }
        }

        else if (collision.tag == "EArrow")
        {
            hm.TakeDamage(5);
            pc.PlayHurtAnim();
            collision.gameObject.SetActive(false);

            if (hm.healthAmount < 1)
            {
                pc.Damage();
                hm.healthAmount = 100;
                hm.healthFill.fillAmount = 1;
                StartCoroutine(Respawn());

                if (pc.lives > 1)
                {
                    StartCoroutine(Respawn());
                }
            }
        }

        else if (collision.tag == "Saw")
        {
            hm.TakeDamage(5);
            pc.PlayHurtAnim();
            if (hm.healthAmount < 1)
            {
                pc.Damage();
                hm.healthAmount = 100;
                hm.healthFill.fillAmount = 1;

                if (pc.lives > 1)
                {
                    StartCoroutine(Respawn());
                }

            }
        }

        else if (collision.tag == "resolve")
        {
            hm.FillResolve(33.33f);
            Destroy(collision.gameObject);
        }


    }

    //for scrolls
    public void CollectScroll()
    {
        //increment krte jao scrolls ka
        scrollsCollected++;
    }

    //for checking all scrolls collected or not
    public bool HasAllScrolls()
    {
        return scrollsCollected >= totalScrollsRequired;
    }

}
