using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
   
    public Image healthFill;
    public float healthAmount = 100f;
    public TMP_Text currenthealth;
    private GameController gm;

    private void Start()
    {
        currenthealth.text = "100";
        gm= GameObject.Find("Player").GetComponent<GameController>();
        healthAmount = 100f;
    }



    // Update is called once per frame
    void Update()
    {
        currenthealth.text = ""+ healthAmount; 

        if(healthAmount<1)
        {
            gm.ZeroHealth();
        }
    }

    public void TakeDamage(float Damage)
    {
        healthAmount -= Damage;
        healthFill.fillAmount = healthAmount / 100f;
    }

    //public void Heal(float healingAmount)
    //{
    //    healthAmount += healingAmount;
    //    healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
    //    healthFill.fillAmount = healthAmount / 100f;
    //}

   
}


