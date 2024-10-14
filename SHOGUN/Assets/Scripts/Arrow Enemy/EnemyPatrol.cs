using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    protected Transform pointA, pointB;
    protected Vector3 CurrenTarget;
    [SerializeField]
    private float speed = 5.0f;
    private bool facingRight = true;

    //private Player player;
    private float distance;
    private SpriteRenderer _sprite;
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pointA.position)
        {
            //Debug.Log("PointA");
            CurrenTarget = pointB.position;
          
        }


        else if (transform.position == pointB.position)
        {
            CurrenTarget = pointA.position;
           
        }
           
        transform.position = Vector3.MoveTowards(transform.position, CurrenTarget, speed * Time.deltaTime);

        

    }


}
