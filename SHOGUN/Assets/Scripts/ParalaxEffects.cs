using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffects : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float ParalaxEffect;


    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

            }
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - ParalaxEffect));
        float dist = (cam.transform.position.x * ParalaxEffect);
        transform.position = new Vector3 (startpos + dist, transform.position.y,transform.position.z);

        if(temp > startpos + length)
        {
           startpos += length;
        }

        else if(temp < startpos - length)
        {
           startpos -= length;
        }
    }
}
