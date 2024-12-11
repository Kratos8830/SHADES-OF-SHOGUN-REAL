using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public Transform firepoint;
    [SerializeField] public GameObject NinjaStarPrefeb;
    private Animator anim;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {

            Shoot();
        }
    }
    private void Shoot()
    {

        Instantiate(NinjaStarPrefeb, firepoint.position, firepoint.rotation);
    }
}
