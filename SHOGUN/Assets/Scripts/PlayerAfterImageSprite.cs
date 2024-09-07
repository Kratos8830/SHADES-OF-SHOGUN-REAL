using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    private Transform player;
    public SpriteRenderer sr;
    public SpriteRenderer playerSR;

    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaset = 0.8f;
    private float alphamultiplier;
    private Color color;

    private void OnEnable()
    {
        sr=GetComponent<SpriteRenderer>();
        player=GameObject.FindGameObjectWithTag("Player").transform;
        playerSR=player.GetComponentInChildren<SpriteRenderer>();

        alpha=alphaset;
        sr.sprite=playerSR.sprite;  
        transform.position=player.position;
        transform.rotation=player.rotation;
        timeActivated=Time.time;
    }

    private void Update()
    {
        alpha *= alphamultiplier;
        color=new Color(1f,1f,1f,alpha);
        sr.color=color;

        if(Time.time>= (timeActivated+activeTime))
        {
            //object pooling
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
